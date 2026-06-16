using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack_Area : MonoBehaviour
{
    public static int EnemyHealth = 100; // Health of the Enemy
    private int damage = 25; // Amount of damage to deal to the Enemy

    private void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyHealth health = collider.GetComponent<EnemyHealth>();
        health.Damage(damage);
    }
}
