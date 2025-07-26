using UnityEngine;
using UnityEngine.UI;

public class CoinPurchase : MonoBehaviour
{

    [SerializeField] private Button button;

    public void handleScrollValueChange(int points)
    {
        Debug.Log("Scroll Position: " + points);
        int totalCoins = GameData.GetPlayerScore(0) + points;
        GameData.SetPlayerScore(0, totalCoins);
        LoadInto.UpdateTotalCoins(totalCoins, GameObject.Find("GameObject"));
    }
}
