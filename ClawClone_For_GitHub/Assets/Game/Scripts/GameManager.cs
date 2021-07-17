using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static float moveSpeed;
    public static float actualMoveSpeed;
    public static float jumpSpeed;
    public static float maxJumpTime;
    public static float gravityScale;
    public static float legsCheckRadius;
    public static float laddersCheckRadius;

    public static float timeToMoveDownCamera;

    public static Vector2 laddersCheckBoxSize;

    public static bool canClimp;
    public static bool canClimpUp;
    public static bool canClimpDown;
    public static bool isClimping;
    public static bool isClimpingUp;
    public static bool isClimpingDown;
    public static bool isOnLadder;
    public static bool topReached;
    public static bool bottomReached;

    public static bool canClickUp;
    public static bool canClickDown;

    public static bool isGrounded;
    public static bool isRunning;
    public static bool isJumping;
    public static bool isFalling;

    public static Attacks attackState;
    public static Movements movementState;
    public static ClimpingStates climpingState;

    private void Update()
    {
        InputManager();
    }

    private void InputManager()
    {
        if (Input.GetKeyDown(KeyCode.W) || canClickUp)
            if (Input.GetKey(KeyCode.W))
                canClickUp = true;

        if (Input.GetKeyDown(KeyCode.S) || canClickDown)
            if (Input.GetKey(KeyCode.S))
                canClickDown = true;

        if (Input.GetKeyUp(KeyCode.W))
            canClickUp = false;
        if (Input.GetKeyUp(KeyCode.S))
            canClickDown = false;
    }
}