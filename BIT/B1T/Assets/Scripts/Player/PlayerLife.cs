using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] int life;
    [SerializeField] int maxLife = 10;
    [SerializeField] SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        life = maxLife;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddLife(int qtt)
    {
        life += qtt;
        if (life > maxLife)
        {
            life = maxLife;
        }

    }
    public int GetLife()
    {
        return life;
    }
    public void TakeDamage(int dmg)
    {
        AddLife(-dmg);
        StartCoroutine(HitShine());
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator HitShine()
    {
        sr.color = Color.grey;
        yield return new WaitForSeconds(0.08f);
        sr.color = Color.white;
    }
}
