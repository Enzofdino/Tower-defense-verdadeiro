using System;
using UnityEngine;

// Classe que representa uma torre no jogo
[Serializable] // Permite que a classe seja serializada, útil para armazenar dados em arquivos ou no editor
public class Tower
{
    public string name;      // Nome da torre
    public int cost;         // Custo de construção da torre
    public GameObject prefab; // Prefab da torre (modelo 3D ou 2D)

    // Construtor para inicializar a torre com valores específicos
    public Tower(string _name, int _cost, GameObject _prefab)
    {
        name = _name;        // Inicializa o nome da torre
        cost = _cost;       // Inicializa o custo da torre
        prefab = _prefab;   // Inicializa o prefab da torre
    }
}
