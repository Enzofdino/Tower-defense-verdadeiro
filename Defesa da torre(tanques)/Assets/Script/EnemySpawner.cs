using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timebetweenWaves = 5f;
    [SerializeField] private float difficultyScallingFactor = 0.75f;
    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new  UnityEvent();


    private int currentwave = 1;
    private float timesinceLastSpawn;
    private int enemiesAlive;
    private int enemiesLeftToSpawn;
    private bool isSpawning = false;

    private void Awake()
    {
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }
    private void Start()
    {
        StartCoroutine(StartWave());
    }
    private void Update()
    {
        if(!isSpawning) return;

        timesinceLastSpawn += Time.deltaTime;

        if(timesinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn >0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timesinceLastSpawn = 0f;
        }
        if(enemiesAlive == 0 && enemiesLeftToSpawn == 0)
        {
            Endwave();
        }
    }
    private void Endwave()
    {
        isSpawning = false;
        timesinceLastSpawn = 0f;
        currentwave++;
        StartCoroutine(StartWave());
    }
    private void EnemyDestroyed()
    {
        enemiesAlive--;
    }
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timebetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerwave();
    }

    private void SpawnEnemy()
    {
        GameObject PrefabTospawn = enemyPrefabs[0];
        Instantiate(PrefabTospawn, LevelManager.instance.startpoint.position, Quaternion.identity);
    }
    private int EnemiesPerwave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentwave, difficultyScallingFactor));
    }
   
}
