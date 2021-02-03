using UnityEngine;

public class WeaponDrop : MonoBehaviour
{
    public Weapon drop;
    public System.Type windowType;
    bool entered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) entered = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) entered = false;
    }

    bool over = false;

    [ContextMenu("Turn into drop")]
    public void TurnIntoDrop()
    {
        SphereCollider sphere = GetComponent<SphereCollider>();
        if (sphere == null) sphere = gameObject.AddComponent<SphereCollider>();
        sphere.radius *= 4.5f;
        sphere.isTrigger = true;

        MeshCollider meshCol = GetComponent<MeshCollider>();
        if (meshCol == null) meshCol = gameObject.AddComponent<MeshCollider>();
        meshCol.convex = true;
        meshCol.sharedMesh = GetComponent<MeshFilter>().sharedMesh;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        transform.rotation = Quaternion.Euler(0, 0, 90);
    }

    void Update()
    {
        if (entered && !over)
        {
            if (Input.GetKeyDown("e"))
            {
                over = true;
                WeaponSwitch.instance.UnlockWeapon(drop);
                Transform[] kids = new Transform[transform.childCount];
                for (int i = 0; i < transform.childCount; i++) kids[i] = (transform.GetChild(i));
                foreach (Transform t in kids) Destroy(t.gameObject);
                Destroy(GetComponent<Collider>());
                Destroy(GetComponent<Rigidbody>());
                Renderer rend = GetComponent<Renderer>();
                if (rend != null) Destroy(rend);
                Destroy(this);
            }
        }
    }
}
