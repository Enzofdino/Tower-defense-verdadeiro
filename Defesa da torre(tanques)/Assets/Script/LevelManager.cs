using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour // Classe LevelManager: Gerencia informações relacionadas ao nível, como pontos de início e caminho dos inimigos.

{
    public static LevelManager instance;     // Instância singleton da classe LevelManager.

    public Transform startPoint;    // Ponto de início para onde os inimigos devem ser gerados.

    public Transform[] path;    // Array de pontos que definem o caminho que os inimigos devem seguir.

    private void Awake()    // Método chamado quando o objeto é inicializado.

    {
        instance = this; // Inicializa a instância singleton.
    }
}
