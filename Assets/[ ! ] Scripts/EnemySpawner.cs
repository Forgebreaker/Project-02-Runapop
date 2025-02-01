using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] EnemiesList;
    [SerializeField] private GameObject[] SpawnersList;
    [SerializeField] private GameObject coinPrefab;

    private int RandomMonsterIndex;
    private int RandomSpawnerIndex;

    private float CurrentCoinSpawnTime = 1.5f;
    private float spawnInterval = 1.5f;
    private float difficultyIncreaseInterval = 10f;
    private float minimumSpawnInterval = 1.1f;

    void Start()
    {
        StartCoroutine(SpawnMonster());
        StartCoroutine(IncreaseDifficulty());
    }

    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Coin") == null && CurrentCoinSpawnTime <= 0)
        {
            CurrentCoinSpawnTime = 1.5f; 
            float randomX = Random.Range(-10f, 10f);
            Vector3 coinPosition = new Vector3(randomX, 0f, 0f);
            Instantiate(coinPrefab, coinPosition, Quaternion.identity);
        }
        if (GameObject.FindGameObjectWithTag("Coin") == null && CurrentCoinSpawnTime > 0)
        {
            CurrentCoinSpawnTime -= Time.deltaTime;
        }
    }


    IEnumerator SpawnMonster()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            RandomSpawnerIndex = Random.Range(0, SpawnersList.Length);
            RandomMonsterIndex = Random.Range(0, EnemiesList.Length);

            Instantiate(EnemiesList[RandomMonsterIndex],
                        SpawnersList[RandomSpawnerIndex].transform.position,
                        SpawnersList[RandomSpawnerIndex].transform.rotation);
        }
    }

    IEnumerator IncreaseDifficulty()
    {
        while (true)
        {
            yield return new WaitForSeconds(difficultyIncreaseInterval);
            spawnInterval = Mathf.Max(minimumSpawnInterval, spawnInterval - 0.1f);
        }
    }
}
