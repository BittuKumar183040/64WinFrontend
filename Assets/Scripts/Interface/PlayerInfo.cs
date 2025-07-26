using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo
{
    public string Avatar;
    public string Username;
    public int TotalCoins;
    public ConnectionStatus Status;
    public string Card;

    public PlayerInfo(string avatar, string username, int totalCoins, ConnectionStatus status, string card)
    {
        Avatar = avatar;
        Username = username;
        TotalCoins = totalCoins;
        Status = status;
        Card = card;
    }
}
