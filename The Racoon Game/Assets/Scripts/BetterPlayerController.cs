using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterPlayerController : MonoBehaviour
{
    public Rigidbody2D rb;

    private float moveX;
    private Vector2 moveDirection;

    public bool isGrounded;


    void Start()
    {

    }


    void Update()
    {
        GetInputs();
    }

    void GetInputs()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        moveDirection = new Vector2(moveX, rb.velocity.y);
    }
}
