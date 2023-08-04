using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D bc2d;
    public bool isDead;
    [SerializeField] Material[] materiais;
    SpriteRenderer sprite;

    //Transform t;
    private void Awake()
    {
        bc2d = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        isDead = false;
        sprite = GetComponent<SpriteRenderer>();
        //t = GetComponent<Transform>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {
            isDead = true;
            rb.velocity = new Vector2(0, 0);
            bc2d.enabled = false;
            rb.freezeRotation = false;
            Destroy(gameObject, 8f);
            rb.AddForce(new Vector2(1, 20), ForceMode2D.Impulse);
            transform.rotation = Quaternion.SlerpUnclamped(transform.rotation,  Quaternion.Euler(0, 0, 45), 1f);
            //sprite.material = materiais[1];
            StartCoroutine(waiter());
        }
    }
    IEnumerator waiter()
    {
        yield return new WaitForSeconds(0.1f);
        //sprite.material = materiais[0];
    }
}
