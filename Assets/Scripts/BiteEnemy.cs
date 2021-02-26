using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiteEnemy : MonoBehaviour
{
    [SerializeField] CircleCollider2D hitbox;
    [SerializeField] Transform target;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject weaponDropPrefab;
    [SerializeField] GameObject expPrefab;

    [SerializeField] int maxHealth;
    [SerializeField] int health;

    [SerializeField] float speed;
    [SerializeField] float maxRange;
    [SerializeField] float minRange;
    [SerializeField] int damage = 1;
    [SerializeField] float meleeRange;
    [SerializeField] float magnitude;

    [SerializeField] GameObject biteHitbox;

    [SerializeField] int expDrop;

    private bool inDamageRange;

    private bool locked;
    private Vector2 knockbackVector;

    void Start()
    {
        health = maxHealth;
        target = FindObjectOfType<PlayerMovement>().transform;
        expDrop += UnityEngine.Random.Range(0, 6);
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


        if (Vector3.Distance(this.transform.position, target.position) < meleeRange)
        {
            Debug.Log("In Range");
            // find the angle between the two points
            float angle = (float)(GetAngle(new Vector2(target.position.x, target.position.y), new Vector2(this.transform.position.x, this.transform.position.y))) * Mathf.Deg2Rad;

            // put in the values based on the weapon
            Vector2 newHitboxPos = new Vector2(-(magnitude * Mathf.Cos(angle)), -(magnitude * Mathf.Sin(angle)));
            //biteHitbox.GetComponent<CircleCollider2D>().radius = radius;

            // set the hitbox's position and start it
            biteHitbox.transform.position = this.transform.position + new Vector3(newHitboxPos.x, newHitboxPos.y, 0);
            biteHitbox.SetActive(true);
            StartCoroutine(EndHitbox());
            locked = true;
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
        if (UnityEngine.Random.Range(0f, 1f) < 0.2)
        {
            Instantiate(weaponDropPrefab, transform.position, Quaternion.identity);
        }
        for (int i = 0; i < expDrop; i++)
        {
            GameObject _e = Instantiate(expPrefab, transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)), Quaternion.identity);
            _e.GetComponent<Experience>().CreateExp(1);
        }
        Destroy(this.gameObject);
    }

    private IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(0.1f);
        knockbackVector = new Vector2(0, 0);
        locked = false;
    }

    IEnumerator EndHitbox()
    {
        yield return new WaitForSeconds(0.8f);
        locked = false;
        biteHitbox.transform.position = this.gameObject.transform.position;
        biteHitbox.SetActive(false);
    }

    double GetAngle(Vector2 me, Vector2 target)
    {
        return Math.Atan2(target.y - me.y, target.x - me.x) * (180 / Math.PI);
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
