using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public EnemyWave[] waves;
    int currentWaveIndex = -1;
    public static int alive = 0;
    public Vector3 maxPoint, minPoint;
    private bool endedSpawning = false;
    private int leftToSpawn = 0;

    private float countdown = 0;

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnEnable()
    {
        if (leftToSpawn > 0) StartCoroutine(SpawnWave());
        else NextWave();
    }

    void Update()
    {
        if (!endedSpawning) return;

        countdown -= Time.deltaTime;

        if (countdown <= 0 || alive == 0)
            NextWave();
    }

    void SpawnReward()
    {
        Reward reward = waves[currentWaveIndex].reward;

        float total = 0;
        foreach (SupplySpawner.SupplyBlueprint supply in reward.supplies) total += supply.chance;

        Transform parent = new GameObject("Reward " + currentWaveIndex).transform;

        for (int i = 0; i < reward.amount; i++)
        {
            float r = Random.Range(0, total);
            float sum = 0;
            int index = -1;

            do
            {
                index++;
                sum += reward.supplies[index].chance;
            } while (sum < r);

            GameObject toSpawn = reward.supplies[index].prefab;
            Vector3 randomOffset = StaticFunctions.RandomLerp(minPoint, maxPoint);
            randomOffset.y = 0;
            Vector3 position = transform.position + reward.offset + randomOffset;
            Quaternion rotation = Quaternion.identity;

            Instantiate(toSpawn, position, rotation, parent);
        }
    }

    void NextWave()
    {
        if(currentWaveIndex >= 0)
        {
            SpawnReward();
        }

        currentWaveIndex++;
        if (currentWaveIndex == waves.Length) currentWaveIndex = 0;
        leftToSpawn = waves[currentWaveIndex].amount;
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        endedSpawning = false;

        for(; leftToSpawn > 0; leftToSpawn--)
        {
            SpawnEnemy();

            yield return new WaitForSeconds(waves[currentWaveIndex].spawnDelta);
        }

        endedSpawning = true;
        countdown = waves[currentWaveIndex].waitTime;
    }

    void SpawnEnemy()
    {
        float total = 0;
        foreach (EnemyBlueprint blueprint in waves[currentWaveIndex].enemies) total += blueprint.chance;
        float r = Random.Range(0, total);

        int index = -1;

        float sum = 0;

        do
        {
            index++;
            sum += waves[currentWaveIndex].enemies[index].chance;
        } while (sum < r);

        GameObject chosen = waves[currentWaveIndex].enemies[index].prefab;
        Vector3 position = new Vector3(Random.Range(minPoint.x, maxPoint.x), transform.position.y, Random.Range(minPoint.z, maxPoint.z));
        Quaternion rotation = Quaternion.identity;

        Instantiate(chosen, position, rotation, transform);

        alive++;
    }

    void FixedUpdate()
    {
        if (endedSpawning && alive == 0) NextWave();
    }

    [System.Serializable]
    public class EnemyWave
    {
        public EnemyBlueprint[] enemies;
        public int amount;
        public float spawnDelta;
        public float waitTime;
        public Reward reward;
    }

    [System.Serializable]
    public struct Reward
    {
        public Vector3 offset;
        public int amount;
        public SupplySpawner.SupplyBlueprint[] supplies;
    }

    [System.Serializable]
    public struct EnemyBlueprint
    {
        public GameObject prefab;
        public float chance;
    }
}
