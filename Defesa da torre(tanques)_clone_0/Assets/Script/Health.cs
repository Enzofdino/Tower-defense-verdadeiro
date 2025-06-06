using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public static Health instance;
  

    private void Awake()
    {
        instance = this;
    }

    [SerializeField] public float hitPoints = 10; // Pontos de vida do inimigo
    [SerializeField] private int currencyWorth = 50; // Quantidade de moeda que o inimigo gera ao ser destru�do

    protected bool isDestroyed = false; // Flag para verificar se o inimigo j� foi destru�do

    // M�todo para aplicar dano ao inimigo
    public virtual void TakeDamage(float dmg)
    {
        hitPoints -= dmg; // Reduz os pontos de vida pelo dano recebido
        if (hitPoints <= 0 && !isDestroyed)
        {
          

            EnemySpawner.onEnemyDestroy.Invoke(); // Notifica que um inimigo foi destru�do
          
            LevelManager.instance.IncreaseCurrency(currencyWorth); // Aumenta a moeda do jogador
            isDestroyed = true; // Marca o inimigo como destru�do
           
            Destroy(gameObject); // Remove o inimigo da cena
        }
    }
  

}
