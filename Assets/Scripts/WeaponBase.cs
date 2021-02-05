using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class WeaponBase : ScriptableObject
{
    [SerializeField] string name;

    [SerializeField] Sprite droppedSprite;
    [SerializeField] Sprite uiSprite;

    // Melee Stats
    [SerializeField] float range;
    [SerializeField] float radius;
    [SerializeField] float duration;
    [SerializeField] float cooldown;

    [SerializeField] bool melee = true;
    // Ranged Stuff

    public string Name
    {
        get { return name; }
    }

    public Sprite Dropped
    {
        get { return droppedSprite; }
    }

    public Sprite UI
    {
        get { return uiSprite; }
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


}
