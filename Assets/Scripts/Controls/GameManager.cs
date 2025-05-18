using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;

    private GameStates _gameStates;

    [Header("UI")]
    [SerializeField]
    private TMP_Text _gameStateText;

    [SerializeField]
    private GameObject _gameplayPanel;

    [SerializeField]
    private GameObject _gameStatePanel;

    private void Awake()
    {
        _instance = this;

        _gameStates = new GameStates();
    }

    private void PanelControl(bool _state)
    {
        _gameplayPanel.SetActive(_state);
        _gameStatePanel.SetActive(!_state);
    }

    private void Start()
    {
        PanelControl(true);
    }

    private void GameOver()
    {
        PanelControl(false);
        CursorManager._instance.UpdateCursorState(CursorManager.CursorState.UnLocked);
        PauseManager._instance.OnPauseGame();
    }

    public void Won()
    {
        GameOver();
        _gameStateText.text = _gameStates.won;
        _gameStateText.color = Color.green;
    }

    public void Loss()
    {
        GameOver();
        _gameStateText.text = _gameStates.loss;
        _gameStateText.color = Color.red;
    }
}