using UnityEngine;

public abstract class EnemyWeapon : MonoBehaviour
{
    [SerializeField] protected GameObject _bullet;
    [SerializeField] protected Transform _shootPoint;
    [SerializeField] protected float _timeBetweenShoots;
    [SerializeField] protected float _ammo;
    [SerializeField] protected float _reloadTime;
    [SerializeField] protected float _recoilPower;
    public abstract void StartShooting();
    public abstract void StopShooting();
    
}
