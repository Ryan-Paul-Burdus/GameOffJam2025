using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Dash")]
    public bool IsDashing;
    public AbilityObject DashAbility;
    private TrailRenderer dashTrailRenderer;

    private float timeCurrentlyDashing = 0;

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

        if (IsDashing)
        {
            timeCurrentlyDashing += Time.deltaTime;

            // Dash
            rb.linearVelocity = transform.up * PlayerManager.Instance.DashSpeed;

            if (timeCurrentlyDashing >= PlayerManager.Instance.DashDuration)
            {
                timeCurrentlyDashing = 0;
                IsDashing = false;
                DashAbility.IsCoolingDown = true;
            }
        }
        else
        {
            // Moves the player forward all the time
            rb.linearVelocity = transform.up * PlayerManager.Instance.MoveSpeed;
        }
    }

    #region Inputs

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

    /// <summary>
    /// Dashes the player in the direction the player is looking
    /// </summary>
    /// <param name="context">The input manager action</param>
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && !IsDashing && !DashAbility.IsCoolingDown)
        {
            IsDashing = true;
        }
    }

    #endregion Inputs
}
