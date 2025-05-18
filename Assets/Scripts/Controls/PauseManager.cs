using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager _instance;

    private void Awake()
    {
        _instance = this;
    }

    public void OnPauseGame()
    {
        Time.timeScale = 0;
    }

    public void OffPauseGame()
    {
        Time.timeScale = 1;
    }
}