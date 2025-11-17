using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform TargetToFollow;

    // Update is called once per frame
    void Update()
    {
        if (PickupManager.Instance.PickupUIVisibile)
        {
            return;
        }

        transform.position = new Vector3(TargetToFollow.position.x, TargetToFollow.position.y, -10f);
    }
}
