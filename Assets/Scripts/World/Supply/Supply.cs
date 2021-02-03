using UnityEngine;
using System.Collections;

public class Supply : MonoBehaviour
{
    public SupplyType supplyType;
    public int value;
    public UnlockType unlockType = UnlockType.None;
    bool opened = false;
    [HideInInspector]public int index;
    private SupplySpawner spawner;

    public GameObject deathEffect;

    void Start()
    {
        spawner = SupplySpawner.instance;
    }

    void OnTriggerEnter(Collider other)
    {
        if (opened) return;

        if (other.CompareTag("Player"))
        {
            opened = true;
            switch (supplyType)
            {
                case SupplyType.Health:
                    PlayerStats.health += value;
                    break;
                case SupplyType.Bullets:
                    PlayerStats.bullets += value;
                    break;
                case SupplyType.Money:
                    PlayerStats.money += value;
                    break;
            }

            float time = 4;

            switch (unlockType)
            {
                case UnlockType.Anim:
                    GetComponent<Animator>().SetTrigger("Open");
                    break;
                case UnlockType.Effect:
                    GameObject effectObject = Instantiate(deathEffect, transform.position, transform.rotation);
                    effectObject.transform.localScale = transform.localScale;
                    Destroy(effectObject, time);
                    time = 0;
                    break;
                default:
                    time = 0;
                    break;
            }

            StartCoroutine(Replace(time));
        }
    }

    IEnumerator Replace(float time)
    {
        yield return new WaitForSeconds(time);

        spawner.ApplySupply(index);
        Player4D.instance.Moved = true;
        Destroy(gameObject);
    }

    public enum UnlockType { Anim, Effect, None }
    public enum SupplyType { Health, Bullets, Money }
}
