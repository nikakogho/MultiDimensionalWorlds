using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public SpawnStats[] spawns;
    public int amount;
    //public Transform spawnParent;
    //private Transform[] spawnPoints;
    public Vector3 maxPoint, minPoint;

    /*void Awake()
    {
        spawnPoints = new Transform[spawnParent.childCount];

        for(int i = 0; i < spawnParent.childCount; i++)
        {
            spawnPoints[i] = spawnParent.GetChild(i);
        }
    }*/

    void Start()
    {
        float total = 0;
        foreach (SpawnStats spawn in spawns) total += spawn.chance;

        for (int i = 0; i < amount; i++)
        {
            float r = Random.Range(0, total);
            float sum = 0;
            int index;

            for (index = 0; sum < r; index++)
            {
                sum += spawns[index].chance;
            }

            SpawnStats toSpawn = spawns[index - 1];
            //Transform spawnPlace = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Vector3 spawnPosition = new Vector3(Random.Range(minPoint.x, maxPoint.x), 0, Random.Range(minPoint.z, maxPoint.z));
            Quaternion spawnRotation = Quaternion.identity;

            GameMaster4D.instance.objects.Add(Instantiate(toSpawn.prefab, spawnPosition, spawnRotation, transform).GetComponent<Object4D>());
        }
    }

    [System.Serializable]
    public struct SpawnStats
    {
        public GameObject prefab;
        public float chance;
    }
}
