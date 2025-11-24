using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Dash")]
    public bool IsDashing;
    private bool canDash = true;
    private TrailRenderer dashTrailRenderer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (PickupManager.Instance.PickupUIVisibile || MenuManager.Instance.IsPaused)
        {
            rb.linearVelocity = Vector3.zero;
            return;
        }

        /// Stuff that can be done whilst dashing goes here

        if (IsDashing)
        {
            return;
        }

        /// Stuff we dont want happening when dashing goes here

        // Moves the player forward all the time
        rb.linearVelocity = transform.up * PlayerManager.Instance.MoveSpeed;
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
        IsDashing = true;
        canDash = false;
        //dashTrailRenderer.emitting = true;

        // Dash
        rb.linearVelocity = transform.up * PlayerManager.Instance.DashSpeed;

        yield return new WaitForSeconds(PlayerManager.Instance.DashDuration);

        // Stop the dash movement
        rb.linearVelocity = transform.up * PlayerManager.Instance.MoveSpeed;

        IsDashing = false;
        //dashTrailRenderer.emitting = false;

        yield return new WaitForSeconds(PlayerManager.Instance.DashCooldown);
        canDash = true;
    }

    #endregion Dash
}
