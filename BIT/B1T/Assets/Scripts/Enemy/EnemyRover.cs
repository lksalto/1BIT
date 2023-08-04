using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRover : MonoBehaviour
{

    [SerializeField] float atkCd = 3;
    [SerializeField] float atkCount = 0;

    public float detectionRadius = 15f; // the radius in which the enemy can detect the player
    public LayerMask playerLayer; // the layer on which the player object is placed
    public float moveSpeed = 2f; // the speed at which the enemy moves towards the player
    private bool playerDetected = false; // flag to track if player is detected
    private Transform playerTransform; // reference to the player's transform component

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform spawnPoint;
    GameObject bullet;



    void Start()
    {
    }

    void Update()
    {
        atkCount -= Time.deltaTime;
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y-5), detectionRadius, playerLayer); // get all colliders within detection radius and on player layer

        foreach (Collider2D detectedObject in detectedObjects)
        {
            if (detectedObject.CompareTag("Player")) // if detected object is a player
            {
                playerTransform = detectedObject.transform;
                playerDetected = true; // set player detected flag to true
                GetComponent<SpriteRenderer>().color = Color.red; // set color of enemy object to red
            }
        }

        if (!playerDetected) // if player is not detected
        {
            GetComponent<SpriteRenderer>().color = Color.white; // set color of enemy object to white
        }
        else // if player is detected
        {
            // move towards the player in the X-axis
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(playerTransform.position.x, transform.position.y), moveSpeed * Time.deltaTime);
            if(atkCount <= 0)
            {
                bullet = Instantiate(bulletPrefab, spawnPoint);
                //bullet.transform.LookAt(playerTransform);
                atkCount = atkCd;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow; // set color of gizmo to yellow
        Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y-5), detectionRadius); // draw wire sphere gizmo to show detection radius
    }
}
