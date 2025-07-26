using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerCounter : MonoBehaviour
{
    [SerializeField] public TMP_Dropdown dropButton;

    void Start()
    {
        if (dropButton != null)
        {
            for (int i = 0; i < dropButton.options.Count; i++)
            {
                string optionText = dropButton.options[i].text;
                if (optionText.Length > 1 && int.TryParse(optionText.Substring(1), out int count))
                {
                    if (count == GameData.playerCount)
                    {
                        dropButton.value = i;
                        break;
                    }
                }
            }
        }
    }

    public void GetPlayerCount()
    {
        string selectedPlayer = dropButton.options[dropButton.value].text;
        int players = int.Parse(selectedPlayer.Substring(1));
        GameData.playerCount = players;
        AssignRandomPlayersForFirstMatch();
    }
    public void AssignRandomPlayersForFirstMatch()
    {
        int playerCount = GameData.playerCount;
        List<int> allIndexes = new List<int>();
        for (int i = 0; i < playerCount; i++)
        {
            allIndexes.Add(i);
        }

        for (int i = allIndexes.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = allIndexes[i];
            allIndexes[i] = allIndexes[j];
            allIndexes[j] = temp;
        }

        GameData.activePlayersIndexes = new List<int> { allIndexes[0], allIndexes[1] };
    }
}