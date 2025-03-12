using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _health = 100;
    [SerializeField] private Animator _anim;
    [SerializeField] private EnemyBrain _enemy;
    private WinLoose _winMananger;
    private float _deathTime = 10;
    private bool _dead;
    private void Start()
    {
        _winMananger = FindAnyObjectByType<WinLoose>();
    }
    public void GetDamage(float _damage)
    {
        _health -= _damage;
        if(_health < 0 && !_dead)
        {
            _enemy.Death();
            StartCoroutine(Dying());
        }
    }
    IEnumerator Dying()
    {
        _winMananger.MinusEnemy();
        _dead = true;
        _anim.SetTrigger("Death");
        yield return new WaitForSeconds(10);
        Destroy(gameObject);
    }
}
