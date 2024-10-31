using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour // Classe que controla o menu do jogo, incluindo a exibição de informações como o valor atual de moedas.
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI; // Referência ao componente TextMeshProUGUI para mostrar o valor da moeda.
    [SerializeField] Animator animator; // Referência ao componente Animator para controlar as animações do menu.

    private bool ismenuOpen = true; // Variável booleana que indica se o menu está aberto ou fechado.

    public void ToggleMenu() // Método para alternar o estado do menu (aberto/fechado).
    {
        ismenuOpen = !ismenuOpen; // Inverte o estado atual do menu.
        animator.SetBool("MenuOpen", ismenuOpen); // Define a variável "MenuOpen" do Animator de acordo com o novo estado.
    }

    private void OnGUI() // Método chamado para atualizar a interface gráfica do usuário.
    {
        currencyUI.text = LevelManager.instance.currency.ToString(); // Atualiza o texto da UI com o valor atual de moedas do LevelManager.
    }

    public void SetTower() // Método reservado para definir uma torre (implementação futura).
    {
        // Método a ser implementado.
    }
}
