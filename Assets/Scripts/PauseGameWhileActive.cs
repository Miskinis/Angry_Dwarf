using UnityEngine;

public class PauseGameWhileActive : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 1f;
    }

    private void OnDisable()
    {
        Time.timeScale = 1f;
    }
}