using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField]
    private Transform firstPersonPivot;
    [SerializeField]
    private Transform thirdPersonPivot;
    [SerializeField]
    private Transform cameraTransform;

    private bool isFirstPersonPerspective;

    private void Start()
    {
        firstPersonPivot = transform.Find("FirstPersonPivot");
        thirdPersonPivot = transform.Find("ThirdPersonPivot");
        cameraTransform = GetComponentInChildren<Camera>().transform;
        isFirstPersonPerspective = false;
        ChangeView();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            ChangeView();
        }
    }

    private void ChangeView()
    {
        isFirstPersonPerspective = !isFirstPersonPerspective;

        Transform targetPivot = isFirstPersonPerspective ? firstPersonPivot : thirdPersonPivot;
        cameraTransform.position = targetPivot.position;
    }
}
