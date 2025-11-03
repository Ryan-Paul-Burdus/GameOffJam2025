using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    private Vector2 movement;
    private Rigidbody2D rb;

    [Header("Dash")]
    public float DashSpeed = 20f;
    public float DashDuration = 0.05f;
    public float DashCooldown = 0.1f;
    private bool isDashing;
    private bool canDash = true;
    private TrailRenderer dashTrailRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Stuff that can be done whilst dashing goes here

        if (isDashing)
        {
            return;
        }

        // Stuff we dont want happening when dashing goes here
    }

    /// <summary>
    /// Moves the player from given inputs
    /// </summary>
    /// <param name="context">The input manager action</param>
    public void Move(InputAction.CallbackContext context)
    {
        rb.linearVelocity = context.ReadValue<Vector2>() * moveSpeed;
    }

    #region Dash

    /// <summary>
    /// Dashes the player in the direction the player is looking
    /// </summary>
    /// <param name="context">The input manager action</param>
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    /// <summary>
    /// Coroutine containing the dash functionality
    /// </summary>
    /// <returns></returns>
    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        canDash = false;
        //dashTrailRenderer.emitting = true;

        // Dash
        rb.linearVelocity = InputManager.Movement * DashSpeed;

        yield return new WaitForSeconds(DashDuration);

        // Stop the dash movement
        rb.linearVelocity = InputManager.Movement * moveSpeed;

        isDashing = false;
        //dashTrailRenderer.emitting = false;

        yield return new WaitForSeconds(DashCooldown);
        canDash = true;
    }

    #endregion Dash
}
