using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDisplay : MonoBehaviour
{
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        GameManager.instance.enemies.Add(gameObject);
    }

    bool dead;
    public void Dead(float power, Vector3 explosionPos, float radius, float upForce)
    {
        if (dead)
            return;
        GameManager.instance.enemies.Remove(gameObject);
        _animator.enabled = false;

        foreach (Rigidbody item in GetComponentsInChildren<Rigidbody>())
        {
            item.isKinematic = false;
            item.velocity = Vector3.zero;
            item.angularVelocity = Vector3.zero;
        }

        foreach (Rigidbody item in GetComponentsInChildren<Rigidbody>())
            item.AddExplosionForce(power, explosionPos, radius, upForce);

        dead = true;
        GameManager.instance.enemies.Remove(gameObject);
        GameManager.instance.LevelCheck();
    }
}
