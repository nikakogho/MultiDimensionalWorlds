using UnityEngine;

public class SupplySpawner : MonoBehaviour
{
    public SupplyBlueprint[] supplies;
    public Object4D obj;
    public Vector3 maxPoint, minPoint;
    
    public int spawnAmount = 15;
    public static SupplySpawner instance;
    float total = 0;

    void Awake()
    {
        instance = this;
        foreach (SupplyBlueprint supply in supplies) total += supply.chance;
        obj.rends = new RenderData[spawnAmount];
        Spawn();
    }

    void Spawn()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            ApplySupply(i);
        }
    }

    public void ApplySupply(int i)
    {
        Vector3 position = StaticFunctions.RandomLerp(minPoint, maxPoint);

        int index = -1;

        float sum = 0;
        float r = Random.Range(0, total);

        do
        {
            index++;
            sum += supplies[index].chance;
        } while (sum < r);

        GameObject supplyToSpawn = supplies[index].prefab;

        GameObject spawned = Instantiate(supplyToSpawn, position, Quaternion.identity, obj.transform);

        obj.rends[i] = new RenderData
        {
            face = spawned,
            needsVisibleRange = true,
            w = Random.Range(0f, 32.0f),
            visibleRange = Random.Range(0.2f, 0.3f),
            normalScale = spawned.transform.localScale,
            applyW = false
        };

        spawned.GetComponent<Supply>().index = i;
    }
    
    [System.Serializable]
    public struct SupplyBlueprint
    {
        public GameObject prefab;
        public float chance;
    }
}
