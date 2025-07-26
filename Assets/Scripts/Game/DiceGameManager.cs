using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct SpawnTransform
{
    public Vector3 position;
    public Vector3 rotation;

    public SpawnTransform(Vector3 pos, Vector3 rot)
    {
        position = pos;
        rotation = rot;
    }
}

public class DiceGameManager : MonoBehaviour
{
    [SerializeField] private Button dropButton;
    [SerializeField] private List<DropDice> diceList;
    [SerializeField] private TextMeshPro resultTextMesh;
    [SerializeField] private float PlayerWaitTime = 1f;
    [SerializeField] private Image SelfPlayerCardHighlight;
    [SerializeField] private Transform DiceTargetPointTransform;


    private readonly Dictionary<int, SpawnTransform> _spawnTransforms = new()
    {
        { 0, new SpawnTransform(new Vector3(0f, 0f, 50f), new Vector3(45f, 0f, 0f)) },
        { 1, new SpawnTransform(new Vector3(-35f, 0f, 75f), new Vector3(45f, -90f, 0f)) },
        { 2, new SpawnTransform(new Vector3(0f, 0f, 105f), new Vector3(45f, -180f, 0f)) },
        { 3, new SpawnTransform(new Vector3(35f, 0f, 75f), new Vector3(45f, -90f, 0f)) }
    };

    private void Start()
    {
        if (dropButton != null)
        {
            dropButton.onClick.RemoveAllListeners();
            dropButton.onClick.AddListener(DropAllDice);
        }
        if (GameData.currentPlayerIndex != 0)
        {
            //CheckPlayerIsValid();
            DropAllDice();
        }
    }

    //private bool CheckPlayerIsValid()
    //{
    //    if (false)
    //    {
    //        Debug.LogError($"Invalid player index: {GameData.currentPlayerIndex}. Valid range is 0 to {GameData.playerCount - 1}.");
    //        return false;
    //    }
    //    return true;
    //}

    private void DropAllDice()
    {
        dropButton.interactable = false;
        resultTextMesh.text = "";

        int players = GameData.playerCount;

        if (!_spawnTransforms.TryGetValue(GameData.currentPlayerIndex, out var chosenSpawn))
        {
            Debug.LogWarning($"No spawn position defined for player index {GameData.currentPlayerIndex}");
            return;
        }

        Vector3 forceDirection = (DiceTargetPointTransform.position - chosenSpawn.position).normalized;
        int rowSize = Mathf.CeilToInt(Mathf.Sqrt(diceList.Count));
        for (int i = 0; i < diceList.Count; i++)
        {
            int row = i / rowSize;
            int col = i % rowSize;

            Vector3 offset = new Vector3(
                (col - rowSize / 2f) * 15f,
                0f,
                (row - rowSize / 2f) * 15f
            );

            diceList[i].DropWithSharedDirection(
                chosenSpawn.position + offset,
                Quaternion.Euler(chosenSpawn.rotation),
                forceDirection * 1.2f 
            );
        }

        StartCoroutine(WaitUntilDiceStops());
    }


    private IEnumerator WaitUntilDiceStops()
    {
        yield return new WaitForSeconds(0.5f);

        float stableTime = 0f;
        const float requiredStableDuration = 1.0f;
        const float velocityThreshold = 0.1f;

        List<int> lastResults = new List<int>(diceList.Count);
        for (int i = 0; i < diceList.Count; i++)
            lastResults.Add(diceList[i].GetResult());

        while (true)
        {
            bool allDiceStill = true;
            bool resultUnchanged = true;

            for (int i = 0; i < diceList.Count; i++)
            {
                var die = diceList[i];
                Rigidbody rb = die.GetComponent<Rigidbody>();

                if (rb != null && (rb.linearVelocity.magnitude > velocityThreshold || rb.angularVelocity.magnitude > velocityThreshold))
                {
                    allDiceStill = false;
                }

                int currentResult = die.GetResult();
                if (currentResult != lastResults[i])
                {
                    resultUnchanged = false;
                    lastResults[i] = currentResult;
                }
            }

            if (allDiceStill || resultUnchanged)
            {
                stableTime += Time.deltaTime;
                if (stableTime >= requiredStableDuration)
                    break;
            }
            else
            {
                stableTime = 0f;
            }

            yield return null;
        }

        List<int> diceResult = new List<int>();

        for (int i = 0; i < diceList.Count; i++)
        {
            int result = diceList[i].GetResult();
            diceResult.Add(result);
        }

        resultTextMesh.text = string.Join(" ", diceResult);
        GameData.diceResult = diceResult;

        PrepForNextPlayerMove();
    }

