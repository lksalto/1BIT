using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSecs : MonoBehaviour
{
    [SerializeField] float time = 0.5f;    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
