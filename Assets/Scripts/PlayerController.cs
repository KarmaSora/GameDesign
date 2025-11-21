using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody playerRigidBody;
    public float playerSPeed = 10.0f;
    public float playerJumpForce = 25.0f;

    public GameObject focalPoint;

    public bool isGrounded;
    public float gravityModifier = 1;

    public float playerHealth = 100.0f;
    public int playerLives = 3;



    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("FocalPoint");
        Physics.gravity *= gravityModifier;


    }


    private void FixedUpdate()
    {
        
    }


    
    // Update is called once per frame
    void Update()
    {
        float veritcalInput = Input.GetAxis("Vertical");

        float horizontal = Input.GetAxis("Horizontal");


        //playerRigidBody.AddForce(focalPoint.transform.forward * veritcalInput * playerSPeed);
        //playerRigidBody.AddForce(focalPoint.transform.right * horizontal * playerSPeed);
        playerRigidBody.transform.Translate(Vector3.forward * veritcalInput * Time.deltaTime * playerSPeed );
        playerRigidBody.transform.Translate(Vector3.right * horizontal * Time.deltaTime * playerSPeed );


        if (Input.GetKeyDown(KeyCode.Space)  && isGrounded)
        {
            playerRigidBody.AddForce(Vector3.up * playerJumpForce, ForceMode.Impulse);

            isGrounded = false;
        }


    }


    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }


}
