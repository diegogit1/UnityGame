using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAttack : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float attackInterval = 1.5f;

    void Start() => InvokeRepeating(nameof(Attack), 0f, attackInterval);

    void Attack()
    {
        Instantiate(bulletPrefab, transform.position, Quaternion.identity);
    }
}

