using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { None, Melee, Ranged, Bomb }

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponBase : ScriptableObject
{
    [SerializeField] string name;

    [SerializeField] WeaponType type;

    [SerializeField] Sprite droppedSprite;
    [SerializeField] Sprite uiSprite;

    [SerializeField] int damage;
    [SerializeField] float knockback;

    // Melee Stats
    [SerializeField] float range;
    [SerializeField] float radius;
    [SerializeField] float duration;
    [SerializeField] float cooldown;

    [SerializeField] bool melee = true;

    // Ranged Stuff
    [SerializeField] int maxAmmo;
    [SerializeField] float shotSpeed;
    [SerializeField] GameObject bullet;

    // Bomb Stuff
    [SerializeField] GameObject bombHitbox;

    public string Name
    {
        get { return name; }
    }

    public WeaponType Type
    {
        get { return type; }
    }

    public Sprite Dropped
    {
        get { return droppedSprite; }
    }

    public Sprite UI
    {
        get { return uiSprite; }
    }

    public int Damage
    {
        get { return damage; }
    }

    public float Knockback
    {
        get { return knockback; }
    }

    public float Range
    {
        get { return range; }
    }

    public float Radius
    {
        get { return radius; }
    }

    public float Duration
    {
        get { return duration; }
    }

    public float Cooldown
    {
        get { return cooldown; }
    }

    public bool Melee
    {
        get { return melee; }
    }

    public int MaxAmmo
    {
        get { return maxAmmo; }
    }

    public float ShotSpeed
    {
        get { return shotSpeed; }
    }

    public GameObject Bullet
    {
        get { return bullet; }
    }

    public GameObject BombHitbox
    {
        get { return bombHitbox; }
    }

}
