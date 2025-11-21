using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotationSpeed = 20;
    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void LateUpdate()
    {

        //float horizontalInp = Input.GetAxis("Horizontal");
        //transform.Rotate(Vector3.up, horizontalInp * rotationSpeed * Time.deltaTime);
        // Get mouse movement
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        // Vertical rotation (looking up/down)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation (turning left/right)
        if (transform.parent)
        {
            transform.parent.Rotate(Vector3.up * mouseX);
        }
    }
}
