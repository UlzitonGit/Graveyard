using UnityEngine;

public class ScanZone : MonoBehaviour
{
    [HideInInspector] public bool IsScanning { private get; set; }
    [SerializeField] private MeshRenderer[] _cables;
    [SerializeField] private Material _completeMaterial;
    [SerializeField] private Light[] _light;
    [SerializeField] private Animator _door;
    private bool _scanned;
    private void OnTriggerStay(Collider other)
    {
        if (IsScanning && other.CompareTag("Player") && !_scanned)
        {
            _door.SetTrigger("Open");
            for (int i = 0; i < _light.Length; i++)
            {
                _light[i].color = Color.green;
            }
            for (int i = 0; i < _cables.Length; i++)
            {
                _cables[i].material = _completeMaterial;
            }
            _scanned = true;
        }
    }
}
