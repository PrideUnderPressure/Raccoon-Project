using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [SerializeField]
    Transform castPoint;

    [SerializeField]
    Transform castPointMidSide;

    [SerializeField]
    Transform castPointTopSide;

    [SerializeField]
    Transform castPoint2;

    [SerializeField]
    Transform castPoint3;

    //RB
    private Rigidbody2D _rb;
    //fallSpeed = _rb.velocity.y
    public float fallSpeed;

    //ANIMATOR
    public Animator anim;

    //MOVEMENT BASIC
    public float moveSpeed;
    public float jumpForce;
    //moveX = horizontal axis (-1 or 0 or 1)
    public float moveX;
    //Is it grounded?
    public bool _isGrounded = false;
    //Triggers if it's jumping. Used only for fliping horizontaly while jumping.
    public bool isJumping = false;
    //Allows for jumping animation to start. Complicated.
    public bool canJumpAni = false;

    public bool touchingWallLeft = false;
    public bool touchingWallRight = false;

    //WALL JUMPS
    public float wallJumpForceHorizontal;
    public float wallJumpForce;
    //Is it walled?
    public bool walledLeft = false;
    public bool walledRight = false;

    //FASTFALL
    //allows the fastfall keycheck
    public bool canDown = false;
    //jumpForce but for fastfall
    public float downSpeed;

    //PERFORMERS
    //performs the JUMP
    public bool jumped = false;
    //performs the FASTFALL
    public bool down = false;

    //VECTOR
    private Vector2 moveDirection;

    public bool canDoubleJump = false;

    //RAYCASTING MADNESS 
    public float laserLength = 10f;
    public bool rayWalledLeft = false;
    public bool rayWalledRight = false;

    //LEAVING WALLS BY HOLDING BUTTONS
    public const float wallHoldTime = 0.4f;
    public float wallHeldForL = 0f;
    public float wallHeldForR = 0f;

    public bool rotatedLeft = true;
    public bool rotatedRight = false;

    //AUDIO SOURCE
    public AudioSource runSfx;
    public AudioSource jumpSfx;
    public AudioSource bgm;
    private bool runningAudio = false;

    public bool cantWallJump = false;

    //ONLY GETS RIGIDBODY AND PLAYS THE BGM
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        bgm.Play();
    }

    void Update()
    {

        //Get's RB's velocity.Y
        fallSpeed = _rb.velocity.y;

        //AUDIO
        if (moveX != 0 && _isGrounded && runningAudio != true)
        {
            runSfx.Play();
            runningAudio = true;
        }
        if (moveX == 0 && _isGrounded == true)
        {
            runSfx.Pause();
            runningAudio = false;
        }

        //Processes Inputs (as long as the raccoon is NOT touching any WALLS (thus "rayWalled"))
        if (rayWalledLeft == false && rayWalledRight == false)
        {
            ProcessInputs();
        }

        Flips();

        //RAYCASTS MADNESS
        Vector2 castPosTop = castPointTopSide.position;
        Vector2 castPosMid = castPointMidSide.position;
        Vector2 castPos = castPoint.position;
        //Shortcut to the Ground layer (which is also walls)
        int layerMask = LayerMask.GetMask("Ground");

        //LEFT//
        if (rotatedLeft)
        {
            rayWalledLeft = false;
            RaycastHit2D hit = Physics2D.Raycast(castPos, Vector2.left, laserLength, layerMask, 0);
            RaycastHit2D hitMidSide = Physics2D.Raycast(castPosMid, Vector2.left, laserLength, layerMask, 0);
            RaycastHit2D hitTopSide = Physics2D.Raycast(castPosTop, Vector2.left, laserLength, layerMask, 0);

            if (hit.collider != null && hitTopSide.collider != null)
            {
                moveX = 0;
                rayWalledRight = true;
                cantWallJump = false;
                isJumping = false;
                canJumpAni = false;
                anim.SetBool("isWallClimbing", true);
            }
            else if (hit.collider != null && hitTopSide.collider == null && hitMidSide.collider == null && Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Space))
            {
                _rb.velocity = new Vector2(moveDirection.x * 0, moveDirection.y * 0);
                transform.position = new Vector2(transform.position.x - 1, transform.position.y + 1);
            }
            else if (hit.collider == null && hitTopSide.collider != null)
            {
                cantWallJump = true;
                rayWalledRight = true;
                anim.SetBool("isRunning", false);
            }
            else if (hit.collider != null)
            {
                rayWalledRight = true;
                anim.SetBool("isRunning", false);
                anim.SetBool("isWallClimbing", true);
            }
            else
            {
                rayWalledRight = false;
                anim.SetBool("isWallClimbing", false);
            }
        }

        //RIGHT//
        if (rotatedRight)
        {
            rayWalledRight = false;
            RaycastHit2D hit = Physics2D.Raycast(castPos, Vector2.right, laserLength, layerMask, 0);
            RaycastHit2D hitMidSide = Physics2D.Raycast(castPosMid, Vector2.right, laserLength, layerMask, 0);
            RaycastHit2D hitTopSide = Physics2D.Raycast(castPosTop, Vector2.right, laserLength, layerMask, 0);

            if (hit.collider != null && hitTopSide.collider != null)
            {
                moveX = 0;
                rayWalledLeft = true;
                cantWallJump = false;
                isJumping = false;
                canJumpAni = false;
                anim.SetBool("isWallClimbing", true);
            }
            else if (hit.collider != null && hitTopSide.collider == null && hitMidSide.collider == null && Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Space))
            {
                anim.SetBool("isWallClimbing", false);
                _rb.velocity = new Vector2(moveDirection.x * 0, moveDirection.y * 0);
                transform.position = new Vector2(transform.position.x + 1, transform.position.y + 1);
            }
            else if (hit.collider == null && hitTopSide.collider != null)
            {
                cantWallJump = true;
                rayWalledLeft = true;
                anim.SetBool("isRunning", false);
            }
            else if (hit.collider != null)
            {
                rayWalledLeft = true;
                anim.SetBool("isRunning", false);
                anim.SetBool("isWallClimbing", true);
            }
            else
            {
                Debug.DrawRay(castPos, Vector2.right * laserLength, Color.red);
                rayWalledLeft = false;
                anim.SetBool("isWallClimbing", false);
            }
        }

        //BOTTOM RAYS (WORKING FINE SO WHY TOUCH?) (detects ground)
        Vector2 castPos2 = castPoint2.position;
        Vector2 castPos3 = castPoint3.position;
        RaycastHit2D hit2 = Physics2D.Raycast(castPos2, Vector2.down, laserLength, layerMask, 0);
        RaycastHit2D hit3 = Physics2D.Raycast(castPos3, Vector2.down, laserLength, layerMask, 0);

        if (hit2.collider != null || hit3.collider != null)
        {
            _isGrounded = true;
            isJumping = false;
            canJumpAni = false;
            canDown = true;

            anim.SetBool("isJumping", false);
            anim.SetBool("isFastFalling", false);
            anim.SetBool("isWallClimbing", false);
        }
        else
        {
            _isGrounded = false;
            canJumpAni = true;
        }

        //When Touching Walls NOT GROUNDED (so wall climbing)
        //LEFT
        if (rayWalledLeft && Input.GetKey(KeyCode.A) && _isGrounded == false)
        {
            wallHeldForL += Time.deltaTime;
            //WALL JUMP IF A + SPACE
            if (Input.GetKeyDown(KeyCode.Space))
            {
                WallJumpLeft();
            }
            //LEAVE WALL IF A HELD
            if (wallHoldTime <= wallHeldForL)
            {
                _rb.AddForce(Vector2.left * 1, ForceMode2D.Impulse);
                rotatedRight = false;
                rotatedLeft = true;
            }
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyDown(KeyCode.Space))
        {
            wallHeldForL = 0f;
        }
        if (rayWalledLeft && Input.GetKey(KeyCode.Space) && _isGrounded == false)
        {
            if (Input.GetKeyDown(KeyCode.A))
                WallJumpLeft();
        }
        //IF TOUCHING WHEN GROUNDED
        if (rayWalledLeft && Input.GetKey(KeyCode.A) && _isGrounded == true)
        {
            _rb.AddForce(Vector2.left * 1, ForceMode2D.Impulse);
            rotatedRight = false;
            rotatedLeft = true;
        }

        //RIGHT
        if (rayWalledRight && Input.GetKey(KeyCode.D) && _isGrounded == false)
        {
            wallHeldForR += Time.deltaTime;
            //WALL JUMP IF D + SPACE
            if (Input.GetKeyDown(KeyCode.Space))
            {
                WallJumpRight();
            }
            //LEAVE WALL IF D HELD
            if (wallHoldTime <= wallHeldForR)
            {
                _rb.AddForce(Vector2.right * 1, ForceMode2D.Impulse);
                rotatedRight = true;
                rotatedLeft = false;
            }
        }
        else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyDown(KeyCode.Space))
        {
            wallHeldForR = 0f;
        }
        if (rayWalledRight && Input.GetKey(KeyCode.Space) && _isGrounded == false)
        {
            if (Input.GetKeyDown(KeyCode.D))
                WallJumpRight();
        }
        //IF TOUCHING WHILE GROUNDED
        if (rayWalledRight && Input.GetKey(KeyCode.D) && _isGrounded == true)
        {
            _rb.AddForce(Vector2.right * 1, ForceMode2D.Impulse);
            rotatedRight = true;
            rotatedLeft = false;
        }

        //JUMP key check
        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            jumped = true;
            canDoubleJump = true;
            anim.SetBool("isRunning", false);
        }

        //DOUBLE-JUMP key check
        if (_isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            canDoubleJump = true;
            anim.SetBool("isRunning", false);
        }

        //FASTFALL key check
        if (fallSpeed != 0 && canDown && Input.GetKeyDown(KeyCode.S))
        {
            down = true;

        }
    }

    void FixedUpdate()
    {
        Move();

        //JUMP ANIMATION CONTROLLER
        if (canJumpAni)
        {
            if (fallSpeed > -20 && fallSpeed != 0 && _isGrounded == false)
            {
                isJumping = true;
                anim.SetBool("isJumping", true);
                anim.SetBool("isFastFalling", false);
            }
            else if (fallSpeed < -12 && _isGrounded == false)
            {
                isJumping = true;
                anim.SetBool("isJumping", false);
                anim.SetBool("isFastFalling", true);
            }
        }

        //Turns off the running animation when not moving
        if (_isGrounded && moveX == 0)
        {
            anim.SetBool("isRunning", false);
        }

        //Makes sure isRunning anim bool is always off when not grounded
        if (_isGrounded == false)
        {
            anim.SetBool("isRunning", false);
            runSfx.Stop();
            runningAudio = false;
        }

        //JUMP
        if (jumped)
        {
            jumpSfx.PlayOneShot(jumpSfx.clip, 0.5f);
            //This resets the running animation. Probably useless, but better be sure...
            anim.SetBool("isRunning", false);

            //This sets jumped to false so that script can only happen once after jumping
            jumped = false;
            _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        //FASTFALL
        if (down)
        {
            down = false;
            canDown = false;

            _rb.AddForce(Vector2.down * downSpeed, ForceMode2D.Impulse);
        }
    }

    void ProcessInputs()
    {
        //this declars a local variable which gets the input of the player
        moveX = Input.GetAxisRaw("Horizontal");

        //this creates a vector2 for us to use from our two float values
        moveDirection = new Vector2(moveX, fallSpeed);
    }

    //this applies the horizontal movement to our player (if not touching wall)
    void Move()
    {
        if (rayWalledLeft == false && rayWalledRight == false)
        {
            _rb.velocity = new Vector2(moveDirection.x * moveSpeed, fallSpeed);
        }
    }

    void Flips()
    {
        //Horizontal Sprite Flip Right JUMPING
        if (isJumping && moveX > 0)
        {
            rotatedRight = true;
            gameObject.transform.localScale = new Vector2(1, 1);
            if (rotatedLeft)
            {
                transform.position = new Vector2(transform.position.x - 0.8f, transform.position.y);
            }
            rotatedLeft = false;
        }
        //Horizontal Sprite Flip Left JUMPING
        if (isJumping && moveX < 0)
        {
            rotatedLeft = true;
            gameObject.transform.localScale = new Vector2(-1, 1);
            if (rotatedRight)
            {
                transform.position = new Vector2(transform.position.x + 0.8f, transform.position.y);
            }
            rotatedRight = false;
        }

        //RUN ANIMATION CONTROLLERS (right and left)
        if (_isGrounded && moveX > 0)
        {
            fallSpeed = 0;
            rotatedRight = true;
            anim.SetBool("isRunning", true);
            gameObject.transform.localScale = new Vector2(1, 1);
            if (rotatedLeft)
            {
                transform.position = new Vector2(transform.position.x - 0.8f, transform.position.y);
            }
            rotatedLeft = false;
        }
        if (_isGrounded && moveX < 0)
        {
            fallSpeed = 0;
            rotatedLeft = true;
            anim.SetBool("isRunning", true);
            gameObject.transform.localScale = new Vector2(-1, 1);
            if (rotatedRight)
            {
                transform.position = new Vector2(transform.position.x + 0.8f, transform.position.y);
            }
            rotatedRight = false;
        }
    }

    // COLLISIONS ENTER
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }

    //WALL JUMPS (triggered by jumping on a corresponding wall direction) (BEST CODE HERE)
    //left
    private void WallJumpLeft()
    {
        if (cantWallJump == false)
        {
            _rb.velocity = new Vector2(moveDirection.x * 0, moveDirection.y * 0);
            _rb.AddForce(Vector2.up * wallJumpForce, ForceMode2D.Impulse);
            _rb.AddForce(Vector2.left * wallJumpForceHorizontal, ForceMode2D.Impulse);
            canDown = true;
            jumpSfx.PlayOneShot(jumpSfx.clip, 0.5f);
        }
    }
    //right
    private void WallJumpRight()
    {
        if (cantWallJump == false)
        {
            _rb.velocity = new Vector2(moveDirection.x * 0, moveDirection.y * 0);
            _rb.AddForce(Vector2.up * wallJumpForce, ForceMode2D.Impulse);
            _rb.AddForce(Vector2.right * wallJumpForceHorizontal, ForceMode2D.Impulse);
            canDown = true;
            jumpSfx.PlayOneShot(jumpSfx.clip, 0.5f);
        }
    }
}





