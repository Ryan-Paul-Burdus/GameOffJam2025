using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    private Vector2 movement;
    private Rigidbody2D rb;

    [Header("Dash")]
    public float DashSpeed = 10f;
    public float DashDuration = 0.1f;
    public float DashCooldown = 1f;
    private bool isDashing;
    private bool canDash = true;
    private TrailRenderer dashTrailRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (PickupManager.Instance.PickupUIVisibile)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        /// Stuff that can be done whilst dashing goes here

        if (isDashing)
        {
            return;
        }

        /// Stuff we dont want happening when dashing goes here

        // Moves the player forward all the time
        rb.linearVelocity = transform.up * moveSpeed;
    }

    /// <summary>
    /// rotates the player to look at the mouse
    /// </summary>
    /// <param name="context">The input manager action</param>
    public void MoveMouse(InputAction.CallbackContext context)
    {
        if (PickupManager.Instance.PickupUIVisibile)
        {
            return;
        }

        Vector2 mousePosition = context.ReadValue<Vector2>();
        Vector2 objectPosition = (Vector2)Camera.main.WorldToScreenPoint(transform.position);
        Vector2 direction = (mousePosition - objectPosition).normalized;

        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
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
        rb.linearVelocity = transform.up * DashSpeed;

        yield return new WaitForSeconds(DashDuration);

        // Stop the dash movement
        rb.linearVelocity = transform.up * moveSpeed;

        isDashing = false;
        //dashTrailRenderer.emitting = false;

        yield return new WaitForSeconds(DashCooldown);
        canDash = true;
    }

    #endregion Dash
}
