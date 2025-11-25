using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    //NEWSCIRPT
    //Variables
    public GameObject focalPoint;
    private Rigidbody playerRigidBody;


    [SerializeField] private float playerMoveSpeed;

    [SerializeField] private float walkSpeed = 10;
    [SerializeField] private float runSpeed = 20;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravityModifier = -9.81f; 



    private Vector3 moveDirection;
    private Vector3 velocity;

    private CharacterController controller;
    void Start()
    {
        controller = GetComponent<CharacterController>();

        groundCheckDistance = 0.2f;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            //playerRigidBody.AddForce(Vector3.up * playerJumpForce, ForceMode.Impulse);

            isGrounded = false;
        }
    }

    private void Move()
    {

        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        
        float veritcalInput = Input.GetAxis("Vertical");

        moveDirection = new Vector3 (0f, 0f, veritcalInput);

        //check if grounded and stop applying gravity
        controller.Move(moveDirection * Time.deltaTime);

        if (true)
        {
            moveDirection *= playerMoveSpeed;

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }

            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                //Walk
                Walk();

            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                //RUN
                Run();

            }
            else if (moveDirection == Vector3.zero)
            {
                //Idle
                Idle();

            }
        }


        //Apply gravity to character
        velocity.y += gravityModifier * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);



    }


    private void Idle() { }
    private void Walk() { playerMoveSpeed = walkSpeed; }
    private void Run() { playerMoveSpeed = runSpeed; }




}
