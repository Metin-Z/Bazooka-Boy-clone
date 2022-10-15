using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombComponent : MonoBehaviour
{
    public GameObject ExplosionEffect;
    public float radius = 45.0F;
    public float power = 1000.0F;
    Rigidbody _rb;
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    void OnEnable()
    {
        StartCoroutine(Explosion());
        GameManager.instance.bombs.Add(gameObject);
    }

    public IEnumerator Explosion()
    {
        yield return new WaitForSeconds(.2f);

        while (_rb.velocity.magnitude > 1f)
        {
            yield return new WaitForFixedUpdate();
        }

        GameObject effect = Instantiate(ExplosionEffect, transform.position, Quaternion.identity);
        effect.transform.parent = null;

        Destroy(transform.gameObject);
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                if (hit.gameObject.CompareTag("Enemy"))
                {
                    EnemyDisplay _enemy = hit.GetComponent<EnemyDisplay>();
                    _enemy.Dead(power, explosionPos, radius, 3f);
                }
                if (hit.gameObject.CompareTag("Player"))
                {
                    PlayerController _player = hit.GetComponent<PlayerController>();
                    _player.Dead(power, explosionPos, radius, 3f);
                }
                Destroy(hit, 2.5f);
                GameManager.instance.bombs.Remove(gameObject);
            }
        }

        GameManager.instance.LevelCheck();
    }


    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
