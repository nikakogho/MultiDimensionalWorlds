using UnityEngine;

public class Portal : MonoBehaviour
{
    public float w1, w2;
    public bool DestroyOnContact = false;

    void OnTriggerEnter(Collider other)
    {
        Object4D obj = other.GetComponent<Object4D>();

        if (obj == null) return;

        if (obj.t4.v4Pos.w == w2)
            obj.t4.v4Pos.w = w1;
        else
        obj.t4.v4Pos.w = w2;

        Player4D.teleported = true;

        if (DestroyOnContact) Destroy(gameObject);
    }
}
