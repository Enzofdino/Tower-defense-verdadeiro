using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseHealth : MonoBehaviour
{

    [SerializeField] public int maxHealth = 100; // Vida m�xima da base
    public static BaseHealth instance;

    private void Awake()
    {
        instance = this;
    }
    private int currentHealth;
    private void Start()
    {
        currentHealth = maxHealth; // Define a vida inicial da base
    }

    // M�todo que � chamado quando um inimigo atinge a base
    public void TakeDamage(int damage)
    {
        damage = 10;
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }
    

    private void GameOver()
    {
        // Chama o m�todo para exibir a tela de Game Over
        LevelManager.instance.ShowGameOverScreen();
    }
}
