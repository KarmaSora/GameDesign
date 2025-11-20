using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody playerRigidBody;
    public float playerSPeed = 10.0f;
    public float playerJumpForce = 5.0f;

    public GameObject focalPoint;

    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");


    }

    // Update is called once per frame
    void Update()
    {
        float veritcalInput = Input.GetAxis("Vertical");

        playerRigidBody.AddForce(focalPoint.transform.forward * veritcalInput * playerSPeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerRigidBody.AddForce(Vector3.up * playerJumpForce, ForceMode.Impulse);
        }

    }
}
