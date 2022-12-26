using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjectsLayer; //referencing the solid object layer

    //this will be private so it will not be changed
    private bool isMoving;

    //keyboard input
    private Vector2 input;
    //reference to joystick for input
    public Joystick joystick;

    //animator
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            //if the player is not moving, will check for input and try
            //to move the player
            //input.x = Input.GetAxisRaw("Horizontal"); 
            //input.y = Input.GetAxisRaw("Vertical");
            input.x = joystick.Horizontal;
            input.y = joystick.Vertical;
            //axisRaw = input will always be 1 or -1

            //to make sure the player do not move diagonally
            //only one of the input will be 0
            //if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x);
                animator.SetFloat("moveY", input.y);

                //target position = current position plus the input
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;

                if (IsWalkable(targetPos))
                {
                    //calling the co-routine function
                    StartCoroutine(Move(targetPos));
                }            
            }
        }

        animator.SetBool("isMoving", isMoving);
    }

    //this co-routine function is to know the player current position to its target
    //over a period of time 
    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        //if the difference between the current position and target position
        //is greater than a small value, 
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            //then it will move the player current position by small amount
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
            //this will keep repeating until the current position and the target position
            //are really close until this loop is false
        }
        //the position then will be set to the target position
        transform.position = targetPos;

        isMoving = false;
    }

    //will check if the tile is walkable
    private bool IsWalkable(Vector3 targetPos)
    {
       if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null){
            return false;
        }

        return true;
    }

}
