using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] CircleCollider2D hitbox;
    [SerializeField] Transform target;
    [SerializeField] float speed;
    [SerializeField] float maxRange;
    [SerializeField] float minRange;

    void Start()
    {
        target = FindObjectOfType<PlayerMovement>().transform;
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(target.position, transform.position) <= maxRange && Vector3.Distance(target.position, transform.position) >= minRange)
        {
            FollowPlayer();
        }
    }

    public void FollowPlayer()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("Collision");
        }
    }
    
}
