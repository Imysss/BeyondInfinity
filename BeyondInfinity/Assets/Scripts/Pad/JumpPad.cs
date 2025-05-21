using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [SerializeField] private float jumpPower = 50f;
    private PlayerController controller;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out controller))
        {
            controller.Jump(jumpPower);
        }
    }
}
