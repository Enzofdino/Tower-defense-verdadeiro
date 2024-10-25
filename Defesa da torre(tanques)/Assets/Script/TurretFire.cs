using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretFire : Turret

{
    [SerializeField] private float burnDuration = 3f;       

    [SerializeField] private float burnDamagePerSecond = 1f;    

    public override void Atacar()    

    {
        if (target != null)        

        {

            Health enemyHealth = target.GetComponent<Health>();            

            if (enemyHealth != null)            

            {

                StartCoroutine(ApplyBurnDamage(enemyHealth));
            }
        }
    }
    private IEnumerator ApplyBurnDamage(Health enemyHealth)     

    {
        float elapsedTime = 0f; 

        while (elapsedTime < burnDuration) 
        {
            enemyHealth.TakeDamage(burnDamagePerSecond * Time.deltaTime);  
            elapsedTime += Time.deltaTime; 
            yield return null;   
        }
    }


    protected override void Shoot()    

    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);        

        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);

        Atacar();       

    }
}

