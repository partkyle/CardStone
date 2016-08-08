using System.Collections;
using UnityEngine;

class DragTransform : MonoBehaviour
{

    private bool dragging = false;
    private float distance;
    private float oldY;

    private Vector3 lastPoint = Vector3.zero;

    private Vector3 originalPosition;

    void Awake()
    {
        oldY = transform.position.y;
        originalPosition = transform.position;
    }


    void OnMouseDown()
    {
        Debug.Log("OnMouseDown");
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        dragging = true;

        originalPosition = transform.position;
    }

    void OnMouseUp()
    {
        dragging = false;
    }

    void Update()
    {
        Vector3 mouseDirection = lastPoint - Input.mousePosition;
        if (dragging)
        {
            Cursor.visible = false;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            // keep from going below the boards
            rayPoint.y = oldY;
            transform.position = rayPoint;
            Quaternion rot = transform.rotation;

            rot.z = .01f * mouseDirection.x;
            transform.rotation = rot;
        }
        else
        {
            Cursor.visible = true;

            Quaternion rot = transform.rotation;
            rot.z = 0;
            transform.rotation = rot;

            transform.position = originalPosition;

        }

        lastPoint = Input.mousePosition;
    }
}