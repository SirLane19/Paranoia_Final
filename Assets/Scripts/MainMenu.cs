using UnityEngine;
// using UnityEngine.SceneManager; // <- yang ini error
using UnityEngine.SceneManagement; // <- troubleshoot dari internet, SceneManager ada di SceneManagement

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("HeadphoneScene");
        // Debug.Log("Masuk.");
    }

    public void QuitGame()
    {
        // Debug.Log("Keluar dari game.");
        Application.Quit();
    }
}
