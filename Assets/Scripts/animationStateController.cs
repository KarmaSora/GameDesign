using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    // Start is called before the first frame update

    public bool isWalking;
    public bool isRunning;
    public bool isIdle;
    public Animator animator;

    int isWalkingHash;
    int isRunningHash;

    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        Debug.Log(animator);
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPress = Input.GetKey("u");

        bool runPressed = Input.GetKey("left shift");


        if (forwardPress) {
            animator.SetBool(isWalkingHash, true);
        }

        if (!forwardPress)
        {
            animator.SetBool(isWalkingHash, false);
        }



        if (forwardPress && runPressed)
        {
            animator.SetBool(isRunningHash, true);
        }

        if (!forwardPress || !runPressed)
        {
            animator.SetBool(isRunningHash, false);
        }
    }
}
