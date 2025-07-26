using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public static class LoadInto
{
    public static void Player(
        int playerIndex,
        TMP_Text totalCoinsText,
        TMP_Text usernameText,
        Component avatarTarget,
        TMP_Text statusText)
    {
        PlayerInfo p = GameData.players[playerIndex];
        if (p == null) return;

        totalCoinsText.text = p.TotalCoins.ToString();
        usernameText.text = p.Username;
        statusText.text = p.Status.ToString();

        Sprite avatar = Resources.Load<Sprite>(p.Avatar);
        if (avatar == null)
        {
            Debug.LogWarning($"Avatar '{p.Avatar}' not found in Resources.");
            return;
        }

        if (avatarTarget is SpriteRenderer sr)
        {
            sr.sprite = avatar;
        }
        else if (avatarTarget is Image img)
        {
            img.sprite = avatar;
        }
        else
        {
            Debug.LogWarning("Unsupported avatar target type. Must be SpriteRenderer or UI.Image.");
        }
    }

    public static void UpdateTotalCoins(int value, GameObject playerCard)
    {
        Transform coinText = playerCard.transform.Find("Coin/TotalCoins");
        if (coinText != null)
        {
            TMP_Text TotalCoinsText = coinText.GetComponent<TMP_Text>();
            TotalCoinsText.text = value.ToString();
        }
    }

    public static void UpdateTotalCoins(int value, TMP_Text TotalCoinsText)
    {
        TotalCoinsText.text = value.ToString();
    }

}