    private IEnumerator PlayerTurn()
    {
        HighlightActivePlayer();
        if (GameData.currentPlayerIndex == 0)
        {
            dropButton.interactable = true;
        }
        else
        {
            yield return new WaitForSeconds(PlayerWaitTime);
            dropButton.interactable = false;
            Start();
        }
    }
    private void HighlightActivePlayer()
    {
        if (GameData.currentPlayerIndex == 0)
        {
            SelfPlayerCardHighlight.gameObject.SetActive(true);
        }

        for (int i = 0; i < GameData.playerCount; i++)
        {
            string playerCard = $"Player_{i}";
            GameObject card = GameObject.Find(playerCard);
            if (card != null)
            {
                Transform highlight = card.transform.Find("Highlight");
                if (highlight != null)
                {
                    highlight.gameObject.SetActive(GameData.currentPlayerIndex == i);
                }
                else
                {
                    Debug.LogWarning($"No 'Highlight' child on {playerCard}");
                }
            }
        }
    }

    private void PrepForNextPlayerMove()
    {
        List<int> ActiveBefore = GameData.activePlayersIndexes;
        List<int> diceResult = GameData.diceResult;
        if ((
            diceResult[0] == 1 && diceResult[1] == 1) ||
            (diceResult[0] == 3 && diceResult[1] == 3) ||
            (diceResult[0] == 4 && diceResult[1] == 4) ||
            (diceResult[0] == 6 && diceResult[1] == 6
        ))
        {
            if (GameData.players[GameData.currentPlayerIndex].TotalCoins >= GameData.scoreDifference)
            {
                int score = GameData.SetPlayerScore(GameData.currentPlayerIndex, GameData.GetPlayerScore(GameData.currentPlayerIndex) - GameData.scoreDifference);
                GameObject playerCard = GameObject.Find($"Player_{GameData.currentPlayerIndex}");
                LoadInto.UpdateTotalCoins(score, playerCard);
            }
            ReplaceCurrentPlayerIndex();
        } 
        else
        {
            NoChangeNextPlayerTurn();
        }

        StartCoroutine(PlayerTurn());
    }

    private void NoChangeNextPlayerTurn()
    {
        int totalPlayers = GameData.playerCount;
        GameData.currentPlayerIndex = (GameData.currentPlayerIndex + 1) % totalPlayers;
        while (!GameData.activePlayersIndexes.Contains(GameData.currentPlayerIndex))
        {
            GameData.currentPlayerIndex = (GameData.currentPlayerIndex + 1) % totalPlayers;
        }
    }

    private void ReplaceCurrentPlayerIndex()
    {
        if (GameData.playerCount <= 2)
        {
            NoChangeNextPlayerTurn();
            return;
        }
        int newIndex = -1;
        int start = (GameData.currentPlayerIndex + 1) % GameData.playerCount;

        for (int i = 0; i < GameData.playerCount; i++)
        {
            int candidateIndex = (start + i) % GameData.playerCount;

            if (!GameData.activePlayersIndexes.Contains(candidateIndex))
            {
                newIndex = candidateIndex;
                break;
            }
        }

        if (newIndex != -1)
        {
            GameData.activePlayersIndexes.Remove(GameData.currentPlayerIndex);
            GameData.activePlayersIndexes.Add(newIndex);
            GameData.currentPlayerIndex = newIndex;
        }
        else
        {
            Debug.LogWarning("[ReplaceCurrentPlayerIndex] No valid replacement found.");
        }
    }



}
