using System.Collections;
using UnityEngine;

public class EnemyUzi : EnemyWeapon
{
   
    public override void StopShooting()
    {
        StopAllCoroutines();
    }
    public override void StartShooting()
    {
        StartCoroutine(Shooting());
    }
    IEnumerator Shooting()
    {
        for (int i = 0; i < _ammo; i++)
        {
            Instantiate(_bullet, _shootPoint.position, _shootPoint.rotation);
            yield return new WaitForSeconds(_timeBetweenShoots);
        }
        yield return new WaitForSeconds(_reloadTime);
        StartCoroutine(Shooting());
    }
}
