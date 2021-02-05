using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] GameObject player;

    // Other Player Scripts
    [SerializeField] PlayerAttack playerAttack;

    public int maxHealth;
    public int health;
    [SerializeField] WeaponBase defaultWeapon;

    Weapon equippedWeapon;

    void Start()
    {
        equippedWeapon = new Weapon(defaultWeapon);
        SetWeapon(equippedWeapon);
    }

    public void SetWeapon(Weapon w)
    {
        if (w != null)
        {
            playerAttack.magnitude = w.Base.Range;
            playerAttack.radius = w.Base.Radius;
            playerAttack.duration = w.Base.Duration;

        }
    }
}
