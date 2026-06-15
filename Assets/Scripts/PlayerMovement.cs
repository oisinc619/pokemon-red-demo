using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    private Vector2 movement;
    public Animator animator;
    public VectorValue startingPosition;

    private void Start()
    {
        animator = GetComponent<Animator>();

        bool North = false;
        bool South = false;
        bool East = false;
        bool West = false;
        transform.position = startingPosition.initialValue;
    }

    // Update is called once per frame
    void Update()
    {
        //input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        //Stop diagonal movement
        if (Mathf.Abs(movement.x) > Mathf.Abs(movement.y))
        {
            movement.y = 0;
        }
        else
        {
            movement.x = 0;
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if(movement.x < 0)
        {
            setDirection("West");
        }
        else if(movement.x > 0)
        {
            setDirection("East");
        }
        if(movement.y > 0)
        {
            setDirection("North");
        }
        else if(movement.y < 0)
        {
            setDirection("South");
        }


    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void setDirection(string direction)
    {
        animator.SetBool("East", false);
        animator.SetBool("West", false);
        animator.SetBool("North", false);
        animator.SetBool("South", false);

        animator.SetBool(direction, true);
    }


}
   

