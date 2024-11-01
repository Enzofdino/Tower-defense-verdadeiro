using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour // Classe responsável por gerenciar o nível do jogo, controlando variáveis globais como moeda e caminhos.
{
    public static LevelManager instance;    // Instância única do LevelManager para acesso global.

    public Transform startPoint;    // Ponto inicial onde o jogador ou objeto começa no nível.

    public Transform[] path;    // Array contendo os pontos do caminho que o jogador ou objeto pode seguir.
   [SerializeField] public int currency;    // Quantidade de moeda ou pontos que o jogador possui.

    private void Awake()    // Método chamado antes do Start, para inicializar a instância global.
    {
        instance = this; // Define esta instância como a instância global da classe.
    }

    private void Start() // Método inicial que configura o estado do jogo quando ele começa.
    {
        currency = 500; // Define o valor inicial da moeda para o jogador em 100.
    }

    public void IncreaseCurrency(int amount) // Método que aumenta o valor da moeda.
    {
        amount = 50; // Define o valor da moeda a ser adicionado como 50.
        currency += amount; // Adiciona a quantidade definida de moeda ao total atual.
    }

    public bool SpendCurrency(int amount) // Método que tenta reduzir a moeda se houver quantidade suficiente.
    {
        if (amount <= currency) // Verifica se o jogador tem moeda suficiente para gastar.
        {
            currency -= amount; // Subtrai a quantidade de moeda do total atual.
            return true; // Retorna verdadeiro se a transação foi realizada com sucesso.
        }
        else
        {
            Debug.Log("You do not have enough to purchase this item"); // Mensagem de aviso caso o jogador não tenha moeda suficiente.
            return false; // Retorna falso se a transação falhou.
        }
    }
}
