using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour, Iatacavel 

{
    [SerializeField] protected float targetingRange = 5f;    

    [SerializeField] protected LayerMask enemyMask;    

    [SerializeField] protected GameObject bulletPrefab;   

    [SerializeField] protected Transform firingPoint;    

    [SerializeField] private float bps = 1f;   

    protected Transform target;    

    protected float timeUntilFire;  



    public virtual void Atacar()   

    {
       
    }
   
    private void Update()    

    {
        if (target == null)       

        {
            FindTarget();
            return;
        }
        if (!CheckTargetIsInRange())
        {
            target = null;        

        }
        else
        {
            timeUntilFire += Time.deltaTime;           

            if (timeUntilFire >= 1f / bps)            

            {
                Shoot();
                timeUntilFire = 0f;  
            }
        }

    }
    protected virtual void Shoot()    

    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);       
      
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }
    private bool CheckTargetIsInRange()   

    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }
    private void FindTarget()    

    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

       
        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }
}
