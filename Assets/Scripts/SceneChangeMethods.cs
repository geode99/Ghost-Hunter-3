using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuMethods : MonoBehaviour
{
    public void StartButton()
    {
        Debug.Log("Started the game!");
        SceneManager.LoadScene("Enemy Testing");
    }

    public void QuitButton()
    {
        Debug.Log("Quit the game!");
        Application.Quit();
    }

    public void CreditsButton()
    {
        Debug.Log("Opened Credits!");
        SceneManager.LoadScene("Credits");
    }
    public void TitleButton()
    {
        Debug.Log("Opened Title!");
        SceneManager.LoadScene("TitleScreenScene");
    }
}