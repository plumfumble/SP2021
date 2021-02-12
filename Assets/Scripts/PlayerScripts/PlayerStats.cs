using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [SerializeField] GameObject player;
    Rigidbody2D rb;

    // Other Player Scripts
    [SerializeField] PlayerAttack playerAttack;
    [SerializeField] PlayerMovement playerMovement;

    public int maxHealth;
    public int health;


    private Vector2 knockbackVector;
    private bool invincible;
    private float invincibilityDuration = 0.5f;

    [SerializeField] WeaponBase defaultWeapon;

    public Weapon equippedWeapon;

    void Start()
    {
        Instance = this;
        rb = player.GetComponent<Rigidbody2D>();
        Weapon _equip = new Weapon(defaultWeapon);
        SetWeapon(_equip);
    }

    void FixedUpdate()
    {
        if (playerMovement.locked)
        {
            rb.MovePosition(rb.position + knockbackVector * 15 * Time.fixedDeltaTime);
        }
    }


    public void SetWeapon(Weapon w)
    {
        if (w != null)
        {
            equippedWeapon = w;
            playerAttack.magnitude = w.Base.Range;
            playerAttack.radius = w.Base.Radius;
            playerAttack.duration = w.Base.Duration;

            playerAttack.melee = w.Base.Melee;
        }
    }

    public Weapon GetWeapon()
    {
        return equippedWeapon;
    }

    public bool TakeDamage(int damage, Vector3 incomingDirection)
    {
        if (!invincible)
        {
            health -= damage;

            // If the player has died
            if (health <= 0)
            {
                return true;
            }

            // Knockback
            Vector3 _newDirection = (transform.position - incomingDirection);
            knockbackVector = new Vector2(_newDirection.x, _newDirection.y);
            playerMovement.locked = true;

            invincible = true;
            StartCoroutine(InvincibleCounter());
            StartCoroutine(StopKnockback());
        }
        return false;
    }

    private IEnumerator InvincibleCounter()
    {
        yield return new WaitForSeconds(invincibilityDuration);
        invincible = false;
    }

    private IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(0.1f);
        knockbackVector = new Vector2(0, 0);
        playerMovement.locked = false;
    }
}
