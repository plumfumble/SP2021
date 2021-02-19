using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBomb : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject hitbox;

    float timer;
    float speed;
    bool exploding;
    
    Vector2 direction;

    public void Initialize(float t, float s, Vector2 dir)
    {
        timer = t;
        speed = s;
        direction = dir;
        FuseTime();
    }

    private void FixedUpdate()
    {
        Debug.Log("[PlayerBomb.cs] Direction: " + direction);
        if (exploding)
        {
            hitbox.SetActive(true);
        }

        if (speed >= 1)
        {
            speed *= 0.9f;
        }
        else
        {
            speed = 0;
        }
        
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    IEnumerator FuseTime()
    {
        yield return new WaitForSeconds(timer);
        exploding = true;
    }
}
