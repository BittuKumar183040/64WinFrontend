using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayedCoinsChanger : MonoBehaviour
{
    [SerializeField] private TMP_Text PlayedCoinsText;
    [SerializeField] private Slider playedCoinsSlider;

    private void Start()
    {
        if (playedCoinsSlider != null)
        {
            playedCoinsSlider.maxValue = GameData.players[0].TotalCoins;
            playedCoinsSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        if (PlayedCoinsText != null)
        {
            PlayedCoinsText.text = GameData.players[0].TotalCoins.ToString();
        }
    }

    private void OnSliderValueChanged(float value)
    {
        LoadInto.UpdateTotalCoins(Mathf.RoundToInt(value), PlayedCoinsText);
    }
}
