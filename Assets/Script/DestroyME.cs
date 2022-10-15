using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyME : MonoBehaviour
{
    public float DeathSecond;
    void Start()
    {
        Destroy(transform.gameObject,DeathSecond);
    }   
}
