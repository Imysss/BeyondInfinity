using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] 
    private float _moveSpeed = 5f;
    private float _jumpPower = 100f;
    private float _extraJumpPower;
    public LayerMask groundLayerMask;
    
    private Rigidbody _rigid;
    private Vector2 _moveDirection;
    
    [Header("Jump")]
    private bool _isDoubleJumpEnabled;
    private int _doubleJumpCount;

    [Header("Look")] 
    public Transform cameraContainer;
    private float _minXLook = -85f;
    private float _maxXLook = 85f;
    private float _lookSensitivity = 0.1f;

    private float _currentCameraXRotation;
    private Vector2 _mouseDelta;
    private bool canLook = true;
    private CameraSwitcher _cameraSwitcher;
    
    public Action OnInventoryChanged;

    #region Unity Methods
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _cameraSwitcher = GetComponentInChildren<CameraSwitcher>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            RotateCamera();
        }
    }
    #endregion

    #region Movement & Look
    private void HandleMovement()
    {
        Vector3 dir = transform.forward * _moveDirection.y + transform.right * _moveDirection.x;
        dir *= _moveSpeed;
        dir.y = _rigid.velocity.y;
        
        _rigid.velocity = dir;
    }

    private void RotateCamera()
    {
        _currentCameraXRotation += _mouseDelta.y * _lookSensitivity;
        _currentCameraXRotation = Mathf.Clamp(_currentCameraXRotation, _minXLook, _maxXLook);
        
        cameraContainer.localEulerAngles = new Vector3(-_currentCameraXRotation, 0, 0);
        transform.eulerAngles += new Vector3(0, _mouseDelta.x * _lookSensitivity, 0);
    }
    #endregion

    #region Player Input
    private void OnMove(InputValue inputValue)
    {
        _moveDirection = inputValue.Get<Vector2>();
    }

    private void OnLook(InputValue inputValue)
    {
        _mouseDelta = inputValue.Get<Vector2>();
    }

    private void OnJump(InputValue inputValue)
    {
        if (IsGrounded())
        {
            if (PlayerManager.Instance.Player.condition.SubtractStamina(20f))
            {
                _doubleJumpCount = 1;
                Jump();
            }
        }
        else if (_isDoubleJumpEnabled && _doubleJumpCount == 1)
        {
            if (PlayerManager.Instance.Player.condition.SubtractStamina(20f))
            {
                _doubleJumpCount = 0;
                Jump();
            }
        }
    }

    private void OnInventory(InputValue inputValue)
    {
        OnInventoryChanged?.Invoke();
        ToggleCursor();
    }

    private void OnSwitchView(InputValue inputValue)
    {
        _cameraSwitcher?.SwitchView();
    }
    #endregion

    #region Jump & Ground Check
    public void Jump(float jumpPadPower = 0f)
    {
        float finalJumpPower = _jumpPower + _extraJumpPower + jumpPadPower;
        _rigid.AddForce(Vector3.up * finalJumpPower, ForceMode.Impulse);   
    }

    private bool IsGrounded()
    {
        Vector3[] offsets =
        {
            transform.forward * 0.2f,
            -transform.forward * 0.2f,
            transform.right * 0.2f,
            -transform.right * 0.2f,
        };

        foreach (Vector3 offset in offsets)
        {
            Ray ray = new Ray(transform.position + offset + Vector3.up * 0.01f, Vector3.down);
            if (Physics.Raycast(ray, 0.2f, groundLayerMask))
                return true;
        }

        return false;
    }
    #endregion
    
    private void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    #region Power-Up
    public void AddSpeed(float amount)
    {
        StartCoroutine(TemporaryModifier(() => _moveSpeed += amount, () => _moveSpeed -= amount, 10f));
    }

    public void AddJumpPower(float amount)
    {
        StartCoroutine(TemporaryModifier(() => _jumpPower += amount, () => _jumpPower -= amount, 10f));
    }

    public void DoubleJump()
    {
        StartCoroutine(TemporaryModifier(() => _isDoubleJumpEnabled = true, () => _isDoubleJumpEnabled = false, 10f));
    }

    private IEnumerator TemporaryModifier(Action apply, Action revert, float duration)
    {
        apply();
        yield return new WaitForSeconds(duration);
        revert();
    }
    #endregion
}
