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
    private float _jumpPower = 80f;
    private float _extraJumpPower;
    private Vector2 _moveDirection;
    public LayerMask groundLayerMask;

    [Header("Look")] 
    public Transform cameraContainer;
    private float _minXLook = -85f;
    private float _maxXLook = 85f;
    private float _currentCameraXRotation;
    private float _lookSensitivity = 0.1f;
    private Vector2 _mouseDelta;
    private bool canLook = true;

    public Action OnInventoryChanged;

    private Rigidbody _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            RotateCamera();
        }
    }

    private void Move()
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
        if (IsGrounded() && PlayerManager.Instance.Player.condition.SubtractStamina(20f))
        {
            Jump(0f);
        }
    }

    private void OnInventory(InputValue inputValue)
    {
        OnInventoryChanged?.Invoke();
        ToggleCursor();
    }
    #endregion

    public void Jump(float jumpPadPower)
    {
        float finalJumpPower = _jumpPower + _extraJumpPower + jumpPadPower;
        _rigid.AddForce(Vector3.up * finalJumpPower, ForceMode.Impulse);   
    }

    private bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.2f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    private void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    public void AddSpeed(float amount)
    {
        StartCoroutine(SetSpeed(amount));
    }

    private IEnumerator SetSpeed(float amount)
    {
        _moveSpeed += amount;
        yield return new WaitForSeconds(10.0f);
        _moveSpeed -= amount;
    }

    public void AddJumpPower(float amount)
    {
        StartCoroutine(SetJumpPower(amount));
    }

    private IEnumerator SetJumpPower(float amount)
    {
        _jumpPower += amount;
        yield return new WaitForSeconds(10.0f);
        _jumpPower -= amount;
    }
}
