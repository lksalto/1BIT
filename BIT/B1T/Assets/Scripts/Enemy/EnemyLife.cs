using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLife : MonoBehaviour
{
    [SerializeField] int life;
    [SerializeField] SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void TakeDamage(int dmg)
    {
        life -= dmg;
        StartCoroutine(HitShine());
        if(life <= 0)
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

    public int GetLife()
    {
        return life;
    }
}
