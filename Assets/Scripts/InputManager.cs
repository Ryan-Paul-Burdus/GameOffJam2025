using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public static Vector2 Movement;

    private PlayerInput PlayerInput;
    private InputAction MoveAction;
    private InputAction DashAction;

    public void Awake()
    {
        if (Instance != null && Instance != this) // if we are the instance this is fine
        {
            Destroy(this);
            return;
        }
        Instance = this;

        PlayerInput = GetComponent<PlayerInput>();
    }

}
