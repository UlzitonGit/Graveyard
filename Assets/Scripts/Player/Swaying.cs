using UnityEngine;

public class Swaying : MonoBehaviour
{
    [Header("Sway Settings")]
    [SerializeField] private float _smooth;
    [SerializeField] private float _intensity;
    
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * _intensity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _intensity;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, _smooth * Time.deltaTime);
    }
}