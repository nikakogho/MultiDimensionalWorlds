using UnityEngine;

public class FallFixer : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        other.transform.position += Vector3.up * 200;
    }
}
