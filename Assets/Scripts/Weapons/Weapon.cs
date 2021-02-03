using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Inventory/Weapon")]
public class Weapon : ScriptableObject
{
    new public string name;
    public bool unlocked;
    public GameObject prefab;
    public float damage;
    public float shootSpeed;
    public float bulletRange;

    public float explosionRadius = 0;

    void OnValidate()
    {
        if (name == string.Empty) name = base.name;
    }
}
