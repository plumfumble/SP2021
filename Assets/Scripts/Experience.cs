using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    bool burst = true;

    private Transform target;
    private Vector2 direction;
    private Vector3 newPos;
    private int value;
    private float speed = 1;

    private void Start()
    {
        target = GameObject.Find("Player").transform;
    }

    private void FixedUpdate()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        speed += 0.02f;
    }

    public void CreateExp(int val)
    {
        value = val;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerStats.Instance.GainExperience(value);
            Destroy(this.gameObject);
        }
    }
}
