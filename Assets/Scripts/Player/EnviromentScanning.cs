using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;

public class EnviromentScanning : MonoBehaviour
{
    [SerializeField] private MeshRenderer[] _cables;
    [SerializeField] private Material _scaningMaterial;
    [SerializeField] private Material _regularMaterial;
    [SerializeField] private Volume _scanVolume;
    [SerializeField] private ScanZone[] _scanZones;
    private bool _isScanning;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            _isScanning = !_isScanning;
            if (_isScanning)
            {
                DOTween.To(DGtweenUpdate, 0, 1, 0.3f).SetEase(Ease.Linear);
               
                for (int i = 0; i < _cables.Length; i++)
                {
                    _cables[i].material = _scaningMaterial;
                }
                for (int i = 0; i < _scanZones.Length; i++)
                {
                    _scanZones[i].IsScanning = _isScanning;
                }
            }
            else
            {
                DOTween.To(DGtweenUpdate, 1, 0, 0.3f).SetEase(Ease.Linear);
                _scanVolume.weight = 0;
                for (int i = 0; i < _cables.Length; i++)
                {
                    _cables[i].material = _regularMaterial;
                }
                for (int i = 0; i < _scanZones.Length; i++)
                {
                    _scanZones[i].IsScanning = _isScanning;
                }
            }
        }
    }
    void DGtweenUpdate(float deltaTime)
    {
        _scanVolume.weight = deltaTime;
    }

}
