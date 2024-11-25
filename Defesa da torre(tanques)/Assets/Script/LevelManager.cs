using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour // Classe respons�vel por gerenciar o n�vel do jogo, controlando vari�veis globais como moeda e caminhos.
{
    public static LevelManager instance;    // Inst�ncia �nica do LevelManager para acesso global.

    public Transform startPoint;    // Ponto inicial onde o jogador ou objeto come�a no n�vel.

    public Transform[] path;    // Array contendo os pontos do caminho que o jogador ou objeto pode seguir.
   [SerializeField] public int currency;    // Quantidade de moeda ou pontos que o jogador possui.

    public GameObject gameOverPanel; // Tela de Game Over no inspector
    private bool isGameOver = false; // Para evitar chamadas repetidas do Game Over

    private void Awake()    // M�todo chamado antes do Start, para inicializar a inst�ncia global.
    {
        instance = this; // Define esta inst�ncia como a inst�ncia global da classe.
    }

    private void Start() // M�todo inicial que configura o estado do jogo quando ele come�a.
    {
        currency = 500; // Define o valor inicial da moeda para o jogador em 100.
    }

    public void IncreaseCurrency(int amount) // M�todo que aumenta o valor da moeda.
    {
        
        currency += amount; // Adiciona a quantidade definida de moeda ao total atual.
    }

    public bool SpendCurrency(int amount) // M�todo que tenta reduzir a moeda se houver quantidade suficiente.
    {
        if (amount <= currency) // Verifica se o jogador tem moeda suficiente para gastar.
        {
            currency -= amount; // Subtrai a quantidade de moeda do total atual.
            return true; // Retorna verdadeiro se a transa��o foi realizada com sucesso.
        }
        else
        {
            Debug.Log("You do not have enough to purchase this item"); // Mensagem de aviso caso o jogador n�o tenha moeda suficiente.
            return false; // Retorna falso se a transa��o falhou.
        }
    }
    // M�todo para adicionar 100 moedas como recompensa
    public void RewardCurrency()
    {
        int reward = Random.Range(100, 1000);
        IncreaseCurrency(reward);
        Debug.Log($"Voc� ganhou {reward} moedas!");
    }

    public void GameOver()
    {
        if (isGameOver) return; // Evita que o Game Over seja chamado v�rias vezes

        isGameOver = true; // Marca que o jogo terminou
        Debug.Log("Game Over! Um inimigo alcan�ou o ponto final.");

        // Exibe o painel de Game Over
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
        if (!AdManager.instance.isGamePausedByAd)
        {
            Time.timeScale = 0; // Apenas pausa o jogo se n�o estiver pausado por um an�ncio
        }

    }
    public void Reiniciar()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1;
        isGameOver = false;

    }
}

