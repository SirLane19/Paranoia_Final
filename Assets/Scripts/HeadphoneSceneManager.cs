using UnityEngine;
using UnityEngine.SceneManagement;

public class HeadphoneSceneManager : MonoBehaviour
{
    public float displayDuration = 3f; // durasinya ya ka
    public string nextSceneName = "Cutscene_Intro";

    void Start()
    {
        Invoke("GoToNextScene", displayDuration);
    }

    void GoToNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}