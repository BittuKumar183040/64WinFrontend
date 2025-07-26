using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public int sceneIndex=2;
    public void OnButtonClicked()
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }
}
