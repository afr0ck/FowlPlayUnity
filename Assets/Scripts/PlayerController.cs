using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Public variables
    public float moveSpeed;
    public float jumpHeight;
    public float gravityScale;
    public float turnSpeed;
    public float knockBackForce;
    public float knockBackTime;
    
    public CharacterController controller;
    public Animator anim;
    public Transform pivot;
    public GameObject playerModel;

    // Private variables
    private float knockBackCount;
    private bool hasJumped;
    private bool hasDoubleJump;
    private bool grounded;
    private bool jumpCheatOn;
    private float ogJumpHeight;

    private Vector3 moveDirection;
    private Vector3 playerVelocity;

    // Start is called before the first frame update
    void Start()
    {
        hasJumped = false;
        jumpCheatOn = false;
        ogJumpHeight = jumpHeight;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = controller.isGrounded;

        if (grounded)
        {
            hasDoubleJump = true;
            hasJumped = false;
        }

        // If currently in knockback player restricted from moving on their own
        if (knockBackCount <= 0)
        {
            if (playerVelocity.y < 0 && grounded)
            {
                playerVelocity.y = 0f;
            }

            moveDirection = ((transform.right * Input.GetAxisRaw("Horizontal")) + (transform.forward * Input.GetAxisRaw("Vertical")));
            moveDirection.y = 0;
            moveDirection = moveDirection.normalized * moveSpeed;

            if (moveDirection != Vector3.zero)
            {
                gameObject.transform.forward = moveDirection;
            }

            if (Input.GetButtonDown("Jump") & !hasJumped)
            {
                hasJumped = true;
                if (playerVelocity.y < 0)
                {
                    playerVelocity.y = 0;
                    playerVelocity.y += Mathf.Sqrt(jumpHeight * -4f * Physics.gravity.y);
                }
                else
                {
                    playerVelocity.y += Mathf.Sqrt(jumpHeight * -3f * Physics.gravity.y);
                }
            } else
            {
                if (Input.GetButtonDown("Jump") & !grounded & hasDoubleJump)
                {
                    hasDoubleJump = false;

                    if (playerVelocity.y < 0)
                    {
                        playerVelocity.y = 0;
                        playerVelocity.y += Mathf.Sqrt(jumpHeight * -4f * Physics.gravity.y);
                    }
                    else
                    {
                        playerVelocity.y += Mathf.Sqrt(jumpHeight * -3f * Physics.gravity.y);
                    }
                }
            }

            
        } else
        {
            knockBackCount -= Time.deltaTime;
        }

        // Moves the player and allows for knockback to move player as well even if they are locked out
        controller.Move(moveDirection * Time.deltaTime);
        playerVelocity.y += Physics.gravity.y * Time.deltaTime * gravityScale;
        controller.Move(playerVelocity * Time.deltaTime);

        // Rotates the player to move relative to camera view angle.
        // Character will turn the correct directions even if camera angle never changes
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, 0f);
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, turnSpeed * Time.deltaTime);
            }
        }

        // Set parameters for animations
        anim.SetBool("isGrounded", grounded);
        anim.SetFloat("speed", (Mathf.Abs(Input.GetAxis("Vertical")) + Mathf.Abs(Input.GetAxis("Horizontal"))));

        cheatReset();
        cheatJump();
    }

    // Function to actually add knockback to player when damage is taken
    public void KnockBack(Vector3 direction)
    {
        knockBackCount = knockBackTime;
        moveDirection = direction * knockBackForce;
        playerVelocity.y += Mathf.Sqrt(knockBackForce * -3f * Physics.gravity.y);
    }

    private void cheatReset()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.R))
        {
            FindObjectOfType<HealthManager>().respawn();
        }
    }

    private void cheatJump()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.E))
        {
            if (jumpCheatOn)
            {
                jumpCheatOn = false;
                jumpHeight = ogJumpHeight;
            } else
            {
                jumpCheatOn = true;
                jumpHeight = 100;
            }
            
        }
    }
}
