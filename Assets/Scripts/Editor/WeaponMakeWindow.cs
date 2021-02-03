using UnityEngine;
using UnityEditor;

public class WeaponMakeWindow : EditorWindow
{
    static string weaponName = "New Weapon";
    public static Weapon weapon;
    static bool unlocked = false;
    static GameObject prefab = null;
    static float damage = 20;
    static float fireRate = 1.5f;
    static float range = 100;
    static float explosionRadius = 0;

    static Weapon previousWeapon = null;

    [MenuItem("Window/Make A Weapon")]
    public static void ShowWindow()
    {
        GetWindow<WeaponMakeWindow>("Make or edit a weapon");
    }

    static void ApplyToWeapon()
    {
        if (weapon == null) return;

        weapon.name = weaponName;
        weapon.prefab = prefab;
        weapon.damage = damage;
        weapon.shootSpeed = fireRate;
        weapon.bulletRange = range;
        weapon.unlocked = unlocked;
        weapon.explosionRadius = explosionRadius;
    }

    public static void CreateNewWeapon()
    {
        weapon = CreateInstance<Weapon>();
        EditorUtility.SetDirty(weapon);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        ApplyToWeapon();
    }

    static void ApplyToStats()
    {
        previousWeapon = weapon;
        if (weapon == null) return;

        unlocked = weapon.unlocked;
        damage = weapon.damage;
        prefab = weapon.prefab;
        fireRate = weapon.shootSpeed;
        range = weapon.bulletRange;
        weaponName = weapon.name;
        explosionRadius = weapon.explosionRadius;
    }

    void OnGUI()
    {
        GUILayout.Label("Make or edit a weapon here:", EditorStyles.boldLabel);

        weapon = (Weapon)EditorGUILayout.ObjectField("Weapon :", weapon, typeof(Weapon), false);

        if (weapon != previousWeapon) ApplyToStats();

        weaponName = EditorGUILayout.TextField("Name :", weaponName);
        prefab = (GameObject)EditorGUILayout.ObjectField("Prefab :", prefab, typeof(GameObject), true);
        damage = EditorGUILayout.FloatField("Damage :", damage);
        fireRate = EditorGUILayout.FloatField("FireRate :", fireRate);
        range = EditorGUILayout.FloatField("Range :", range);
        explosionRadius = EditorGUILayout.FloatField("ExplosionRadius", explosionRadius);
        unlocked = EditorGUILayout.Toggle("Unlocked", unlocked);

        if (GUILayout.Button("Apply To Weapon"))
        {
            ApplyToWeapon();
        }

        if(GUILayout.Button("Create new weapon"))
        {
            CreateNewWeapon();
        }
    }
}
