using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField] private float _distance = 50f;
    [SerializeField] private EnemyWeapon _weapon;
    private FirstPersonController _player;
    private bool _isRaged = false;
    void Start()
    {
        _player = FindAnyObjectByType<FirstPersonController>();
    }

    
    void Update()
    {
        _isRaged = Vector3.Distance(transform.position, _player.transform.position) < _distance;
        if (_isRaged) _weapon.CanShoot = true;
    }
}
