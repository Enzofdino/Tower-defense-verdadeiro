using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour 

{
    [SerializeField] protected float hitPoints = 2;   

    protected bool isDestroyed = false;    

    public virtual void TakeDamage(float dmg)    

    {
        hitPoints -= dmg;
        if (hitPoints <= 0 && !isDestroyed)       

        {
            EnemySpawner.onEnemyDestroy.Invoke();
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}
