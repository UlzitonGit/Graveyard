using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerShotgun : MonoBehaviour
{
    [SerializeField] private ParticleSystem _shootVfx;
    [SerializeField] private Animator _shotgunAnim;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _recoil = 10;
    [SerializeField] private float _bulletsCount = 15;
    [SerializeField] private float _delayBetweenShoots;
    [SerializeField] private float _reloadDelay;
    [SerializeField] private float _damage;
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private GameObject _hitVfx;
    [SerializeField] private GameObject _BloodVfx;
    [SerializeField] private Animator[] _uiAmmo;
    private bool _canShoot = true;
    private int _ammo = 2;
    private float _rayDistance = 20;

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
        _uiAmmo[_ammo].SetTrigger("Shoot");
        _ammoText.text = "x" + _ammo.ToString();
        _shotgunAnim.SetTrigger("Shoot");
        for (int i = 0; i < _bulletsCount; i++)
        {
            RaycastHit hit;
            Vector3 _dir = (_shootPoint.forward + new Vector3(Random.Range(_recoil * -1, _recoil), Random.Range(_recoil * -1, _recoil), Random.Range(_recoil * -1, _recoil))) * _rayDistance;
            if (Physics.Raycast(_shootPoint.position, _dir, out hit, _rayDistance))
            {
                Debug.DrawRay(_shootPoint.position, _dir, Color.green, 1);
                if (hit.transform.GetComponent<EnemyHealth>())
                {
                    Debug.Log("damage");
                    hit.transform.GetComponent<EnemyHealth>().GetDamage(_damage);
                    Instantiate(_BloodVfx, hit.point, Quaternion.LookRotation(hit.normal));
                }
                else
                {
                    Instantiate(_hitVfx, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }      
        yield return new WaitForSeconds(_delayBetweenShoots);
        _canShoot = true;
    }
    private IEnumerator Reloading()
    {
        
        _shotgunAnim.SetTrigger("Reload");
        _canShoot = false;
        _ammo = 0;
        _ammoText.text = "x" + _ammo.ToString();
        yield return new WaitForSeconds(_reloadDelay * 0.8f);
        _uiAmmo[0].SetTrigger("Return");
        yield return new WaitForSeconds(_reloadDelay * 0.1f);
        _uiAmmo[1].SetTrigger("Return");
        yield return new WaitForSeconds(_reloadDelay * 0.1f);
        _ammo = 2;
        _ammoText.text = "x" + _ammo.ToString();
        _canShoot = true;
    }
}
