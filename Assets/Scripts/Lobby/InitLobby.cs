using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyUIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text totalCoinsText;
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Image avatarImage;

    void Start()
    {
        LoadInto.Player(0, totalCoinsText, usernameText, avatarImage, statusText);
        totalCoinsText.text = GameData.players[0].TotalCoins.ToString();
    }
}
