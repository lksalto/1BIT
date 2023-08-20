using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public bool fireContinuously = false;
    [SerializeField] List<Transform> spawnLocations;
    [SerializeField] GameObject bulletPrefab;
    GameObject bullet;
    [SerializeField] PlayerMovement pm;
    [SerializeField] bool rocketJump = true;
    [SerializeField] int rocketCount;
    [SerializeField] int rocketMax;
    public float attackDuration = 0.5f;
    [SerializeField] float attackCooldown = 1f;
    //float lastAttackTime = -Mathf.Infinity;
    //Atirar
    public bool canShoot;
    public bool isShooting;
    //float atkCooldown = 1f;
    float lastAttack = 0;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        ManageCooldowns();
        Shoot();
    }
    private void Shoot()
    {
        if(fireContinuously)
        {
            if (canShoot && !isShooting)
            {

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    StartCoroutine(ShootContinuously(0, attackCooldown, false));
                }

                else if (Input.GetKey(KeyCode.UpArrow))
                {
                    StartCoroutine(ShootContinuously(1, attackCooldown, false));
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    StartCoroutine(ShootContinuously(2, attackCooldown, false));
                }
                else if (Input.GetKey(KeyCode.DownArrow))
                {
                    StartCoroutine(ShootContinuously(3, attackCooldown, true));
                    if (rocketJump && rocketCount < rocketMax)
                    {
                        rocketCount++;
                        rb.velocity = Vector2.zero;
                        rb.AddForce(new Vector2(0f, pm.fJumpForce), ForceMode2D.Impulse);
                    }



                }
            }
            if (isShooting)
            {
                if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.DownArrow))
                {
                    isShooting = false;
                    StopAllCoroutines();
                }

            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                bullet = Instantiate(bulletPrefab, spawnLocations[0]);
                bullet.transform.parent = null;
            }

            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                bullet = Instantiate(bulletPrefab, spawnLocations[1]);
                bullet.transform.parent = null;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                bullet = Instantiate(bulletPrefab, spawnLocations[2]);
                bullet.transform.parent = null;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                bullet = Instantiate(bulletPrefab, spawnLocations[3]);
                bullet.transform.parent = null;
                if (rocketJump && rocketCount < rocketMax)
                {
                    rocketCount++;
                    rb.velocity = Vector2.zero;
                    rb.AddForce(new Vector2(0f, pm.fJumpForce), ForceMode2D.Impulse);
                }



            }
        }
        
    }
    private void ManageCooldowns()
    {
        lastAttack -= Time.deltaTime;
        if (lastAttack <= 0)
        {
            canShoot = true;

        }
        if (pm.bCanJump)
        {
            rocketCount = 0;
        }
        rocketJump = !pm.isGrounded;
    }

    IEnumerator ShootContinuously(int pos, float cd, bool special)
    {
        lastAttack = attackCooldown;
        isShooting = true;
        bullet = Instantiate(bulletPrefab, spawnLocations[pos]);
        bullet.transform.parent = null;
        if(special && rocketCount < rocketMax)
        {
            bullet.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        canShoot = false;
        yield return new WaitForSeconds(cd);
        StartCoroutine(ShootContinuously(pos, cd, false));
    }
}
