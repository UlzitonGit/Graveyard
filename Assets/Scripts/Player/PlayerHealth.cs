using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Image _hpBar;
    [SerializeField] private GameObject _deathPanel;
    [SerializeField] private Volume _volume;
    private float healthIncrease = 4;
    private float _health = 100;
    private float _maxHealth = 100;
    void Update()
    {
        if(_health < 100)
        {
            _volume.weight = 1 - _health / _maxHealth;
            _health += Time.deltaTime * healthIncrease;
            _hpBar.fillAmount = _health / _maxHealth;
        }
    }
    public void GetDamage(float _damage)
    {
        _hpBar.fillAmount = _health / _maxHealth;
        _health -= _damage;
        if(_health <= 0)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _deathPanel.SetActive(true);
        }
    }
}
