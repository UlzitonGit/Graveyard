using UnityEngine;

public class WinLoose : MonoBehaviour
{
    [SerializeField] private GameObject _winPanel;
    private int _enemyCount;
    private void Start()
    {
        _enemyCount = FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None).Length;
    }
    public void MinusEnemy()
    {
        _enemyCount--;
        if (_enemyCount <= 0) Win();
    }
    public void Win()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        _winPanel.SetActive(true);
    }
}
