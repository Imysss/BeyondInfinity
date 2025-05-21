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
        isFirstPersonPerspective = true;
        ChangeView();
    }

    public void SwitchView()
    {
        isFirstPersonPerspective = !isFirstPersonPerspective;
        ChangeView();
    }

    private void ChangeView()
    {
        Transform targetPivot = isFirstPersonPerspective ? firstPersonPivot : thirdPersonPivot;
        cameraTransform.position = targetPivot.position;
    }
}
