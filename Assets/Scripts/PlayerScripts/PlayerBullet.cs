using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;

    int damage;
    float shotSpeed;
    Vector2 direction;

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * shotSpeed * Time.fixedDeltaTime);
    }

    public void SetStats(int d, float s, Vector2 dir)
    {
        damage = d;
        shotSpeed = s;
        direction = dir;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Enemy _enemy = col.gameObject.GetComponent<Enemy>();
            bool isDead = _enemy.TakeDamage(damage, transform.position);

            // Kill enemy
            if (isDead)
            {
                _enemy.Death();
            }

            Destroy(this.gameObject);
        }
    }
}
