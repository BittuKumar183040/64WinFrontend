using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCard : MonoBehaviour
{
    [Header("UI References")]
    public Image avatar;
    public TextMeshProUGUI username;
    public TextMeshProUGUI totalCoins;
    public TextMeshProUGUI status;

    private void Awake()
    {
        if (avatar == null)
            avatar = transform.Find("Avatar").GetComponent<Image>();

        if (username == null)
            username = transform.Find("Username").GetComponent<TextMeshProUGUI>();

        if (totalCoins == null)
            totalCoins = transform.Find("Coin/TotalCoins").GetComponent<TextMeshProUGUI>();

        if (status == null)
            status = transform.Find("Status").GetComponent<TextMeshProUGUI>();
    }

    public void SetData(PlayerInfo info)
    {
        Sprite avatarSprite = Resources.Load<Sprite>(info.Avatar);
        if (avatarSprite != null)
        {
            avatar.sprite = avatarSprite;
        }
        else
        {
            Debug.LogWarning($"Avatar not found: {info.Avatar}");
        }

        username.text = info.Username;
        totalCoins.text = info.TotalCoins.ToString();
        status.text = info.Status.ToString();
        ApplyStatusStyle(info.Status);
    }

    public void UpdateScore(PlayerCard playerCard, int newScore)
    {
        if (playerCard != null)
        {
            playerCard.totalCoins.text = newScore.ToString();
        }
        else
        {
            Debug.LogWarning("PlayerCard reference is null.");
        }
    }

    private void ApplyStatusStyle(ConnectionStatus connectionStatus)
    {
        switch (connectionStatus)
        {
            case ConnectionStatus.CONNECTING:
                status.color = Color.yellow;
                status.fontStyle = FontStyles.Italic;
                break;
            case ConnectionStatus.CONNECTED:
                status.color = Color.green;
                status.fontStyle = FontStyles.Bold;
                break;
            case ConnectionStatus.OFFLINE:
                status.color = new Color(0.5f, 0.5f, 0.5f);
                status.fontStyle = FontStyles.Normal;
                break;
            case ConnectionStatus.ELEMENATED:
                status.color = Color.red;
                status.fontStyle = FontStyles.Strikethrough;
                break;
            case ConnectionStatus.DISCONNECTED:
                status.color = new Color(1f, 0.4f, 0f);
                status.fontStyle = FontStyles.Italic;
                break;
            default:
                status.color = Color.white;
                status.fontStyle = FontStyles.Normal;
                break;
        }
    }

}
