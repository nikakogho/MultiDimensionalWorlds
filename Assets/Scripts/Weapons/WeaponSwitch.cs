using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitch : MonoBehaviour
{
    List<KeyValuePair<GameObject, Weapon>> weapons = new List<KeyValuePair<GameObject, Weapon>>();
    KeyValuePair<GameObject, Weapon>? currentWeapon = null;

    public static WeaponSwitch instance;
    public Image cursor;

    private float countdown = 0;

    void Awake()
    {
        instance = this;
    }

    public void UnlockWeapon(Weapon weapon)
    {
        weapon.unlocked = true;

        if (currentWeapon == null) cursor.enabled = true;

        GameObject clone = Instantiate(weapon.prefab, transform.position, transform.rotation, transform);
        weapons.Add(new KeyValuePair<GameObject, Weapon>(clone, weapon));

        SwapWeapon(weapons.Count - 1);
    }

    void SwapWeapon(int index)
    {
        if (index > weapons.Count) return;

        if(currentWeapon.HasValue) currentWeapon.Value.Key.SetActive(false);

        currentWeapon = weapons[index];

        currentWeapon.Value.Key.SetActive(true);
    }

    void Update()
    {
        if (PlayerStats.bullets == 0) return;

        countdown -= Time.deltaTime;

        if(currentWeapon.HasValue)
        {
            if(countdown <= 0)
            {
                if (Input.GetMouseButton(0))
                {
                    Shoot();
                }
            }
        }

        for (int i = 1; i <= 9; i++) if (Input.GetKeyDown(i.ToString())) SwapWeapon(i - 1);
    }

    void Shoot()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, currentWeapon.Value.Value.bulletRange))
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();

            if (enemy != null) enemy.TakeDamage(currentWeapon.Value.Value.damage);
        }

        Animator anim = currentWeapon.Value.Key.GetComponent<Animator>();
        if(anim != null)
        anim.SetTrigger("shoot");
        countdown = 1f / currentWeapon.Value.Value.shootSpeed;

        PlayerStats.bullets--;
    }
}
