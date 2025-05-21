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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            isFirstPersonPerspective = !isFirstPersonPerspective;
        }
    }
}
