using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Unity.Netcode;


public class EnemySpawner : MonoBehaviour // Classe que gerencia a cria��o de inimigos no jogo, aumentando a dificuldade em ondas.
{
    public static EnemySpawner instance;
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs; // Array de prefabs de inimigos a serem instanciados.

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8; // Quantidade b�sica de inimigos por onda.
    [SerializeField] private float enemiesPerSecond = 0.5f; // Taxa de aparecimento dos inimigos por segundo.
    [SerializeField] private float timebetweenWaves = 5f; // Tempo de espera entre ondas de inimigos.
    [SerializeField] private float difficultyScallingFactor = 0.75f; // Fator de escala da dificuldade, aumentando o n�mero de inimigos por onda.
    [SerializeField] private float healthIncreasePerWave = 2f; // Fator de aumento de vida dos inimigos por onda.
    [SerializeField] private float SpeedIncreasePerWave = 1.5f;

    public Transform spawnPoint;   // Ponto de spawn dos inimigos
    public Transform endPoint;     // Ponto final ou base



    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent(); // Evento acionado quando um inimigo � destru�do.

    private int currentwave = 1; // Contador da onda atual.
    private float timesinceLastSpawn; // Tempo decorrido desde o �ltimo inimigo gerado.
    private float enemiesAlive; // N�mero de inimigos vivos atualmente no jogo.
    private int enemiesLeftToSpawn; // N�mero de inimigos restantes para aparecer na onda atual.
    private bool isSpawning = false; // Controla se os inimigos est�o sendo gerados.
    private float currentHealthModifier = 1.5f; // Modificador de sa�de inicial (aumenta a cada onda).

    private void Awake() // M�todo chamado ao iniciar o script, antes do Start.
    {
        instance = this;
        onEnemyDestroy.AddListener(EnemyDestroyed); // Adiciona o m�todo EnemyDestroyed como ouvinte para o evento onEnemyDestroy.
    }


    private void Start() // M�todo inicial que configura o estado do jogo ao iniciar.
    {
        StartCoroutine(StartWave()); // Inicia a primeira onda de inimigos ap�s um per�odo de espera.
    }

    private void Update() // Chamado a cada quadro para atualizar o estado da cria��o de inimigos.
    {
        
    
        if (!isSpawning || !NetworkManager.Singleton.IsServer) return;

        timesinceLastSpawn += Time.deltaTime; // Atualiza o tempo desde a �ltima gera��o de inimigos.

        // Verifica se � hora de criar um novo inimigo e se ainda restam inimigos para gerar na onda atual.
        if (timesinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy(); // Gera um inimigo.
            enemiesLeftToSpawn--; // Decrementa o contador de inimigos restantes para gerar.
            enemiesAlive++; // Incrementa o n�mero de inimigos vivos no jogo.
            timesinceLastSpawn = 0f; // Reseta o tempo desde o �ltimo inimigo gerado.
        }

        // Se n�o restam inimigos para gerar e todos os inimigos foram destru�dos, termina a onda.
        if (enemiesLeftToSpawn == 0 && enemiesAlive == 0)
        {

            Endwave(); // Finaliza a onda atual e prepara para a pr�xima.
        }
    }

    private void Endwave() // M�todo para finalizar a onda atual.
    {
        isSpawning = false; // Para a gera��o de inimigos.
        timesinceLastSpawn = 0f; // Reseta o tempo desde o �ltimo inimigo gerado.


        currentwave++; // Incrementa o n�mero da onda atual.
                       // Multiplica o modificador de sa�de em vez de increment�-lo
        currentHealthModifier *= healthIncreasePerWave;
        StartCoroutine(StartWave()); // Inicia a pr�xima onda ap�s o per�odo de espera.
        AdManager.instance.ShowNextAd(); // Exibe o pr�ximo an�ncio
    }

    private void EnemyDestroyed() // M�todo chamado quando um inimigo � destru�do.
    {
        enemiesAlive--; ;
    }

    private IEnumerator StartWave() // Coroutine que inicia uma nova onda ap�s um tempo de espera.
    {
        yield return new WaitForSeconds(timebetweenWaves); // Aguarda o tempo definido entre ondas.
        isSpawning = true; // Ativa a gera��o de inimigos.
        enemiesLeftToSpawn = EnemiesPerwave(); // Calcula o n�mero de inimigos a serem gerados na onda atual.

    }

    private void SpawnEnemy()
    {
        GameObject prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        GameObject enemy = Instantiate(prefabToSpawn, LevelManager.instance.startPoint.position, Quaternion.identity);

        NetworkObject netObj = enemy.GetComponent<NetworkObject>();
        if (netObj != null && NetworkManager.Singleton.IsServer)
        {
            netObj.Spawn(); // Torna o inimigo vis�vel para todos os clientes
        }

        // Aumenta a vida do inimigo de acordo com o modificador atual
        Health enemyHealth = enemy.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.hitPoints *= Mathf.Pow(healthIncreasePerWave, currentwave - 1);
        }
    }






    private int EnemiesPerwave() // Calcula o n�mero de inimigos para a onda atual com base na dificuldade.
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentwave, difficultyScallingFactor)); // Aumenta o n�mero de inimigos por onda de acordo com o fator de dificuldade.
    }
}