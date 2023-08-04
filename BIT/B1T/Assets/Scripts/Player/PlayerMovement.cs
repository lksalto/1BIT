using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Componentes
    public Rigidbody2D rb;
    Animator anim;
    [SerializeField] Player player;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] GameObject graphics;

    //Movimentação
    public float fJumpForce = 5.0f;
    [SerializeField] float moveSpeed = 100f;
    public float attackDuration = 0.5f;
    [SerializeField] float attackCooldown = 1f;
    float lastAttackTime = -Mathf.Infinity;
    public bool bCanJump = true;
    public float horizontalInput;
    [SerializeField] bool rocketJump = true;
    [SerializeField] int rocketCount;
    [SerializeField] int rocketMax;

    //Atirar
    public bool canShoot;
    float atkCooldown = 1f;
    float lastAttack = 0;

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
    }

    // Update is called once per frame
    private void Update()
    {
        lastAttack -= Time.deltaTime;
        if(lastAttack <= 0)
        {
            canShoot = true;
        }
        horizontalInput = Input.GetAxisRaw("Horizontal"); // get the input for horizontal movement
        Debugger();
        MovementHandler();
        JumpHandler();
        AnimationHandler();
    }

    private void MovementHandler()
    {

        // move the object horizontally
        Vector2 movement = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.velocity = movement;

    }

    private void JumpHandler()
    {

        if (bCanJump)
        {



        }
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

    void Debugger()
    {
        RaycastHit2D[] hits;
        RaycastHit2D[] hitsL;
        RaycastHit2D[] hitsR;
        hits = Physics2D.RaycastAll(transform.position, -transform.up, comp);
        hitsL = Physics2D.RaycastAll(new Vector2(transform.position.x - rayX, transform.position.y), -transform.up, comp);
        hitsR = Physics2D.RaycastAll(new Vector2(transform.position.x + rayX, transform.position.y), -transform.up, comp);
        bool isGrounded = false;
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
        if(bCanJump)
        {
            rocketCount = 0;
        }
        rocketJump = !isGrounded;
    }
    
    void AnimationHandler()
    {
        anim.SetFloat("VerticalVelocity", rb.velocity.y);
        anim.SetBool("IsJumping", !bCanJump);
        anim.SetBool("IsRunning", Mathf.Abs(horizontalInput) > 0f);
        if (horizontalInput != 0)
        {
            graphics.transform.localScale = new Vector3(horizontalInput * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            graphics.transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        if(canShoot)
        {

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                
                lastAttack = attackCooldown;
                lastAttackTime = Time.time;
                StartCoroutine(Attack("AttackU", "StopAttackU"));
                
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                
                lastAttack = attackCooldown;
                if(rocketJump && rocketCount < rocketMax)
                {
                    rocketCount++;
                    rb.velocity = Vector2.zero;
                    rb.AddForce(new Vector2(0f, fJumpForce), ForceMode2D.Impulse);
                }
                
                lastAttackTime = Time.time;
                StartCoroutine(Attack("AttackD", "StopAttackD"));
                
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                
                lastAttack = attackCooldown;
                lastAttackTime = Time.time;
                if(horizontalInput >= 0)
                {
                    StartCoroutine(Attack("AttackL", "StopAttackL"));
                }
                else
                {
                    StartCoroutine(Attack("AttackR", "StopAttackR"));
                }
                

            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                
                lastAttack = attackCooldown;
                lastAttackTime = Time.time;
                if(horizontalInput >= 0)
                {
                    StartCoroutine(Attack("AttackR", "StopAttackR"));
                }
                else
                {
                    StartCoroutine(Attack("AttackL", "StopAttackL"));
                }
                

            }
        }

        IEnumerator Attack(string pos, string pos2)
        {
            anim.SetTrigger(pos);
            yield return new WaitForSeconds(attackDuration);
            anim.SetTrigger(pos2);
        }
    }
}
