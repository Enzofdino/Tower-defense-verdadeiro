using System.Collections;
using UnityEngine;

// Classe que representa um inimigo que regenera saúde ao longo do tempo
public class RegeneratingEnemy : Health
{
    [SerializeField] private float regenerationRate = 1f; // Taxa de regeneração de saúde por segundo
    [SerializeField] private float maxHitPoints = 5f; // Pontos de vida máximos do inimigo

    private void Start()
    {
        hitPoints = maxHitPoints; // Inicializa os pontos de vida com o valor máximo
        StartCoroutine(RegenerateHealth()); // Inicia a coroutine para regeneração de saúde
    }

    private IEnumerator RegenerateHealth()
    {
        while (!isDestroyed) // Continua enquanto o inimigo não foi destruído
        {
            if (hitPoints < maxHitPoints) // Verifica se os pontos de vida estão abaixo do máximo
            {
                // Regenera saúde de acordo com a taxa e o tempo passado
                hitPoints += regenerationRate * Time.deltaTime;
                hitPoints = Mathf.Min(hitPoints, maxHitPoints); // Garante que os pontos de vida não ultrapassem o máximo
            }
            yield return null; // Espera o próximo frame antes de continuar
        }
    }

    // Método que recebe dano e o aplica à saúde do inimigo
    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg); // Chama o método base para aplicar dano
    }
}
