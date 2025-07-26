using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayActions : MonoBehaviour
{
    public TextMeshProUGUI playButtonText;
    public string loadingSceneName = "Game";
    public List<Selectable> uiElementsToDisable;
    private Coroutine activeCoroutine;
    private bool loading = false;

    public void OnPlayButtonClicked()
    {
        if (loading)
        {
            StopCoroutine(activeCoroutine);
            playButtonText.text = "PLAY";
            EnableUI();
            loading = false;
            Debug.Log("Loading cancelled.");
        }
        else
        {
            playButtonText.text = "Initiated";
            activeCoroutine = StartCoroutine(PreprocessingGameScreen());
        }
    }

    private IEnumerator PreprocessingGameScreen()
    {
        loading = true;
        DisableUI();

        int requestedPlayers = GameData.playerCount;
        List<PlayerInfo> players = GameData.players;

        yield return new WaitForSeconds(1f);

        if (requestedPlayers <= players.Count + 1)
        {
            playButtonText.text = "Loading...";
            yield return new WaitForSeconds(1f);

            SceneManager.LoadScene(loadingSceneName);
        }
        else
        {
            playButtonText.text = "Searching...";
        }

        loading = false;
        EnableUI();
    }

    private void DisableUI()
    {
        foreach (var item in uiElementsToDisable)
        {
            if (item != null)
                item.interactable = false;
        }
    }

    private void EnableUI()
    {
        foreach (var item in uiElementsToDisable)
        {
            if (item != null)
                item.interactable = true;
        }
    }

}
