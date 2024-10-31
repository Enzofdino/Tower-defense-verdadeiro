using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretHighDamage : Turret
{
    [SerializeField] private float highDamage = 10f;  // Definindo o dano alto que a torre vai causar

    public override void Atacar()
    {
        if (target != null)
        {
            Health enemyHealth = target.GetComponent<Health>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(highDamage);  // Aplica o dano alto diretamente
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