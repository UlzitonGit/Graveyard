using UnityEngine;

public abstract class EnemyWeapon : MonoBehaviour
{
    public bool CanShoot;
    public abstract void Shoot();
    public abstract void StopShooting();
    
}
