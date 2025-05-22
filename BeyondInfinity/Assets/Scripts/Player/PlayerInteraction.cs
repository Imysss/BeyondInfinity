using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float checkRate = 0.05f;
    private float _lastCheckTime;
    [SerializeField] private float maxCheckDistance = 5f;
    public LayerMask layerMask;

    private GameObject currentInteractGameObject;
    private IInteractable _currentInteractable;
    
    [SerializeField] private TextMeshProUGUI promptText;

    public Transform dropPosition;
    
    private Camera _camera;
    
    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Time.time - _lastCheckTime > checkRate)
        {
            _lastCheckTime = Time.time;

            Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                if (hit.collider.gameObject != currentInteractGameObject)
                {
                    currentInteractGameObject = hit.collider.gameObject;
                    if (hit.collider.TryGetComponent<IInteractable>(out _currentInteractable))
                    {
                        DisplayPromptText();
                    }
                }
            }
            else
            {
                ClearInteraction();
            }
        }
    }

    private void DisplayPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = _currentInteractable.GetInteractPrompt();
    }

    private void OnInteract(InputValue inputValue)
    {
        if (_currentInteractable != null)
        {
            _currentInteractable.Interact();
            ClearInteraction();
        }
    }

    private void ClearInteraction()
    {
        currentInteractGameObject = null;
        _currentInteractable = null;
        promptText.gameObject.SetActive(false);
    }
    
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (_camera == null)
            _camera = Camera.main;

        if (_camera != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 rayOrigin = _camera.transform.position;
            Vector3 rayDirection = _camera.transform.forward * maxCheckDistance;
            Gizmos.DrawRay(rayOrigin, rayDirection);
        }
#endif
    }
}
