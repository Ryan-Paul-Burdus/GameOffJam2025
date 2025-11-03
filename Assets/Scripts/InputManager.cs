using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 Movement;

    private PlayerInput PlayerInput;
    private InputAction MoveAction;
    private InputAction DashAction;

    public void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();
        MoveAction = PlayerInput.actions["Move"];
    }

    private void Update()
    {
        Movement = MoveAction.ReadValue<Vector2>();
    }
}
