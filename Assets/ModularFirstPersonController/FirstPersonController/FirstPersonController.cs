// CHANGE LOG
// 
// CHANGES || version VERSION
//
// "Enable/Disable Headbob, Changed look rotations - should result in reduced camera jitters" || version 1.0.1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
    using UnityEditor;
    using System.Net;
#endif

public class FirstPersonController : MonoBehaviour
{
    private Rigidbody _rb;

    #region Camera Movement Variables

    [SerializeField] private Camera _playerCamera;

    [SerializeField] private float _fov = 60f;
    [SerializeField] private bool _invertCamera = false;
    [SerializeField] private bool _cameraCanMove = true;
    [SerializeField] private float _mouseSensitivity = 2f;
    [SerializeField] private float _maxLookAngle = 50f;

    // Crosshair
    [SerializeField] private bool _lockCursor = true;



    // Internal Variables
    [SerializeField] private float _yaw = 0.0f;
    [SerializeField] private float _pitch = 0.0f;


  
    #endregion

    #region Movement Variables

    [SerializeField] private bool _playerCanMove = true;
    [SerializeField] private float _walkSpeed = 5f;
    [SerializeField] private float _maxVelocityChange = 10f;

    // Internal Variables
    private bool _isWalking = false;

    #region Sprint


    #endregion

    #region Jump

    [SerializeField] private bool _enableJump = true;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private float _jumpPower = 5f;

    // Internal Variables
    private bool isGrounded = false;

    #endregion

    #region Crouch

    [SerializeField] private bool _enableCrouch = true;
    [SerializeField] private bool _holdToCrouch = true;
    [SerializeField] private KeyCode _crouchKey = KeyCode.LeftControl;
    [SerializeField] private float _crouchHeight = .75f;
    [SerializeField] private float _speedReduction = .5f;

    // Internal Variables
    private bool _isCrouched = false;
    private Vector3 _originalScale;

    #endregion
    #endregion

    #region Head Bob

    [SerializeField] private bool _enableHeadBob = true;
    [SerializeField] private Transform _joint;
    [SerializeField] private float _bobSpeed = 10f;
    [SerializeField] private Vector3 _bobAmount = new Vector3(.15f, .05f, 0f);

    // Internal Variables
    private Vector3 _jointOriginalPos;
    private float _timer = 0;

    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();

     

        // Set internal variables
        _playerCamera.fieldOfView = _fov;
        _originalScale = transform.localScale;
        _jointOriginalPos = _joint.localPosition;


    }

    void Start()
    {
        if(_lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }       
    }

    float camRotation;

    private void Update()
    {
        #region Camera

        // Control camera movement
        if(_cameraCanMove)
        {
            _yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * _mouseSensitivity;

            if (!_invertCamera)
            {
                _pitch -= _mouseSensitivity * Input.GetAxis("Mouse Y");
            }
            else
            {
                // Inverted Y
                _pitch += _mouseSensitivity * Input.GetAxis("Mouse Y");
            }

            // Clamp pitch between lookAngle
            _pitch = Mathf.Clamp(_pitch, -_maxLookAngle, _maxLookAngle);

            transform.localEulerAngles = new Vector3(0, _yaw, 0);
            _playerCamera.transform.localEulerAngles = new Vector3(_pitch, 0, 0);
        }

      
        #endregion

        #region Jump

        // Gets input and calls jump method
        if(_enableJump && Input.GetKeyDown(_jumpKey) && isGrounded)
        {
            Jump();
        }

        #endregion

        #region Crouch

        if (_enableCrouch)
        {
            if(Input.GetKeyDown(_crouchKey) && !_holdToCrouch)
            {
                Crouch();
            }
            
            if(Input.GetKeyDown(_crouchKey) && _holdToCrouch)
            {
                _isCrouched = false;
                Crouch();
            }
            else if(Input.GetKeyUp(_crouchKey) && _holdToCrouch)
            {
                _isCrouched = true;
                Crouch();
            }
        }

        #endregion

        CheckGround();

        if(_enableHeadBob)
        {
            HeadBob();
        }
    }

    void FixedUpdate()
    {
        #region Movement

        if (_playerCanMove)
        {
            // Calculate how fast we should be moving
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            // Checks if player is walking and isGrounded
            // Will allow head bob
            if (targetVelocity.x != 0 || targetVelocity.z != 0 && isGrounded)
            {
                _isWalking = true;
            }
            else
            {
                _isWalking = false;
            }

           
            targetVelocity = transform.TransformDirection(targetVelocity) * _walkSpeed;

            // Apply a force that attempts to reach our target velocity
            Vector3 velocity = _rb.linearVelocity;
            Vector3 velocityChange = (targetVelocity - velocity);
            velocityChange.x = Mathf.Clamp(velocityChange.x, -_maxVelocityChange, _maxVelocityChange);
            velocityChange.z = Mathf.Clamp(velocityChange.z, -_maxVelocityChange, _maxVelocityChange);
            velocityChange.y = 0;
          
            _rb.AddForce(velocityChange, ForceMode.VelocityChange);        
        }

        #endregion
    }

    // Sets isGrounded based on a raycast sent straigth down from the player object
    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .75f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void Jump()
    {
        // Adds force to the player rigidbody to jump
        if (isGrounded)
        {
            _rb.AddForce(0f, _jumpPower, 0f, ForceMode.Impulse);
            isGrounded = false;
        }

        // When crouched and using toggle system, will uncrouch for a jump
        if(_isCrouched && !_holdToCrouch)
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        // Stands player up to full height
        // Brings walkSpeed back up to original speed
        if(_isCrouched)
        {
            transform.localScale = new Vector3(_originalScale.x, _originalScale.y, _originalScale.z);
            _walkSpeed /= _speedReduction;

            _isCrouched = false;
        }
        // Crouches player down to set height
        // Reduces walkSpeed
        else
        {
            transform.localScale = new Vector3(_originalScale.x, _crouchHeight, _originalScale.z);
            _walkSpeed *= _speedReduction;

            _isCrouched = true;
        }
    }

    private void HeadBob()
    {
        if(_isWalking)
        {
            // Calculates HeadBob speed during sprin
            // Calculates HeadBob speed during crouched movement
            if (_isCrouched)
            {
                _timer += Time.deltaTime * (_bobSpeed * _speedReduction);
            }
            // Calculates HeadBob speed during walking
            else
            {
                _timer += Time.deltaTime * _bobSpeed;
            }
            // Applies HeadBob movement
            _joint.localPosition = new Vector3(_jointOriginalPos.x + Mathf.Sin(_timer) * _bobAmount.x, _jointOriginalPos.y + Mathf.Sin(_timer) * _bobAmount.y, _jointOriginalPos.z + Mathf.Sin(_timer) * _bobAmount.z);
        }
        else
        {
            // Resets when play stops moving
            _timer = 0;
            _joint.localPosition = new Vector3(Mathf.Lerp(_joint.localPosition.x, _jointOriginalPos.x, Time.deltaTime * _bobSpeed), Mathf.Lerp(_joint.localPosition.y, _jointOriginalPos.y, Time.deltaTime * _bobSpeed), Mathf.Lerp(_joint.localPosition.z, _jointOriginalPos.z, Time.deltaTime * _bobSpeed));
        }
    }
}



