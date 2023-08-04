using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] List<Transform> spawnLocations;
    [SerializeField] GameObject bulletPrefab;
    GameObject bullet;
    [SerializeField] PlayerMovement pm;
    void Update()
    {
        Shoot();
    }
    private void Shoot()
    {
        if(pm.canShoot)
        {
            
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                bullet = Instantiate(bulletPrefab, spawnLocations[0]);
                bullet.transform.parent = null;
                pm.canShoot = false;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                bullet = Instantiate(bulletPrefab, spawnLocations[1]);
                bullet.transform.parent = null;
                pm.canShoot = false;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                bullet = Instantiate(bulletPrefab, spawnLocations[2]);
                bullet.transform.parent = null;
                pm.canShoot = false;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                bullet = Instantiate(bulletPrefab, spawnLocations[3]);
                bullet.transform.parent = null;
                pm.canShoot = false;

                
            }
        }
    }

    IEnumerator FireContinuously(float cd, int loc)
    {

        yield return new WaitForSeconds(cd);
        StartCoroutine(FireContinuously(cd, loc));
    }
}
