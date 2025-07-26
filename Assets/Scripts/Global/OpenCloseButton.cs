using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Component Container;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    

    void Start()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            if (Container != null)
            {
                bool isActive = Container.gameObject.activeSelf;
                Container.gameObject.SetActive(!isActive);
            }
            else
            {
                Debug.LogWarning("Container is not assigned.");
            }
        });
    }

    
}
