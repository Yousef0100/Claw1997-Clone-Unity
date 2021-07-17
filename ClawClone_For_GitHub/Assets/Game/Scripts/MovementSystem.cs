using UnityEngine;

public enum Movements { NotMoving, Moving, Jumping, Falling, LookingDown, LookingUp };

public enum Attacks { None, SwordAttack, PunchAttack };

public class MovementSystem : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public Transform legsCheck;
    public LayerMask groundLayer;
    private Vector2 inputs;

    private float velocity;
    private float actualJumpTime;

    private bool startedJumping;
    private bool right;
    private bool left;

    private void Update()
    {
        GetInput();

        GameManager.isGrounded = IsGroundedCheck();
        ApplyingGravityForce();

        MovementSys();
        NotMovingBehaviour();
        HorizontalMovementBehaviour();

        LookingDown();
        JumpingSystem();
        JumpingBehaviour();
        FallingBehaviour();
    }

    private void GetInput()
    {
        inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        right = Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.D);
        left = Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.A);
    }

    private void ApplyingGravityForce()
    {
        if (!GameManager.isGrounded && !GameManager.isOnLadder)
        {
            velocity += (GameManager.gravityScale + 2f) * Time.deltaTime;
            transform.position += new Vector3(0f, -1f, 0f) * velocity * Time.deltaTime;
        }
        else velocity = 0f;
    }

    private bool IsGroundedCheck()
    {
        bool isGrounded = false;
        isGrounded = Physics2D.OverlapCircle(legsCheck.position, GameManager.legsCheckRadius, groundLayer);
        return isGrounded;
    }

    private void MovementSys()
    {
        if (GameManager.isGrounded && (GameManager.movementState != Movements.LookingDown && GameManager.movementState != Movements.LookingUp))
        {
            if (!right && !left) // Idle Animation Should Be Played
            {
                GameManager.movementState = Movements.NotMoving;
                GameManager.isRunning = false;
                GameManager.actualMoveSpeed = 0f;
            }
            else if (right && left) // Stay In Place
            {
                GameManager.movementState = Movements.NotMoving;
                GameManager.isRunning = false;
                GameManager.actualMoveSpeed = 0f;
            }
            else if (right && !GameManager.isOnLadder && !Input.GetKey(KeyCode.S)) // Moving Right
            {
                GameManager.movementState = Movements.Moving;
                transform.localScale = new Vector2(1f, 1f);
                GameManager.isRunning = true;
            }
            else if (left && !GameManager.isOnLadder && !Input.GetKey(KeyCode.S)) //Moving Left
            {
                GameManager.movementState = Movements.Moving;
                transform.localScale = new Vector2(-1f, 1f);
                GameManager.isRunning = true;
            }
        }
    }

    private void JumpingSystem()
    {
        if (GameManager.movementState != Movements.LookingDown && GameManager.movementState != Movements.LookingUp)
        {
            if (GameManager.isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    actualJumpTime = GameManager.maxJumpTime;
                    startedJumping = true;
                }
            }
            if (Input.GetKey(KeyCode.Space) && actualJumpTime >= 0f && startedJumping)
            {
                if (actualJumpTime >= 0f && !GameManager.isOnLadder)
                {
                    GameManager.isJumping = true;
                    GameManager.movementState = Movements.Jumping;
                    //Move The Man
                }
                else
                {
                    GameManager.isJumping = false;

                    if (GameManager.isOnLadder == false)
                    {
                        GameManager.isFalling = true;
                        GameManager.movementState = Movements.Falling;
                    }
                    else
                    {
                        GameManager.isFalling = false;
                        GameManager.movementState = Movements.NotMoving;
                    }
                }
                actualJumpTime -= Time.deltaTime;
            }
            else
            {
                GameManager.isJumping = false;

                if (GameManager.isGrounded)
                    GameManager.isFalling = false;
                else
                {
                    if (GameManager.isOnLadder)
                    {
                        GameManager.isFalling = false;
                        GameManager.movementState = Movements.NotMoving;
                    }
                    else
                    {
                        GameManager.isFalling = true;
                        GameManager.movementState = Movements.Falling;
                    }
                }
            }

            if (Input.GetKeyUp(KeyCode.Space)) // To Prevent Mid-Air Jumping
            {
                startedJumping = false;
            }
        }
    }

    private void JumpingBehaviour()
    {
        if (GameManager.movementState == Movements.Jumping)
        {
            transform.position += new Vector3(0f, 1f, 0f) * GameManager.jumpSpeed * actualJumpTime * Time.deltaTime;
            anim.SetBool("IsJumping", true);

            // Jumping And Moving In Air At The Same Time
            if ((right || left) && !GameManager.isOnLadder)
            {
                anim.SetBool("IsRunning", false);
                transform.localScale = (right) ? new Vector2(1f, 1f) : new Vector2(-1f, 1f);
                transform.position += new Vector3(inputs.x, 0f, 0f) * GameManager.actualMoveSpeed * Time.deltaTime;
            }
        }
        else anim.SetBool("IsJumping", false);
    }

    private void FallingBehaviour()
    {
        if (GameManager.movementState == Movements.Falling)
        {
            anim.SetBool("IsFalling", true);
            anim.SetBool("IsJumping", false);

            if ((right || left) && !GameManager.isOnLadder)
            {
                anim.SetBool("IsRunning", false);
                transform.localScale = (right) ? new Vector2(1f, 1f) : new Vector2(-1f, 1f);
                transform.position += new Vector3(inputs.x, 0f, 0f) * GameManager.actualMoveSpeed * Time.deltaTime;
            }
        }
        else anim.SetBool("IsFalling", false);
    }

    private void NotMovingBehaviour()
    {
        if (GameManager.movementState == Movements.NotMoving)
        {
            GameManager.isRunning = false;
            anim.SetBool("IsRunning", false);
        }
        else GameManager.actualMoveSpeed = GameManager.moveSpeed;
    }

    private void HorizontalMovementBehaviour()
    {
        if (GameManager.movementState == Movements.Moving) // Moving Right And Left
        {
            transform.position += new Vector3(inputs.x, 0f, 0f) * GameManager.actualMoveSpeed * Time.deltaTime;
            if (!GameManager.isJumping)
                anim.SetBool("IsRunning", true);
            else anim.SetBool("IsRunning", false);
        }
    }

    private void LookingDown()
    {
        float time = 0f;

        if (GameManager.isGrounded && !GameManager.canClimpDown)
        {
            if (Input.GetKey(KeyCode.S))
            {
                time += Time.deltaTime;
                anim.SetBool("IsLookingDown", true);
                anim.SetBool("IsRunning", false);
                GameManager.isRunning = false;
                GameManager.movementState = Movements.LookingDown;

                transform.localScale = (inputs.x != 0) ? new Vector3(inputs.x, 1f, 0f) : transform.localScale;
            }
            if ((Input.GetKeyUp(KeyCode.S) || !Input.GetKey(KeyCode.S)) && GameManager.movementState == Movements.LookingDown)
            {
                time = 0f;
                anim.SetBool("IsLookingDown", false);
                GameManager.movementState = Movements.NotMoving;
            }
        }
        else
        {
            GameManager.movementState = Movements.NotMoving;
            anim.SetBool("IsLookingDown", false);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(legsCheck.position, GameManager.legsCheckRadius);
    }
}