using System.Collections;
using UnityEngine;

public class PlayerShotgun : MonoBehaviour
{
    [SerializeField] private ParticleSystem _shootVfx;
    [SerializeField] private Animator _shotgunAnim;
    [SerializeField] private float _delayBetweenShoots;
    [SerializeField] private float _reloadDelay;
    private bool _canShoot = true;
    private int _ammo = 2;

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && _ammo > 0 && _canShoot)
        {
            StartCoroutine(Shooting());
        }
        if(_ammo == 0 && _canShoot)
        {
            StopAllCoroutines();
            StartCoroutine(Reloading());
        }
    }
    private IEnumerator Shooting()
    {
        _canShoot = false;
        _shootVfx.Play();
        _ammo -= 1;
        _shotgunAnim.SetTrigger("Shoot");
        yield return new WaitForSeconds(_delayBetweenShoots);
        _canShoot = true;
    }
    private IEnumerator Reloading()
    {
        
        _shotgunAnim.SetTrigger("Reload");
        _canShoot = false;
        _ammo = 0;
        yield return new WaitForSeconds(_reloadDelay);
        _ammo = 2;
        _canShoot = true;
    }
}
