using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour // Classe que representa uma área onde torres podem ser construídas no jogo.
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr; // Referência ao SpriteRenderer para alterar a cor quando o mouse passa sobre o objeto.
    [SerializeField] private Color hoverColor; // Cor a ser exibida quando o mouse está sobre o objeto.

    private GameObject tower; // Armazena a torre construída neste plot.
    private Color startColor; // Armazena a cor original do plot antes da mudança ao passar o mouse.

    private void Start() // Inicializa o script.
    {
        startColor = sr.color; // Define a cor inicial com base na cor atual do SpriteRenderer.
    }

    private void OnMouseDown() // Método chamado quando o jogador clica no plot.
    {
        if (tower != null) return; // Impede construção de uma nova torre se já houver uma no plot.

        Tower towertobuild = BuildManager.Instance.GetselectedTower(); // Obtém a torre selecionada para construir.
        if (towertobuild.cost > LevelManager.instance.currency) // Verifica se o jogador possui moedas suficientes para construir.
        {
            Debug.Log("you cant afford this"); // Exibe uma mensagem se o jogador não tiver moedas suficientes.
            return;
        }

        LevelManager.instance.SpendCurrency(towertobuild.cost); // Deduz o custo da torre da moeda do jogador.
        tower = Instantiate(towertobuild.prefab, transform.position, Quaternion.identity); // Instancia a torre no plot.
    }

    private void OnMouseEnter() // Método chamado quando o mouse passa sobre o plot.
    {
        sr.color = hoverColor; // Muda a cor do plot para a cor de hover.
    }

    private void OnMouseExit() // Método chamado quando o mouse sai de cima do plot.
    {
        sr.color = startColor; // Restaura a cor original do plot.
    }
}
