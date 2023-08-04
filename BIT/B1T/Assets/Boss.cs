using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : MonoBehaviour
{
    Transform playerTransform;
    float jumpTarget;
    bool playerDetected;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask playerLayer;
    public float speed = 1;
    public float jumpDistance = 0;
    public float jumpHeigth = 3.5f;
    public float jumpForce = 35;
    public float jumpCooldown;
    float jumpCounter;
    public float raycastSize;
    public bool isGrounded;
    float calculatedDistance;
    Rigidbody2D rb;
    

    private void Start()
    {
        jumpCounter = jumpCooldown;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        jumpCounter -= Time.deltaTime;
        GroundCheck();
        JumpCheck();

    }

    private void FixedUpdate()
    {

    }

    private void JumpCheck()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y - 5), jumpDistance, playerLayer); // get all colliders within detection radius and on player layer

        foreach (Collider2D detectedObject in detectedObjects)
        {
            if (detectedObject.CompareTag("Player")) // if detected object is a player
            {
                
                playerTransform = detectedObject.transform;
                playerDetected = true; // set player detected flag to true
                //GetComponent<SpriteRenderer>().color = Color.red; // set color of enemy object to red
                
                if (!isGrounded)
                {

                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(jumpTarget, transform.position.y), speed * calculatedDistance * Time.deltaTime);
                    if (transform.position.x == jumpTarget)
                    {
                        isGrounded = true;
                        rb.AddForce(Vector2.down * jumpHeigth/25, ForceMode2D.Impulse);
                    }

                        
                }
                else
                {
                    
                    if (jumpCounter <= 0)
                    {
                        jumpTarget = playerTransform.position.x;
                        calculatedDistance = Vector2.Distance(transform.position, detectedObject.transform.position) / 5;
                        jumpCounter = jumpCooldown;
                        rb.AddForce(Vector2.up * jumpHeigth, ForceMode2D.Impulse);
                    }
                }
            }
        }

    }

    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, transform.localScale.y * 1.2f, groundLayer);

        if(hit.collider != null && ((1 << hit.collider.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpHeigth, ForceMode2D.Impulse);
    }



}







