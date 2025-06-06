using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    // Instância estática para permitir o acesso a BuildManager de qualquer lugar
    public static BuildManager Instance;

    [Header("References")]
    [SerializeField] private Tower[] towers; // Array de torres disponíveis para construção
    private int selectedTower = 0; // Índice da torre atualmente selecionada

    private void Awake()
    {
        // Inicializa a instância singleton para que apenas uma BuildManager exista em toda a cena
        Instance = this;
    }

    // Método que retorna a torre que está atualmente selecionada
    public Tower GetselectedTower()
    {
        // Retorna a torre do array usando o índice da torre selecionada
        return towers[selectedTower];
    }

    // Método para definir qual torre deve ser a torre selecionada
    public void SetSelectedTower(int _selectedtower)
    {
        // Atribui o índice da torre selecionada à variável selectedTower
        selectedTower = _selectedtower;
    }
}
