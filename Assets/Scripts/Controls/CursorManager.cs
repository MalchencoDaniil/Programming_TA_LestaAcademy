using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager _instance;

    public enum CursorState
    {
        Locked,
        Visible,
        Unvisible,
        UnLocked
    }

    public CursorState _cursorState;

    private void Awake()
    {
        _instance = this;

        UpdateCursorState(_cursorState);
    }

    public void UpdateCursorState(CursorState _newCursorState)
    {
        _cursorState = _newCursorState;

        switch (_cursorState)
        {
            case CursorState.Locked:
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case CursorState.UnLocked:
                Cursor.lockState = CursorLockMode.None;
                break;
            case CursorState.Unvisible:
                Cursor.visible = false;
                break;
            case CursorState.Visible:
                Cursor.visible = true;
                break;
        }
    }
}