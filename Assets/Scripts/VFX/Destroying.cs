using System.Collections;
using UnityEngine;

public class Destroying : MonoBehaviour
{
    [SerializeField] private float _timeToDestroy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds( _timeToDestroy );
        Destroy(gameObject);    
    }
}
