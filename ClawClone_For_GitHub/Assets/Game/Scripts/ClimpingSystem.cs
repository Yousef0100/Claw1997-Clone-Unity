using UnityEngine;

public enum ClimpingStates { NotClimpingAndOnLadder, NotClimpingAndNotOnLadder, Climping, LeavingLadder, StandingOnLadder, ClimpingDownTheLadder, JumpingFromTheLadder };

public class ClimpingSystem : MonoBehaviour
{
    private RaycastHit2D hit;
    private Transform ladderTop;
    public Transform ladderBottom;
    private Collider2D whatIsUnder;

    public Animator anim;

    public Transform legsCheck;
    public Transform topLadderChecker;
    public Transform bottomLadderChecker;
    public Transform ladderChecker;
    public Transform rightLadderCheck;
    public Transform leftLadderCheck;

    public LayerMask ladderLayer;
    public LayerMask ladderGroundLayer;

    public Collider2D ladderObject; // The Ladder That Iam Currently Overlaping Onto
    public Collider2D lastDetectedLadder;

    public float climpSpeed;
    public float bonusClimpSpeed;
    public float offsetToLadders;
    public float snapForce;

    public bool canClickDown = false;
    public bool canClickUp = false;

    private void Update()
    {
        ClimpingSys();
        ClimpingBehaviour();
        LeavingLadderBehaviour();
        LadderTopChecker();

        NotClimpingBehaviour();
        StandingOnLadderBehaviour();
        ClimpingDownLadderBehaviour();
        anim.SetBool("IsOnLadder", GameManager.isOnLadder);

        if (GameManager.isGrounded && GameManager.climpingState != ClimpingStates.ClimpingDownTheLadder) GameManager.isOnLadder = false; // To Set The Value Of The 'isOnLadder' Var To Be False (Very Important Line Of Code) Without it The Value Will Be Never Set To False Again Which Means He Will Never Move Again Instead He Will Get Stuck At The Ladder For Ever
        if (GameManager.climpingState == ClimpingStates.NotClimpingAndOnLadder) GameManager.isOnLadder = true;
        if (lastDetectedLadder != ladderObject && ladderObject != null && GameManager.isOnLadder != false)
            lastDetectedLadder = ladderObject;

        canClickUp = GameManager.canClickUp;
        canClickDown = GameManager.canClickDown;
    }

