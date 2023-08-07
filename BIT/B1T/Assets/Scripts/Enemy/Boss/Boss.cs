using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : MonoBehaviour
{
    Transform playerTransform;
    float jumpTarget;
    bool playerDetected;
    [SerializeField] int phase;
    EnemyLife life;
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
    public bool isPounding;
    float calculatedDistance;
    Rigidbody2D rb;
    SpriteRenderer sr;
    [SerializeField] bool chargingAttack2;

    private void Start()
    {
        phase = 1;
        jumpCounter = jumpCooldown;
        rb = GetComponent<Rigidbody2D>();
        life = GetComponent<EnemyLife>();
        chargingAttack2 = false;
    }

    private void Update()
    {
        jumpCounter -= Time.deltaTime;
        GroundCheck();
        if(life.GetLife() < 350)
        {
            phase = 2;
        }
        if (phase == 1)
        {
            
            JumpCheck();
        }
        else if(phase == 2)
        {
            
            Jump2Check();
        }


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

                if (!isGrounded )
                {

                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(jumpTarget, transform.position.y), speed * calculatedDistance * Time.deltaTime);
                    if (transform.position.x == jumpTarget)
                    {
                        isGrounded = true;
                        GroundPound();
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

    private void Jump2Check()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y - 5), jumpDistance, playerLayer); // get all colliders within detection radius and on player layer

        foreach (Collider2D detectedObject in detectedObjects)
        {
            if (detectedObject.CompareTag("Player")) // if detected object is a player
            {

                playerTransform = detectedObject.transform;
                playerDetected = true; // set player detected flag to true
                                       //GetComponent<SpriteRenderer>().color = Color.red; // set color of enemy object to red

                if (!chargingAttack2)
                {
                    StartCoroutine(StartPhase2());

                }


            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor"))
        {
            GroundPound();
           
        }
        if (collision.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
    }
    private void GroundCheck()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, transform.localScale.y * 1.2f, groundLayer);

        if(hit.collider != null && ((1 << hit.collider.gameObject.layer) & groundLayer) != 0)
        {
            isGrounded = true;
            transform.GetChild(0).gameObject.SetActive(Mathf.Abs(rb.velocity.x) > 0);
        }
        else
        {
            isGrounded = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
    private void Jump()
    {
        rb.AddForce(Vector2.up * jumpHeigth, ForceMode2D.Impulse);
    }

    private void GroundPound()
    {
        jumpTarget = gameObject.transform.position.x;
        rb.velocity = new Vector3(0,0,0);
        
        rb.AddForce(Vector2.down * jumpHeigth, ForceMode2D.Impulse);
    }

    IEnumerator StartPhase2()
    {
        

        chargingAttack2 = true;
        Jump();
        yield return new WaitForSeconds(1.4f);
        Debug.Log("subir");
        
        StartCoroutine(ChargePhase2());
    }

    IEnumerator ChargePhase2()
    {

        Debug.Log("parar");
        rb.velocity = new Vector3(0,0,0);
        rb.gravityScale = 0;
        yield return new WaitForSeconds(1.4f);
        StartCoroutine(AttackPhase2());
    }

    IEnumerator AttackPhase2()
    {
        Debug.Log("atacar");
        rb.AddForce((playerTransform.position - transform.position).normalized * 15f, ForceMode2D.Impulse);
        
        
        yield return new WaitForSeconds(2f);
        rb.velocity = new Vector3(0, 0, 0);
        rb.gravityScale = 1;

        StartCoroutine(RechargePhase2());
    }

    IEnumerator RechargePhase2()
    {
        yield return new WaitForSeconds(1f);
        chargingAttack2 = false;
    }

}







