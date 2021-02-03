using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WeaponDrop))]
public class WeaponDropEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        WeaponDrop weaponDrop = (WeaponDrop)target;

        if (GUILayout.Button("Edit Weapon"))
        {
            EditorWindow.GetWindow<WeaponMakeWindow>();

            WeaponMakeWindow.weapon = weaponDrop.drop;
        }
        
        if (GUILayout.Button("New Weapon"))
        {
            EditorWindow.GetWindow<WeaponMakeWindow>();

            weaponDrop.drop = WeaponMakeWindow.weapon;
        }

        if(GUILayout.Button("Turn Into Weapon Drop"))
        {
            weaponDrop.TurnIntoDrop();
        }
    }
}
