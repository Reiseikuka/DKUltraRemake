using System;
using UnityEngine;

public class PlayerInput
{
    private PlayerInputActions playerInputActions;

    public event Action OnJumpPressed;

    public void Initialize()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable(); //Activate the input map to be able to use controls

        playerInputActions.Player.Jump.started += Jump_started;
    }

    private void Jump_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJumpPressed?.Invoke();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;
        return inputVector;
    }
}
