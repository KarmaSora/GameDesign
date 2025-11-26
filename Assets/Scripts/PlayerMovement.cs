using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    //NEWSCIRPT
    //Variables
    private Rigidbody playerRigidBody;


    [SerializeField] private float playerMoveSpeed;

    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float runSpeed = 10;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravityModifier = -9.81f;

    [SerializeField] public float jumpHight = 7;



    private Vector3 moveDirection;
    private Vector3 velocity;

    private CharacterController controller;
    private Animator animator;

    [SerializeField] private Collider weaponCollidor;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        groundCheckDistance = 0.2f;
        animator = GetComponentInChildren<Animator>(); 


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

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Attack());
            

        }

    }

    private void Move()
    {

        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
        
        float veritcalInput = Input.GetAxis("Vertical"); //Move Z

        moveDirection = new Vector3 (0f, 0f, veritcalInput);
        moveDirection = transform.TransformDirection(moveDirection);

        //check if grounded and stop applying gravity

        if (isGrounded)
        {


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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                jump();
                Debug.Log("player should jump");
            }

            moveDirection *= playerMoveSpeed;

        }


        //Apply gravity to character
        controller.Move(moveDirection * Time.deltaTime);


        velocity.y += gravityModifier * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);



    }


    private void Idle() { 
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }
    private void Walk() { 
        playerMoveSpeed = walkSpeed;
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }
    private void Run() {
        animator.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
        playerMoveSpeed = runSpeed;
    }
    private void jump()
    {

        velocity.y = Mathf.Sqrt(jumpHight * -2 * gravityModifier);

    }
    private IEnumerator Attack()
    {
        EnableWeaponCollidor();
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 1);
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(0.9f);
        animator.SetLayerWeight(animator.GetLayerIndex("Attack Layer"), 0);
        disableWeaponCollidor();

    }

    private void EnableWeaponCollidor() {
        weaponCollidor.enabled = true;

    }
    private void disableWeaponCollidor() {
    
        weaponCollidor.enabled = false;
    }




}
