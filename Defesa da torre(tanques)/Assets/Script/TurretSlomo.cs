using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class TurretSlomo : Turret 

{

    [SerializeField] private float aps = 4f;   

    [SerializeField] private float FreezeTime = 1f;
    



    private void Update()   

    {
        RotateTowardsTarget();
        timeUntilFire += Time.deltaTime;        

        if (timeUntilFire >= 1f / aps)        

        {
            FreezeEnemies(); 
            timeUntilFire = 0f; 
        }

    }
    private void FreezeEnemies() 
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingrange, (Vector2)transform.position, 0f, enemyMask);         

        if (hits.Length > 0)       

        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>();               

                em.UpdateSpeed(0.5f);               

                StartCoroutine(ResetEnemySpeed(em));             

            }
        }
    }
    private IEnumerator ResetEnemySpeed(EnemyMovement em)     

    {
        yield return new WaitForSeconds(FreezeTime);       

        em.ResetSpeed();       

    }
    private void RotateTowardsTarget()
    {

        if (target == null)
        {
            //Debug.LogWarning("Nenhum alvo detectado para rotacionar.");
            return;
        }


        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - turretRotationPoint.position.x) * Mathf.Rad2Deg - 90;


        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationspeed * Time.deltaTime);
    }
}
