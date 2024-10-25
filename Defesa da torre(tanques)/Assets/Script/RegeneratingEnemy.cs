using System.Collections;
using UnityEngine;

public class RegeneratingEnemy : Health

{
    [SerializeField] private float regenerationRate = 1f;
    [SerializeField] private float maxHitPoints = 5f;

    private void Start()

    {
        hitPoints = maxHitPoints;
        StartCoroutine(RegenerateHealth());
    }

    private IEnumerator RegenerateHealth()

    {
        while (!isDestroyed)

        {
            if (hitPoints < maxHitPoints)

            {
                hitPoints += regenerationRate * Time.deltaTime;
                hitPoints = Mathf.Min(hitPoints, maxHitPoints);
            }
            yield return null;
        }
    }

    public override void TakeDamage(float dmg)
    {
        base.TakeDamage(dmg);
    }
}
