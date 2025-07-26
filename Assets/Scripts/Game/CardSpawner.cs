using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class CardSpawner : MonoBehaviour
{
    public GameObject cardPrefab;
    public Transform tableCenter;
    public Transform lookTargetTransform;

    void Start()
    {
        SpawnCards(GameData.playerCount);
    }

    void SpawnCards(int playerCount)
    {
        IReadOnlyList<SpawnData> spawnData = GetFixedSpawnTransforms(playerCount);

        for (int i = 0; i < spawnData.Count; i++)
        {
            var data = spawnData[i];

            GameObject card = Instantiate(
                cardPrefab,
                data.position,
                Quaternion.Euler(data.rotation)
            );
            card.name = $"Player_{i + 1}";

            TextMeshPro status = card.transform.Find("Status")?.GetComponent<TextMeshPro>();
            TextMeshPro usernameText = card.transform.Find("Username")?.GetComponent<TextMeshPro>();
            TextMeshPro totalCoins = card.transform.Find("Coin/TotalCoins")?.GetComponent<TextMeshPro>();
            SpriteRenderer avatarRenderer = card.transform.Find("Avatar")?.GetComponent<SpriteRenderer>();

            if (status && usernameText && avatarRenderer && totalCoins)
            {
                LoadInto.Player(i + 1, totalCoins, usernameText, avatarRenderer, status);
            }
            else
            {
                Debug.LogWarning($"Missing components on card {card.name}:\n" +
                    $"Status: {(status ? "OK" : "MISSING")}, " +
                    $"Username: {(usernameText ? "OK" : "MISSING")}, " +
                    $"PlayedCoins: {(totalCoins ? "OK" : "MISSING")}, " +
                    $"Avatar: {(avatarRenderer ? "OK" : "MISSING")}");
            }
        }
    }


    IReadOnlyList<SpawnData> GetFixedSpawnTransforms(int count)
    {
        var list = new List<SpawnData>();

        if (count >= 2) list.Add(new SpawnData(
            new Vector3(-50f, -3f, 75f),
            new Vector3(-90f, 0f, -35f)
        ));
        if (count >= 3) list.Add(new SpawnData(
            new Vector3(0f, -3f, 125f),
            new Vector3(-90f, 0f, 0f)
        ));
        if (count >= 4) list.Add(new SpawnData(
            new Vector3(50f, -3f, 75f),
            new Vector3(-90f, 0f, 35f)
        ));

        return list;
    }
}

public struct SpawnData
{
    public Vector3 position;
    public Vector3 rotation;

    public SpawnData(Vector3 pos, Vector3 rot)
    {
        position = pos;
        rotation = rot;
    }
}
