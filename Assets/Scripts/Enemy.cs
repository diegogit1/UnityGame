using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f;
    Transform player;

    void Start() => player = GameObject.FindGameObjectWithTag("Player").transform;

    void Update()
    {
        if (player != null)
        {
            Vector2 dir = (player.position - transform.position).normalized;
            transform.position += (Vector3)(dir * speed * Time.deltaTime);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerHealth>().TakeDamage(1);
            Destroy(gameObject);
        }
    }
}

