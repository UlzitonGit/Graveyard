using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    [SerializeField] private float _distance = 50f;
    [SerializeField] private EnemyWeapon _weapon;
    private FirstPersonController _player;
    private bool _isRaged = false;
    private bool _isActive = true;
    private bool _chasing = false;
    void Start()
    {
        _player = FindAnyObjectByType<FirstPersonController>();
    }

    
    void Update()
    {
        if (!_isActive) return; 
        _isRaged = Vector3.Distance(transform.position, _player.transform.position) < _distance;
        if (_isRaged)
        {
            transform.LookAt(_player.transform.position);
            //transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z);
            if(!_chasing) _weapon.StartShooting();
            _chasing = true;
        }
        else if(!_isRaged && _chasing) 
        {
            _chasing = false;
            _weapon.StopShooting();
        }
    }
    public void Death()
    {
        GetComponent<BoxCollider>().enabled = false;
        _isActive = false;
        _isRaged = false;
        _weapon.StopShooting();
    }
}
