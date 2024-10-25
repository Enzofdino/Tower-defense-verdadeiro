using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemymask;
    [SerializeField] private GameObject bulletprefab;
    [SerializeField] private Transform firingpoint;
    [Header("Attribute")]
    [SerializeField] private float targetingrange = 5f;
    [SerializeField] private float rotationspeed = 10f;
    [SerializeField] private float bps = 1f; //Balas por segundo

    private Transform target;
    private float timeUntilFire;
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position, transform.forward, targetingrange);
    }
    private void Update()
    {
        if (target == null)
        {
            Findtarget();
            return;
        }

        RotateTowardsTarget();

        if (!checktargetisrange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if(timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }
    private void Shoot()
    {
       GameObject bulletObj = Instantiate(bulletprefab,firingpoint.position,Quaternion.identity);
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();

        bulletScript.SetTarget(target);
    }
    private void Findtarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingrange, (Vector2)
            transform.position, 0f, enemymask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }
    private bool checktargetisrange()
    {
        
        if (target == null)
        {
            Debug.LogWarning("Nenhum alvo detectado no alcance.");
            return false; 
        }

    
        return Vector2.Distance(target.position, transform.position) <= targetingrange;
    }
    private void RotateTowardsTarget()
    {
       
        if (target == null)
        {
            Debug.LogWarning("Nenhum alvo detectado para rotacionar.");
            return;
        }

       
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - turretRotationPoint.position.x) * Mathf.Rad2Deg - 90;

        // Criar a rotação do alvo e aplicá-la ao objeto
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationspeed * Time.deltaTime);
    }


}