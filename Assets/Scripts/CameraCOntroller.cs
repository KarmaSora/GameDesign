using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCOntroller : MonoBehaviour
{
    // Start is called before the first frame update


    //[SerializeField] private float mouseSensitivity = 220;
    private float xRotation = 0f;

    private Transform parent;


    [Header("Mouse Settings")]
    [SerializeField] private float mouseSensitivity = 150f;
    [SerializeField] private float minPitch = -80f;   // Look down limit
    [SerializeField] private float maxPitch = 80f;    // Look up limit

    [Header("References")]
    [SerializeField] private Transform yawTransform;  // Object rotated left/right (usually player body or camera pivot)

    private float pitch; // current x-rotation in degrees



    void Start()
    {


        parent = transform.parent.parent;

        yawTransform = parent.transform;

        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        //Rotate();
        //Rotate2();
        Rotate3();
    }


    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        parent.Rotate(Vector3.up, mouseX);
    }


    private void Rotate2()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Camera up/down on THIS object
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80, 80);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the outer parent left/right
        parent.Rotate(Vector3.up * mouseX, Space.Self);

    }
    private void Rotate3()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Horizontal rotation (yaw) on the yaw transform (player body / pivot)
        if (yawTransform != null)
        {
            yawTransform.Rotate(0f, mouseX, 0f, Space.Self);
        }

        // Vertical rotation (pitch) on the camera itself
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

    }




}
