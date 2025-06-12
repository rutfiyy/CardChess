using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void ShowHowToPlay()
    {
        Debug.Log("How to Play screen not yet implemented.");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit pressed in Editor");
    }
}
