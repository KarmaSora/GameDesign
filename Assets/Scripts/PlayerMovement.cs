using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    //NEWSCIRPT
    //Variables
    //private Rigidbody playerRigidBody;


    [SerializeField] private float playerMoveSpeed;

    public float moveSpeedIncreaser =1;
    [SerializeField] private float walkSpeed = 3 ;
    [SerializeField] private float runSpeed = 7 ;

    [SerializeField] private bool isGrounded;
    //[SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravityModifier = -9.81f;


    public float jumpIncreaser = 0; 

    [SerializeField] private float jumpHight = 3;




    private Vector3 moveDirection;
    private Vector3 velocity;

    private CharacterController controller;
    private Animator animator;

    [SerializeField] private Collider weaponCollidor;

    [SerializeField] private bool isBlocking;     // Read-only flag for other scripts

    [Header("Block Color Settings")]
    [SerializeField] private Color blockColor = Color.blue;

    private Renderer playerRenderer;

    private Material originalMaterial;
    private Material blockMaterial;

    private void Awake()
    {
        playerRenderer = GetComponentInChildren<Renderer>();

        originalMaterial = playerRenderer.material;
        blockMaterial = new Material(originalMaterial);
        blockMaterial.color = blockColor;
    }



    void Start()
    {
        controller = GetComponent<CharacterController>();

        //groundCheckDistance = 0.2f;
        animator = GetComponentInChildren<Animator>(); 


    }

    // Update is called once per frame
    void Update()
    {
        Move();


        if (!isBlocking && Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Attack());
            
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (!isBlocking)
                playerRenderer.material = blockMaterial;

                StartBlock();
            
        }
        else
        {
            if (isBlocking)
            {
                StopBlock();

                playerRenderer.material = originalMaterial;

                if (playerRenderer == null)
{
    Debug.LogError("PlayerMovement: No Renderer found in children. Cannot apply block color.");
    return;
}
            }
        }

    }


    private void Move()
    {

        isGrounded = controller.isGrounded;
        //isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        float veritcalInput = Input.GetAxis("Vertical"); //Move Z
        float horizontal = Input.GetAxis("Horizontal"); //Move X

        moveDirection = new Vector3 (horizontal, 0f, veritcalInput);
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


        }
            moveDirection *= playerMoveSpeed;


        //Apply gravity to character
        controller.Move(moveDirection * Time.deltaTime);


        velocity.y += gravityModifier * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);



    }


    private void Idle() { 
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }
    private void Walk() { 
        playerMoveSpeed = walkSpeed * moveSpeedIncreaser;
        animator.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
    }
    private void Run() {
        animator.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
        playerMoveSpeed = runSpeed * moveSpeedIncreaser;
    }
    private void jump()
    {

        velocity.y = Mathf.Sqrt((jumpHight + jumpIncreaser) * -2 * gravityModifier);

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


    private void EnableWeaponCollidor()
    {
        weaponCollidor.enabled = true;

    }
    private void disableWeaponCollidor()
    {

        weaponCollidor.enabled = false;
    }



    private void StartBlock()
    {
        isBlocking = true;

        if (animator != null)
        {
            animator.SetBool("isBlocking", true);

            // If you use a specific layer for block animations, enable it here
            int attackLayerIndex = animator.GetLayerIndex("Attack Layer");
            if (attackLayerIndex >= 0)
            {
                animator.SetLayerWeight(attackLayerIndex, 1f);
            }
        }
    }

    private void StopBlock()
    {
        isBlocking = false;

        if (animator != null)
        {
            animator.SetBool("isBlocking", false);

            int attackLayerIndex = animator.GetLayerIndex("Attack Layer");
            if (attackLayerIndex >= 0)
            {
                animator.SetLayerWeight(attackLayerIndex, 0f);
            }
        }
    }



    // Public read-only accessor so other scripts can check if we’re blocking
    public bool IsBlocking
    {
        get { return isBlocking; }
    }




}
