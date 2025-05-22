using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")] 
    private Define.MovementState movementState;
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
    
    [Header("Climb")]
    private float wallCheckDistance = 0.5f;
    public LayerMask wallLayerMask;
    private Vector3 climbNormal;
    
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
        movementState =  Define.MovementState.Normal;
    }

    private void FixedUpdate()
    {
        switch (movementState)
        {
            case Define.MovementState.Normal:
                HandleMovement();
                break;
            case Define.MovementState.Climbing:
                HandleClimbMovement();
                break;
            case Define.MovementState.LeavingWall:
                HandleLeavingWallMovement();
                break;
        }
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
        ApplyHorizontalMovement();

        if (IsWallFront())
        {
            TransitionToClimb();
        }
    }
    
    private void HandleClimbMovement()
    {
        if (!IsWallFront() && !IsGrounded())
        {
            TransitionToLeavingWall();
            return;
        }

        Vector3 climbDir = GetClimbDirection(_moveDirection);
        _rigid.velocity = climbDir * _moveSpeed;
        
        if (IsGrounded() && _moveDirection.y < 0f) 
        {
            TransitionToNormal();
        }
    }

    private void HandleLeavingWallMovement()
    {
        Vector3 movement = Vector3.zero;

        if (Mathf.Abs(_moveDirection.y) > 0.1f)
        {
            Vector3 verticalMove = Vector3.up * _moveDirection.y;
            if (CanMoveInDirection(verticalMove))
            {
                movement += verticalMove * _moveSpeed;
            }
        }

        Vector3 sideDir = Vector3.Cross(climbNormal, Vector3.up).normalized;
        if (Mathf.Abs(_moveDirection.x) > 0.1f)
        {
            Vector3 horizontalMove = sideDir * _moveDirection.x;
            if (CanMoveInDirection(horizontalMove))
            {
                movement += horizontalMove * _moveSpeed;
            }
        }
        
        _rigid.velocity = movement;

        if (IsWallFront())
        {
            TransitionToClimb();
        }
        else if (IsGrounded())
        {
            TransitionToNormal();
        }
    }

    private void ApplyHorizontalMovement()
    {
        Vector3 inputDir = transform.forward * _moveDirection.y + transform.right * _moveDirection.x;
        Vector3 inputVelocity = inputDir * _moveSpeed;
        _rigid.velocity = new Vector3(inputVelocity.x, _rigid.velocity.y, inputVelocity.z);
    }

    private void RotateCamera()
    {
        _currentCameraXRotation += _mouseDelta.y * _lookSensitivity;
        _currentCameraXRotation = Mathf.Clamp(_currentCameraXRotation, _minXLook, _maxXLook);
        
        cameraContainer.localEulerAngles = new Vector3(-_currentCameraXRotation, 0, 0);
        transform.eulerAngles += new Vector3(0, _mouseDelta.x * _lookSensitivity, 0);
    }
    #endregion

    #region Climb Methods
    private bool IsWallFront()
    {
        if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out RaycastHit hit, wallCheckDistance,
                wallLayerMask))
        {
            climbNormal = hit.normal;
            return true;
        }

        return false;
    }

    private bool CanMoveInDirection(Vector3 direction)
    {
        Vector3 checkOrigin = transform.position + Vector3.up * 0.5f;
        Vector3 nextPos = checkOrigin + direction.normalized * 0.3f;
        bool isHit = Physics.Raycast(nextPos, -climbNormal, out RaycastHit hit, wallCheckDistance, wallLayerMask);
        Debug.DrawRay(nextPos, -climbNormal * wallCheckDistance, isHit ? Color.green : Color.red, 0.1f);
        return isHit;
    }

    private Vector3 GetClimbDirection(Vector2 input)
    {
        Vector3 upDir = Vector3.up;
        Vector3 sideDir = Vector3.Cross(climbNormal, upDir).normalized;
        return (upDir * input.y + sideDir * input.x).normalized;
    }

    private void TransitionToClimb()
    {
        movementState = Define.MovementState.Climbing;
        _rigid.useGravity = false;
    }

    private void TransitionToNormal()
    {
        movementState = Define.MovementState.Normal;
        _rigid.useGravity = true;
        _rigid.velocity=Vector3.zero;
    }

    private void TransitionToLeavingWall()
    {
        movementState = Define.MovementState.LeavingWall;
    }
    #endregion

    #region Player Input Action
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
            Ray ray = new Ray(transform.position + offset + Vector3.up * 0.1f, Vector3.down);
            if (Physics.Raycast(ray, 0.2f, groundLayerMask))
                return true;
        }

        return false;
    }
    #endregion

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
    
    private void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

    #region Gizmo Methods
    /*private void OnDrawGizmosSelected()
    {
        if (!Application.isPlaying) return;

        Vector3[] offsets =
        {
            transform.forward * 0.2f,
            -transform.forward * 0.2f,
            transform.right * 0.2f,
            -transform.right * 0.2f,
        };

        Gizmos.color = Color.red;

        foreach (Vector3 offset in offsets)
        {
            Vector3 origin = transform.position + offset + Vector3.up * 0.1f;
            Vector3 direction = Vector3.down * 0.2f;
            Gizmos.DrawRay(origin, direction);
        }
    }*/
    
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = canClimb ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * wallCheckDistance);
    }*/
    #endregion
}
