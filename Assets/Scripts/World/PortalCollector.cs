using UnityEngine;

public class PortalCollector : MonoBehaviour
{
    bool entered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) entered = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) entered = false;
    }

    void Update()
    {
        if (entered)
        {
            if (Input.GetKeyDown("e"))
            {
                Player4D.portalGunUnlocked = true;
                Player4D.instance.ActivatePortalGun();

                Transform[] kids = new Transform[transform.childCount];

                for (int i = 0; i < kids.Length; i++) kids[i] = transform.GetChild(i);

                foreach (Transform t in kids) Destroy(t.gameObject);
                Destroy(this);
            }
        }
    }
}
