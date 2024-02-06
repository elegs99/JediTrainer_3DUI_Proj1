using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenuController : MonoBehaviour
{
    public GameObject menu;
    public GameObject gameStats;
    public GameObject controlsMenu;
    private void Update()
    {
        menu.transform.LookAt(Camera.main.transform);
        menu.transform.forward *= -1; 
        controlsMenu.transform.LookAt(Camera.main.transform);
        controlsMenu.transform.forward *= -1;
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DisplaySettings()
    {
        menu.SetActive(false);
        controlsMenu.SetActive(true);
    }

    public void DisplayMainMenu()
    {
        menu.SetActive(true);
        controlsMenu.SetActive(false);
    }
}
