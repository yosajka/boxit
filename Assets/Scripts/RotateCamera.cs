using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class RotateCamera : MonoBehaviour {
 
    public Transform target;
    public float distance = 10f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 20f;

    Vector3 point;
    //float zoomspeed = 5.0f;

    private new Rigidbody rigidbody;

    float x = 0.0f;
    float y = 0.0f;

    float mouseX = 0f;
    float mouseY = 0f;

    // Use this for initialization
    void Start()
    {

        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
    }

    void LateUpdate()
    {
        if (target)
        {
            GetMouseButtonDown_XY();

            x += mouseX * xSpeed * distance * 0.02f;
            y -= mouseY * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);

            RaycastHit hit;
            if (Physics.Linecast(target.position, transform.position, out hit))
            {
                distance -= hit.distance;
            }
            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
        
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    Vector3 mousePosPrev;
    void GetMouseButtonDown_XY()
    {
        if (Input.GetMouseButtonDown(1))
        {
            mousePosPrev = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        } 

        if (Input.GetMouseButton(1))
        {
            Vector3 newMousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);

            if (newMousePos.x < mousePosPrev.x)
            {
                mouseX = -1;
            } else if (newMousePos.x > mousePosPrev.x)
            {
                mouseX = 1;
            } else
            {
                mouseX = -0;
            }

            if (newMousePos.y < mousePosPrev.y)
            {
                mouseY = -1;
            }
            else if (newMousePos.y > mousePosPrev.y)
            {
                mouseY = 1;
            }
            else
            {
                mouseY = -0;
            }

            mousePosPrev = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        }
    }
}

