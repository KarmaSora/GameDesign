using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody playerRigidBody;
    [SerializeField] private float playerMoveSpeed = 10.0f;
    [SerializeField] private float playerJumpForce = 25.0f;

    public GameObject focalPoint;
    public bool isGrounded;
    public float gravityModifier = 1;



    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;





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


        //playerRigidBody.AddForce(focalPoint.transform.forward * veritcalInput * playerMoveSpeed);
        //playerRigidBody.AddForce(focalPoint.transform.right * horizontal * playerMoveSpeed);
        playerRigidBody.transform.Translate(Vector3.forward * veritcalInput * Time.deltaTime * playerMoveSpeed );
        playerRigidBody.transform.Translate(Vector3.right * horizontal * Time.deltaTime * playerMoveSpeed );


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
