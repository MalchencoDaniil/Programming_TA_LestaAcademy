using UnityEngine;

namespace Player
{
    public class PlayerInput
    {
        public Vector2 MovementInput()
        {
            return InputManager._instance._inputActions.Player.Movement.ReadValue<Vector2>();
        }

        public bool CanJump()
        {
            return InputManager._instance._inputActions.Player.Jump.triggered;
        }
    }
}