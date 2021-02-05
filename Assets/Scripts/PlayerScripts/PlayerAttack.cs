using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] GameObject hitbox;
    [SerializeField] GameObject player;

    public bool melee = true;

    public float magnitude = 1;
    public float radius = 0.09f;
    public float duration = 0.5f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (melee)
            {
                StartCoroutine(CreateHitbox());
            }
        }
    }

    IEnumerator CreateHitbox()
    {
        // get the mouse position and where it is relative to the player
        Vector3 mousePos = Input.mousePosition;
        Vector3 clickPoint = Camera.main.ScreenToWorldPoint(mousePos);// - player.transform.position).normalized;
        clickPoint.z = 0;

        hitbox.transform.parent = player.transform;
        
        // find the angle between the two points
        float angle = (float)(GetAngle(new Vector2(clickPoint.x, clickPoint.y), new Vector2(player.transform.position.x, player.transform.position.y))) * Mathf.Deg2Rad;

        // put in the values based on the weapon
        Vector2 newHitboxPos = new Vector2(-(magnitude * Mathf.Cos(angle)), -(magnitude * Mathf.Sin(angle)));
        hitbox.GetComponent<CircleCollider2D>().radius = radius;

        // set the hitbox's position and start it
        hitbox.transform.position = player.transform.position + new Vector3(newHitboxPos.x, newHitboxPos.y, 0);
        hitbox.SetActive(true);

        yield return new WaitForSeconds(duration);
        
        // reset the position after the duration
        hitbox.transform.position = new Vector3(0, 0, 0);
        hitbox.SetActive(false);
    }

    double GetAngle(Vector2 me, Vector2 target)
    {
        return Math.Atan2(target.y - me.y, target.x - me.x) * (180 / Math.PI);
    }
}
