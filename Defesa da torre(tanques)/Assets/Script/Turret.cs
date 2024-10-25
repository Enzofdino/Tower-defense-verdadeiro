using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Turret : MonoBehaviour, Iatacavel

{
    [SerializeField] public Transform turretRotationPoint;

    [SerializeField] protected float targetingrange = 5f;

    [SerializeField] protected LayerMask enemyMask;
    [SerializeField] protected LayerMask enemymaskinvisible;

    [SerializeField] protected GameObject bulletPrefab;

    [SerializeField] protected Transform firingPoint;

    [SerializeField] public float rotationspeed = 10f;

    [SerializeField] private float bps = 1f;

    protected Transform target;

    protected float timeUntilFire;



    public virtual void Atacar()

    {

    }
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

            if (timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

        private void RotateTowardsTarget()
    {

        if (target == null)
        {
            Debug.LogWarning("Nenhum alvo detectado para rotacionar.");
            return;
        }


        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - turretRotationPoint.position.x) * Mathf.Rad2Deg - 90;

       
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationspeed * Time.deltaTime);
    }

    protected virtual void Shoot()

    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);

        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        bulletScript.SetTarget(target);
    }
    private bool checktargetisrange()

    {
        return Vector2.Distance(target.position, transform.position) <= targetingrange;
    }
    private void Findtarget()

    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingrange, (Vector2)transform.position, 0f, enemyMask);


        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }
   
}
