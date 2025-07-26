using System.Collections.Generic;
using UnityEngine;

public static class GameData
{
    public static List<PlayerCard> playerCards = new List<PlayerCard>();

    public static int scoreDifference = 100;
    public static int playerCount = 4;
    public static List<int> diceResult = new List<int>();
    public static List<int> activePlayersIndexes = new List<int>{ 1, 0 };

    public static int currentPlayerIndex = activePlayersIndexes[0];

    public static List<PlayerInfo> players = new List<PlayerInfo>
    {
        new PlayerInfo("me", "Red", 1000, ConnectionStatus.CONNECTING, null),
        new PlayerInfo("Players/1", "Alice", 2000, ConnectionStatus.CONNECTING, null),
        new PlayerInfo("Players/2", "Bob", 3000, ConnectionStatus.CONNECTING, null),
        new PlayerInfo("Players/3", "Charlie", 4000, ConnectionStatus.CONNECTED, null),
    };

    
    public static int SetPlayerScore(int index, int newScore)
    {
        players[index].TotalCoins = newScore;
        return newScore;
    }

    public static int GetPlayerScore(int index)
    {
        return players[index].TotalCoins;
    }
    public static PlayerInfo GetPlayerInfo(int index)
    {
        return players[index];
    }

}
