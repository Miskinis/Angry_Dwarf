using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Scripting;

[DisallowMultipleComponent]
[DefaultExecutionOrder(-1000)]
public class UiController : MonoBehaviour
{
    public static UiController main;

    public PlayableDirector startGamePlayable;
    public GameObject startGamePanel;
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (main == null)
            main = this;
        else
            Destroy(this);
        startGamePanel.SetActive(true);
    }

    [Preserve]
    public void StartGame()
    {
        startGamePanel.SetActive(false);
        startGamePlayable.Play();
    }

    [Preserve]
    public void RestartGame()
    {
        SceneManager.LoadSceneAsync(0);
        gameOverPanel.SetActive(true);
    }

    [Preserve]
    public void ExitGame()
    {
        Application.Quit();
    }
}