using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{ 
    public WeaponBase Base { get; set; }

    public int Ammo { get; set; }

    public Weapon(WeaponBase _wbase)
    {
        Base = _wbase;

        Ammo = _wbase.MaxAmmo;
        Debug.Log(Ammo);
    }

    public bool Fire()
    {
        if (Ammo > 0)
        {
            Ammo--;
            return true;
        }
        return false;
    }
}
