using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class PlayerInformation : MonoBehaviour
{
    [SerializeField] private Component player1UI;
    [SerializeField] private TextMeshProUGUI player1NameText;
    [SerializeField] private Component player2UI;
    [SerializeField] private TextMeshProUGUI player2NameText;
    [SerializeField] private Component player3UI;
    [SerializeField] private TextMeshProUGUI player3NameText;
    [SerializeField] private Component player4UI;
    [SerializeField] private TextMeshProUGUI player4NameText;

    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color activeColor = Color.yellow;

    private GameObject[] playerUIs;
    private TextMeshProUGUI[] playerNameTexts;

    private List<int> previousActiveIndexes = new();

    void Start()
    {
        playerUIs = new GameObject[]
        {
            player1UI.gameObject,
            player2UI.gameObject,
            player3UI.gameObject,
            player4UI.gameObject
        };

        playerNameTexts = new TextMeshProUGUI[]
        {
            player1NameText,
            player2NameText,
            player3NameText,
            player4NameText
        };

        int playerCount = GameData.playerCount;

        for (int i = 0; i < playerUIs.Length; i++)
        {
            bool isActive = i < playerCount;
            playerUIs[i].SetActive(isActive);

            if (isActive)
            {
                playerNameTexts[i].text = GameData.GetPlayerInfo(i).Username;
            }
        }

        UpdateActivePlayerHighlights(GameData.activePlayersIndexes);
    }

    void Update()
    {
        List<int> currentActiveIndexes = GameData.activePlayersIndexes;

        if (!Enumerable.SequenceEqual(previousActiveIndexes, currentActiveIndexes))
        {
            UpdateActivePlayerHighlights(currentActiveIndexes);
        }
    }

    private void UpdateActivePlayerHighlights(List<int> activeIndexes)
    {
        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            if (playerUIs[i].activeSelf)
            {
                playerNameTexts[i].color = activeIndexes.Contains(i) ? activeColor : normalColor;
            }
        }

        previousActiveIndexes = new List<int>(activeIndexes);
    }
}
