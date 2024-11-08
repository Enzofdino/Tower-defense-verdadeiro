using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour // Classe que gerencia a criação de inimigos no jogo, aumentando a dificuldade em ondas.
{
    public static EnemySpawner instance; 
    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs; // Array de prefabs de inimigos a serem instanciados.

    [Header("Attributes")]
    [SerializeField] private int baseEnemies = 8; // Quantidade básica de inimigos por onda.
    [SerializeField] private float enemiesPerSecond = 0.5f; // Taxa de aparecimento dos inimigos por segundo.
    [SerializeField] private float timebetweenWaves = 5f; // Tempo de espera entre ondas de inimigos.
    [SerializeField] private float difficultyScallingFactor = 0.75f; // Fator de escala da dificuldade, aumentando o número de inimigos por onda.
    [SerializeField] private float healthIncreasePerWave = 1.2f; // Fator de aumento de vida dos inimigos por onda.

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent(); // Evento acionado quando um inimigo é destruído.

    private int currentwave = 1; // Contador da onda atual.
    private float timesinceLastSpawn; // Tempo decorrido desde o último inimigo gerado.
    private int enemiesAlive; // Número de inimigos vivos atualmente no jogo.
    private int enemiesLeftToSpawn; // Número de inimigos restantes para aparecer na onda atual.
    private bool isSpawning = false; // Controla se os inimigos estão sendo gerados.
    private float currentHealthModifier = 1.5f; // Modificador de saúde inicial (aumenta a cada onda).

    private void Awake() // Método chamado ao iniciar o script, antes do Start.
    {
        instance = this;
        onEnemyDestroy.AddListener(EnemyDestroyed); // Adiciona o método EnemyDestroyed como ouvinte para o evento onEnemyDestroy.
    }
    private void OnDestroy()
    {
        onEnemyDestroy.RemoveListener(EnemyDestroyed); // Remove o ouvinte para evitar problemas ao destruir.
    }

    private void Start() // Método inicial que configura o estado do jogo ao iniciar.
    {
        StartCoroutine(StartWave()); // Inicia a primeira onda de inimigos após um período de espera.
    }

    private void Update() // Chamado a cada quadro para atualizar o estado da criação de inimigos.
    {
        if (!isSpawning) return; // Se não for o momento de gerar inimigos, retorna.

        timesinceLastSpawn += Time.deltaTime; // Atualiza o tempo desde a última geração de inimigos.

        // Verifica se é hora de criar um novo inimigo e se ainda restam inimigos para gerar na onda atual.
        if (timesinceLastSpawn >= (1f / enemiesPerSecond) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy(); // Gera um inimigo.
            enemiesLeftToSpawn--; // Decrementa o contador de inimigos restantes para gerar.
            enemiesAlive++; // Incrementa o número de inimigos vivos no jogo.
            timesinceLastSpawn = 0f; // Reseta o tempo desde o último inimigo gerado.
        }

        // Se não restam inimigos para gerar e todos os inimigos foram destruídos, termina a onda.
        if (enemiesLeftToSpawn == 0 && enemiesAlive == 0)
        {
           
            Endwave(); // Finaliza a onda atual e prepara para a próxima.
        }
    }

    private void Endwave() // Método para finalizar a onda atual.
    {
        isSpawning = false; // Para a geração de inimigos.
        timesinceLastSpawn = 0f; // Reseta o tempo desde o último inimigo gerado.
        currentwave++; // Incrementa o número da onda atual.
                       // Multiplica o modificador de saúde em vez de incrementá-lo
        currentHealthModifier *= healthIncreasePerWave;
        StartCoroutine(StartWave()); // Inicia a próxima onda após o período de espera.
    }

    private void EnemyDestroyed() // Método chamado quando um inimigo é destruído.
    {
        enemiesAlive--; // Decrementa o contador de inimigos vivos.
    }

    private IEnumerator StartWave() // Coroutine que inicia uma nova onda após um tempo de espera.
    {
        yield return new WaitForSeconds(timebetweenWaves); // Aguarda o tempo definido entre ondas.
        isSpawning = true; // Ativa a geração de inimigos.
        enemiesLeftToSpawn = EnemiesPerwave(); // Calcula o número de inimigos a serem gerados na onda atual.
      
    }

    private void SpawnEnemy() // Método para gerar um inimigo.
    {
        GameObject prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)]; // Escolhe um prefab aleatório da lista de inimigos.
        GameObject enemy = Instantiate(prefabToSpawn, LevelManager.instance.startPoint.position, Quaternion.identity); // Instancia o inimigo na posição inicial definida no LevelManager.

        // Aumenta a vida do inimigo de acordo com o modificador atual
        Health enemyHealth = enemy.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.hitPoints *= Mathf.Pow(healthIncreasePerWave, currentwave - 1);
        }
    }

    private int EnemiesPerwave() // Calcula o número de inimigos para a onda atual com base na dificuldade.
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentwave, difficultyScallingFactor)); // Aumenta o número de inimigos por onda de acordo com o fator de dificuldade.
    }
}
