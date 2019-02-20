using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float lifeTime = 5f;
    public GameObject hitEffect;

    float lifeLeft;
    Rigidbody2D rb;

    void Start()
    {
        lifeLeft = Time.time + lifeTime;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = transform.up * moveSpeed;
        if (lifeLeft < Time.time)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerCollider"))
        {
            FindObjectOfType<OnPlayerDeath>().KillPlayer(collision.transform.parent.transform.parent.gameObject);
        }
        DestroyProjectile();
    }

    void DestroyProjectile()
    {
        if (hitEffect != null)
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
