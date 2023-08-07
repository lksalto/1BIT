using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] float bulletSpeed = 10;
    [SerializeField] Transform explosionTransf;
    [SerializeField] int dmg;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //rb.AddRelativeForce(new Vector2(bulletSpeed, 0), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject exp = Instantiate(explosionPrefab, explosionTransf);
        exp.transform.parent = null;
        if(collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerLife>().AddLife(-dmg);
        }
        Destroy(gameObject);
    }
}
