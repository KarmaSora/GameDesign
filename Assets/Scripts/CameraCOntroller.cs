using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCOntroller : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] private float mouseSentintivity = 220;
    private float xRotation = 0f;

    private Transform parent;
    void Start()
    {
        parent = transform.parent.parent;
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        //Rotate2();
    }


    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSentintivity * Time.deltaTime;

        parent.Rotate(Vector3.up, mouseX);
    }


    private void Rotate2()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSentintivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSentintivity * Time.deltaTime;

        // Camera up/down on THIS object
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80, 80);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotate the outer parent left/right
        parent.Rotate(Vector3.up * mouseX);

    }




}
