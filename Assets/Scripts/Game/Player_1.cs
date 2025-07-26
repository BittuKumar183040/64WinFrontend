using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SelfCard : MonoBehaviour
{
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text totalCoinsText;
    [SerializeField] private TMP_Text statusText;
    [SerializeField] private Image avatarImage;

    void Start()
    {
        LoadInto.Player(0, totalCoinsText, usernameText, avatarImage, statusText);
    }

}
