using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitbox : MonoBehaviour
{
    [SerializeField] PlayerStats playerStats;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Enemy _enemy = col.gameObject.GetComponent<Enemy>();
            bool isDead = _enemy.TakeDamage(playerStats.equippedWeapon.Base.Damage, playerStats.transform.position);

            // Kill enemy
            if (isDead)
            {
                _enemy.Death();
            }

        }
    }
}
