using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float atkCd = 3;
    [SerializeField] float atkCount = 0;
    [SerializeField] float bulletForce = 25;
    public float detectionRadius = 5f; // the radius in which the enemy can detect the player
    public float newDetectionRadius = 25f;
    public LayerMask playerLayer; // the layer on which the player object is placed
    public float moveSpeed = 1;
    public float life;
    public int enemyType;
    private bool playerDetected = false; // flag to track if player is detected
    private Transform playerTransform;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform spawnPoint;
    GameObject bullet;
    void Update()
    {
        atkCount -= Time.deltaTime;
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, detectionRadius, playerLayer); // get all colliders within detection radius and on player layer

        foreach (Collider2D detectedObject in detectedObjects)
        {
            if (detectedObject.CompareTag("Player")) // if detected object is a player
            {
                playerTransform = detectedObject.transform;
                playerDetected = true; // set player detected flag to true
                if (enemyType == 1)
                {
                    playerDetected = true; // set player detected flag to true
                                           
                    Vector2 targetPosition = new Vector2(detectedObject.transform.position.x, detectedObject.transform.position.y + detectedObject.transform.localScale.y / 1.5f);
                    Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
                    Vector2 newPosition = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
                    newPosition.y += (newPosition.y - currentPosition.y) * 1.5f;
                    transform.position = newPosition;
                    detectionRadius = newDetectionRadius;
                }
                else if(enemyType == 2)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerTransform.position.x, transform.position.y), moveSpeed * Time.deltaTime);
                    if (atkCount <= 0)
                    {
                        bullet = Instantiate(bulletPrefab, spawnPoint);
                        bullet.transform.parent = null;
                        bullet.GetComponent<Rigidbody2D>().AddForce((detectedObject.transform.position - spawnPoint.transform.position).normalized * bulletForce, ForceMode2D.Force);
                        //bullet.transform.LookAt(playerTransform);
                        atkCount = atkCd;
                    }
                }

            }
        }


    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; // set color of gizmo to yellow
        Gizmos.DrawWireSphere(transform.position, detectionRadius); // draw wire sphere gizmo to show detection radius
    }
    public void CalculateLife()
    {
        if(life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
