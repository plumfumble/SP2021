using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] CircleCollider2D hitbox;
    [SerializeField] Transform target;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject weaponDropPrefab;

    [SerializeField] int maxHealth;
    [SerializeField] int health;

    [SerializeField] float speed;
    [SerializeField] float maxRange;
    [SerializeField] float minRange;
    [SerializeField] int damage = 1;

    private bool inDamageRange;

    private bool locked;
    private Vector2 knockbackVector;

    void Start()
    {
        health = maxHealth;
        target = FindObjectOfType<PlayerMovement>().transform;
    }

    void FixedUpdate()
    {
        if (!locked)
        {
            if (Vector3.Distance(target.position, transform.position) <= maxRange && Vector3.Distance(target.position, transform.position) >= minRange)
            {
                FollowPlayer();
            }
        }
        else
        {
            rb.MovePosition(rb.position + knockbackVector * PlayerStats.Instance.equippedWeapon.Base.Knockback * Time.fixedDeltaTime);
        }
        

        if (inDamageRange)
        {
            PlayerStats.Instance.TakeDamage(damage, transform.position);
        }
        
    }

    public void FollowPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    public bool TakeDamage(int damage, Vector3 incomingDirection)
    {
        health -= damage;


        // If the enemy has died
        if (health <= 0)
        {
            return true;
        }

        // Knockback
        Vector3 _newDirection = (transform.position - incomingDirection);

        Debug.Log(knockbackVector);
        knockbackVector = new Vector2(_newDirection.x, _newDirection.y);

        Debug.Log(knockbackVector);
        locked = true;
        
        StartCoroutine(StopKnockback());

        return false;
    }

    public void Death()
    {
        if (Random.Range(0f, 1f) < 0.2)
        {
            Instantiate(weaponDropPrefab, transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }

    private IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(0.1f);
        knockbackVector = new Vector2(0, 0);
        locked = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            inDamageRange = true;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            inDamageRange = false;
        }
    }
    
}
