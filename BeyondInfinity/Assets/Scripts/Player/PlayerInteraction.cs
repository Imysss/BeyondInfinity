using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private float _checkRate = 0.05f;
    private float _lastCheckTime;
    private float _maxCheckDistance = 3f;
    public LayerMask layerMask;

    [SerializeField] private GameObject currentInteractGameObject;
    private IInteractable _currentInteractable;
    
    [SerializeField] private TextMeshProUGUI promptText;
    
    public ItemData itemData;   //금방 획득한 아이템 데이터
    public Action OnAddItem;    //아이템 획득 이벤트

    public Transform dropPosition;
    
    private Camera _camera;
    
    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Time.time - _lastCheckTime > _checkRate)
        {
            _lastCheckTime = Time.time;

            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != currentInteractGameObject)
                {
                    currentInteractGameObject = hit.collider.gameObject;
                    if (hit.collider.TryGetComponent<IInteractable>(out _currentInteractable))
                    {
                        SetPromptText();
                    }
                }
            }
            else
            {
                currentInteractGameObject = null;
                _currentInteractable = null;
                promptText.gameObject.SetActive(false);
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = _currentInteractable.GetInteractPrompt();
    }

    private void OnInteract(InputValue inputValue)
    {
        if (_currentInteractable != null)
        {
            _currentInteractable.Interact();
            currentInteractGameObject = null;
            _currentInteractable = null;
            promptText.gameObject.SetActive(false);
        }
    }
}
