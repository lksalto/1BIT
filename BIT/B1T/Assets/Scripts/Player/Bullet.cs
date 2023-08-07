using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    PlayerMovement pm;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float bulletSpeed = 10;
    [SerializeField] Transform explosionTransf;
    [SerializeField] int dmg = 1;
    EnemyLife enemyLife;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(new Vector2(bulletSpeed, 0), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            enemyLife = collision.GetComponent<EnemyLife>();
            enemyLife.TakeDamage(dmg);
        }
        GameObject exp = Instantiate(explosionPrefab, explosionTransf);
        exp.transform.parent = null;
        Destroy(gameObject);
    }
}
