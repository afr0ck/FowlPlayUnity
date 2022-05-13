using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Transform pivot;

    public Vector3 offset;
    public bool useOffSet;

    public float turnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        if (!useOffSet)
        {
            offset = target.position - transform.position;
        }

        // Sets pivot as child of target with same position
        pivot.position = target.position;
        pivot.parent = null;

        // Turns cursor off so not shown on screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        pivot.position = target.position;

        // Rotate player based on mouse
        float horizontal = Input.GetAxis("Mouse X") * turnSpeed;
        pivot.Rotate(0, horizontal, 0);

        // Rotate pivot for vertical camera movement
        float vertical = Input.GetAxis("Mouse Y") * turnSpeed;
        pivot.Rotate(-vertical, 0, 0);

        // Limit camera angles
        /*if (pivot.rotation.eulerAngles.x > 45f && pivot.rotation.eulerAngles.x < 180f)
        {
            pivot.rotation = Quaternion.Euler(45f, 0, 0);
        }*/

        /*if (pivot.rotation.eulerAngles.x >= 180f && pivot.rotation.eulerAngles.x < 315f)
        {
            pivot.rotation = Quaternion.Euler(315f, 0, 0);
        }*/


        // Get specific rotation angles for new camera angle
        float desiredYAngle = pivot.eulerAngles.y;
        float desiredXAngle = pivot.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);

        // Apply all camera adjustments
        transform.position = target.position - (rotation * offset);

        // Prevent looking underground
        if (transform.position.y < target.position.y)
        {
            transform.position = new Vector3(transform.position.x, target.position.y - .5f, transform.position.z);
        }

        transform.LookAt(target);
    }
}