    private void ClimpingSys()
    {
        ladderObject = Physics2D.OverlapCircle(ladderChecker.position, GameManager.laddersCheckRadius, ladderLayer);

        if (Physics2D.OverlapBox(rightLadderCheck.position, GameManager.laddersCheckBoxSize, 0f, ladderLayer) && Physics2D.OverlapBox(leftLadderCheck.position, GameManager.laddersCheckBoxSize, 0f, ladderLayer))
        {
            GameManager.canClimp = true;
            if (!GameManager.isGrounded)
                GameManager.canClimpUp = true;
            else
                GameManager.canClimpUp = false;
            GameManager.canClimpDown = false;
        }
        else
        {
            GameManager.canClimp = false;
            GameManager.canClimpUp = false;
            GameManager.canClimpDown = false;
        }

        whatIsUnder = Physics2D.OverlapCircle(legsCheck.position, GameManager.legsCheckRadius, ladderGroundLayer);

        if (whatIsUnder != null && Physics2D.Raycast(rightLadderCheck.position, -transform.up, float.PositiveInfinity, ladderLayer) && Physics2D.Raycast(leftLadderCheck.position, -transform.up, float.PositiveInfinity, ladderLayer))
        {
            GameManager.canClimp = true;
            GameManager.canClimpUp = false;
            GameManager.canClimpDown = true;
        }

        if (GameManager.canClimp)
        {
            if (GameManager.isGrounded)
            {
                if (Input.GetKeyDown(KeyCode.Space) && Input.GetKey(KeyCode.W))
                {
                    GameManager.isOnLadder = true;
                    GameManager.isClimpingUp = true;
                    GameManager.isClimpingDown = false;

                    GameManager.climpingState = ClimpingStates.Climping;
                }

                if (Input.GetKey(KeyCode.S) && whatIsUnder != null)
                {
                    GameManager.isOnLadder = true;
                    GameManager.isClimpingUp = false;
                    GameManager.isClimpingDown = true;

                    GameManager.climpingState = ClimpingStates.ClimpingDownTheLadder;
                    return;
                }

                if (GameManager.climpingState == ClimpingStates.ClimpingDownTheLadder)
                {
                    if (Input.GetKey(KeyCode.W))
                    {
                        GameManager.climpingState = ClimpingStates.ClimpingDownTheLadder;
                    }
                    if (Input.GetKey(KeyCode.S))
                    {
                        GameManager.climpingState = ClimpingStates.ClimpingDownTheLadder;
                    }
                    if (!Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W))
                        GameManager.climpingState = ClimpingStates.ClimpingDownTheLadder;
                    GameManager.isOnLadder = true;
                    return;
                }
            }
            if (GameManager.isGrounded == false || GameManager.isOnLadder)
            {
                if (!GameManager.isGrounded) GameManager.canClimpUp = true;
                if (GameManager.isOnLadder) GameManager.canClimpUp = GameManager.canClimpDown = true;
                else GameManager.canClimpDown = false;

                if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S) && GameManager.canClimpUp && GameManager.canClickUp)
                {
                    GameManager.isOnLadder = true;
                    GameManager.isClimpingUp = true;
                    GameManager.isClimpingDown = false;
                    GameManager.climpingState = ClimpingStates.Climping;
                }
                else
                {
                    GameManager.isClimpingUp = false;
                    GameManager.climpingState = (GameManager.isOnLadder) ? ClimpingStates.NotClimpingAndOnLadder : ClimpingStates.NotClimpingAndNotOnLadder;
                }
                if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.W) && GameManager.canClickDown)
                {
                    if (GameManager.isGrounded)
                        GameManager.climpingState = ClimpingStates.LeavingLadder;

                    if (GameManager.canClimpDown)
                    {
                        GameManager.isOnLadder = true;
                        GameManager.isClimpingUp = false;
                        GameManager.isClimpingDown = true;
                        GameManager.climpingState = ClimpingStates.Climping;
                    }
                }
                else
                {
                    GameManager.isClimpingDown = false;
                }

                if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
                    GameManager.climpingState = (GameManager.isOnLadder) ? ClimpingStates.NotClimpingAndOnLadder : ClimpingStates.NotClimpingAndNotOnLadder;
            }
            else
            {
                if (GameManager.climpingState != ClimpingStates.ClimpingDownTheLadder)
                    GameManager.climpingState = ClimpingStates.NotClimpingAndNotOnLadder;
            }
        }
        else
        {
            GameManager.isOnLadder = false;
            GameManager.climpingState = ClimpingStates.NotClimpingAndNotOnLadder;
        }
    }

    private void ClimpingBehaviour()
    {
        if (GameManager.climpingState == ClimpingStates.Climping || GameManager.climpingState == ClimpingStates.NotClimpingAndOnLadder)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3((ladderObject != null) ? ladderObject.transform.position.x + offsetToLadders : transform.position.x, transform.position.y, transform.position.z), snapForce);

            if (Input.GetKey(KeyCode.W) && GameManager.canClimpUp && GameManager.canClickUp)
            {
                transform.position += Vector3.up * climpSpeed * Time.deltaTime;

                anim.speed = 1f;
                anim.SetBool("IsClimping", true);
                anim.SetFloat("Direction", 1.2f);
            }
            else if (Input.GetKey(KeyCode.S) && GameManager.canClimpDown && GameManager.canClickDown)
            {
                transform.position += Vector3.down * climpSpeed * Time.deltaTime;

                anim.speed = 1f;
                anim.SetBool("IsOnLadder", false);
                anim.SetBool("IsClimping", true);
                anim.SetFloat("Direction", -1.2f);
                anim.SetBool("TopReached", false);
            }
            else
            {
                anim.speed = 0f;
                anim.SetBool("IsClimping", false);
                anim.SetFloat("Direction", 0f);
            }
        }
    }

    private void LeavingLadderBehaviour()
    {
        if (GameManager.climpingState == ClimpingStates.LeavingLadder)
        {
            GameManager.isOnLadder = false;
            GameManager.isClimping = false;
            GameManager.isClimpingUp = false;
            GameManager.isClimpingDown = false;

            anim.SetBool("IsClimping", false);
            anim.SetTrigger("BottomReached");
            anim.speed = 1f;
        }
    }

    private void NotClimpingBehaviour()
    {
        if (GameManager.climpingState == ClimpingStates.NotClimpingAndNotOnLadder || GameManager.climpingState == ClimpingStates.NotClimpingAndOnLadder)
        {
            if (GameManager.climpingState == ClimpingStates.NotClimpingAndNotOnLadder)
            {
                GameManager.isOnLadder = false;
                GameManager.isClimping = false;
                GameManager.isClimpingUp = false;
                GameManager.isClimpingDown = false;

                anim.speed = 1f;
                anim.SetBool("IsClimping", false);
                anim.SetBool("IsOnLadder", false);
                anim.SetBool("TopReached", false);
                anim.SetBool("ClimpingDownLadder", false);
            }
            else if (GameManager.climpingState == ClimpingStates.NotClimpingAndOnLadder)
            {
                GameManager.isOnLadder = true;
                GameManager.isClimping = false;
                GameManager.isClimpingUp = false;
                GameManager.isClimpingDown = false;
                anim.SetBool("TopReached", false);
            }
        }
    }

    private void LadderTopChecker()
    {
        float distTop = -100f;
        if (GameManager.climpingState != ClimpingStates.NotClimpingAndNotOnLadder)
        {
            hit = Physics2D.Raycast(topLadderChecker.position, transform.up * Input.GetAxisRaw("Vertical"), float.PositiveInfinity, ladderGroundLayer);

            if (hit.collider != null)
            {
                ladderTop = hit.collider.transform.parent.Find("TopLadderCheck").transform;
            }

            distTop = topLadderChecker.position.y - ladderTop.position.y;

            if (Mathf.Abs(distTop) < 0.3f && Mathf.Abs(distTop) > 0f)
            {
                anim.SetBool("ClimpingDownLadder", false);
                anim.SetBool("TopReached", true);
                GameManager.climpingState = ClimpingStates.StandingOnLadder;
            }
        }
    }

    private void StandingOnLadderBehaviour()
    {
        if (GameManager.climpingState == ClimpingStates.StandingOnLadder)
        {
            if (Input.GetKey(KeyCode.W))
            {
                anim.speed = 1f;
                anim.SetBool("TopReached", true);
                anim.SetFloat("Direction", 1f);
                anim.SetBool("IsClimping", false);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                anim.speed = 1f;
                anim.SetFloat("Direction", -1f);
                anim.SetBool("IsClimping", true);
            }
            else
            {
                anim.speed = 0f;
            }
        }
    }

    private void ClimpingDownLadderBehaviour()
    {
        if (GameManager.climpingState == ClimpingStates.ClimpingDownTheLadder)
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += Vector3.up * climpSpeed * Time.deltaTime;

                anim.speed = 1f;
                anim.SetFloat("Direction", -1f);
                anim.SetBool("ClimpingDownLadder", true);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                transform.position += Vector3.down * climpSpeed * Time.deltaTime;

                anim.speed = 1f;
                anim.SetFloat("Direction", 1f);
                anim.SetBool("ClimpingDownLadder", true);
            }
            else
            {
                anim.speed = 0f;
                anim.SetFloat("Direction", 0f);
            }
        }
    }

    private void FinishClimpingAction() // Called By The Animation Events In The Editor At The End Of (Claw_PullUp.anim)
    {
        if (anim.GetBool("TopReached"))
        {
            anim.SetBool("TopReached", false);
            anim.SetBool("IsClimping", false);
            anim.SetBool("IsOnLadder", false);
        }
        if (ladderTop != null)
        {
            transform.position = new Vector3(transform.position.x, ladderObject.transform.position.y + 0.5f * ladderObject.transform.localScale.y + 0.5f * transform.localScale.y + 0.2f, 0f);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, lastDetectedLadder.transform.position.y + 0.5f * lastDetectedLadder.transform.localScale.y + 0.5f * transform.localScale.y + 0.2f, 0f);
        }
        GameManager.isGrounded = true;
        GameManager.isOnLadder = false;
        GameManager.climpingState = ClimpingStates.NotClimpingAndNotOnLadder;
    }

    private void StartClimpingDownAction() // Called By The Animation Events In The Editor At The Start Of (Claw_PullDown.anim)
    {
        if (GameManager.isGrounded)
        {
            gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
        }
    }
}