using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    [SerializeField] WeaponBase[] weaponBase;

    public Weapon weapon;

    bool playerOn;

    void Start()
    {
        weapon = new Weapon(weaponBase[Random.Range(0, weaponBase.Length)]);
    }

    void Update()
    {
        if (playerOn)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Swapped!");
                SwapWeapons();
            }
        }
    }

    void SwapWeapons()
    {
        Weapon _tempWeapon = weapon;
        weapon = PlayerStats.Instance.GetWeapon();
        PlayerStats.Instance.SetWeapon(_tempWeapon);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerOn = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            playerOn = false;
        }
    }
}
