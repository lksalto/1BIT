using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    //Componentes
    public Rigidbody2D rb;
    Animator anim;
    [SerializeField] Player player;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] GameObject graphics;

    //Movimentação
    public bool canMove = true;
    public float fJumpForce = 5.0f;
    [SerializeField] float moveSpeed = 100f;
    public bool isGrounded;
    public bool bCanJump = true;
    public float horizontalInput;

    //Dodge
    [SerializeField] bool canDodge;
    [SerializeField] float dodgeCooldown = 1f;
    [SerializeField] float dodgeTimer;
    [SerializeField] float dodgeDuration = 0.2f;
    [SerializeField] float initialGravity;
    [SerializeField] GameObject dustPrefab;
    //Debugger
    [SerializeField] bool debugColor;
    [SerializeField] float comp;
    [SerializeField] float rayX;

    // Start is called before the first frame update
    void Start()
    {
        //sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        dodgeTimer = dodgeCooldown;
        initialGravity = rb.gravityScale;
    }

    // Update is called once per frame
    private void Update()
    {

        horizontalInput = Input.GetAxisRaw("Horizontal"); // get the input for horizontal movement
        Debugger();
        MovementHandler();
        JumpHandler();
        AnimationHandler();
        DodgeHandler();
    }

    private void MovementHandler()
    {

        // move the object horizontally
        if(canMove)
        {
            Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
            rb.velocity = movement;
        }


    }
    private void DodgeHandler()
    {
        dodgeTimer -= Time.deltaTime;
        if(horizontalInput != 0)
        {
            if (dodgeTimer < 0)
            {
                canDodge = true;
                sprite.color = Color.white;
            }
            else
            {
                sprite.color = Color.gray;
            }
            if (canDodge)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    StartCoroutine(Dodge());
                }
            }
        }
        
    }
    IEnumerator Dodge()
    {
        dodgeTimer = dodgeCooldown;
        if(horizontalInput != 0 )
        {
            GameObject dust = Instantiate(dustPrefab, transform.position, Quaternion.identity);
            canMove = false;
            rb.velocity = Vector3.zero;
            rb.gravityScale = 0;
            rb.AddForce(Vector3.right * horizontalInput * fJumpForce * 100f );
            yield return new WaitForSeconds(dodgeDuration);
            rb.gravityScale = initialGravity;
            rb.velocity = Vector3.zero;
            canMove = true;
        }
        else
        {
            yield return null;
        }
        

    }

    private void JumpHandler()
    {

        if (Input.GetKeyDown(KeyCode.W) && bCanJump)
        {

            Jump();
            bCanJump = false;
        }

    }

    void Jump()
    {

        rb.AddForce(new Vector2(0f, fJumpForce), ForceMode2D.Impulse);

    }

    private void CooldownHandler()
    {

    }

    void Debugger()
    {
        RaycastHit2D[] hits;
        RaycastHit2D[] hitsL;
        RaycastHit2D[] hitsR;
        hits = Physics2D.RaycastAll(transform.position, -transform.up, comp);
        hitsL = Physics2D.RaycastAll(new Vector2(transform.position.x - rayX, transform.position.y), -transform.up, comp);
        hitsR = Physics2D.RaycastAll(new Vector2(transform.position.x + rayX, transform.position.y), -transform.up, comp);
        isGrounded = false;
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y), -transform.up * comp);
        Debug.DrawRay(new Vector2(transform.position.x - rayX, transform.position.y), -transform.up * comp);
        Debug.DrawRay(new Vector2(transform.position.x + rayX, transform.position.y), -transform.up * comp);

        // For each object that the raycast hits.
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit2D hit = hits[i];
            if (hit.collider.CompareTag("Floor"))
            {
                
                if (transform.position.y > hit.collider.transform.position.y)
                    isGrounded = true;
                    
                    //Debug.Log("mid");
                    //bCanJump = true;
                    //rocketJump = true;
            }

        }
        for (int i = 0; i < hitsL.Length; i++)
        {
            RaycastHit2D hitL = hitsL[i];
            if (hitL.collider.CompareTag("Floor"))
            {
                
                if (transform.position.y > hitL.collider.transform.position.y + hitL.collider.transform.localScale.y/2)
                    isGrounded = true;
                    
                    //Debug.Log("left");
                //bCanJump = true;
                //rocketJump = true;

            }

        }
        for (int i = 0; i < hitsR.Length; i++)
        {
            RaycastHit2D hitR = hitsR[i];
            if (hitR.collider.CompareTag("Floor"))
            {
               
                if (transform.position.y > hitR.collider.transform.position.y + hitR.collider.transform.localScale.y/2)
                    isGrounded = true;
                    
                    //Debug.Log("right");
                //bCanJump = true;
                //rocketJump = true;
            }
        }
        if (debugColor)
        {
            if (!bCanJump)
            {
                sprite.color = Color.red;
            }
            else
            {
                sprite.color = Color.green;
            }
        }

        
        bCanJump = isGrounded;

    }
    
    void AnimationHandler()
    {
        anim.SetFloat("VerticalVelocity", rb.velocity.y);
        anim.SetBool("IsJumping", !bCanJump);
        anim.SetBool("IsRunning", Mathf.Abs(horizontalInput) > 0f);
        if (horizontalInput != 0)
        {
            sprite.flipX = horizontalInput < 0;
        }



    }
}
