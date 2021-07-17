using UnityEngine;

public class ValuesSetter : MonoBehaviour
{
    public Vector2 laddersCheckBoxSize;

    public float moveSpeed;
    public float actualMoveSpeed;
    public float jumpSpeed;
    public float maxJumpTime;
    public float gravityScale;
    public float legsCheckRadius;
    public float laddersCheckRadius;

    public bool isGrounded;
    public bool isRunning;
    public bool isJumping;
    public bool isFalling;

    public bool canClimp;
    public bool canClimpUp;
    public bool canClimpDown;
    public bool isClimping;
    public bool isClimpingUp;
    public bool isClimpingDown;
    public bool isOnLadder;
    public bool topReached;
    public bool bottomReached;

    public Attacks attackState;
    public Movements movementState;
    public ClimpingStates climpingState;

    private void Awake()
    {
        GameManager.actualMoveSpeed = moveSpeed;
    }

    private void Update()
    {
        GameManager.laddersCheckBoxSize = laddersCheckBoxSize;

        GameManager.moveSpeed = moveSpeed;
        GameManager.jumpSpeed = jumpSpeed;
        GameManager.maxJumpTime = maxJumpTime;
        GameManager.gravityScale = gravityScale;
        GameManager.legsCheckRadius = legsCheckRadius;
        GameManager.laddersCheckRadius = laddersCheckRadius;

        actualMoveSpeed = GameManager.actualMoveSpeed;

        canClimp = GameManager.canClimp;
        canClimpUp = GameManager.canClimpUp;
        canClimpDown = GameManager.canClimpDown;
        isClimping = GameManager.isClimping;
        isClimpingUp = GameManager.isClimpingUp;
        isClimpingDown = GameManager.isClimpingDown;
        isOnLadder = GameManager.isOnLadder;

        isGrounded = GameManager.isGrounded;
        isRunning = GameManager.isRunning;
        isJumping = GameManager.isJumping;
        isFalling = GameManager.isFalling;

        attackState = GameManager.attackState;
        movementState = GameManager.movementState;
        climpingState = GameManager.climpingState;
    }
}