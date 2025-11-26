using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCOntroller : MonoBehaviour
{
    // Start is called before the first frame update


    [SerializeField] private float mouseSentintivity = 220;
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
    }


    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSentintivity * Time.deltaTime;

        parent.Rotate(Vector3.up, mouseX);
    }

}
