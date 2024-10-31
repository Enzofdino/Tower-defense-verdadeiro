using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// Classe que representa uma torre que causa alto dano
public class TurretHighDamage : Turret
{
    [SerializeField] private float highDamage = 10f;  // Dano alto que a torre vai causar

    public override void Atacar()
    {
        if (target != null)  // Verifica se há um alvo
        {
            Health enemyHealth = target.GetComponent<Health>();  // Obtém o componente de saúde do inimigo

            if (enemyHealth != null)  // Verifica se o inimigo tem um componente de saúde
            {
                enemyHealth.TakeDamage(highDamage);  // Aplica o dano alto
            }
        }
    }
}

/*protected override void Shoot()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);

        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);

        Atacar();  // Chama o método Atacar para aplicar o dano alto
    }
}*/