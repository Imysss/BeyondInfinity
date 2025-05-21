using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePad : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3[] moveDestination;
    private PlayerController controller;
    private Rigidbody _rigid;

    [SerializeField] private int maxDestinationIndex;
    [SerializeField] private int destinationIndex = 0;
    private Vector3 destination;

    private Vector3 lastPosition;
    private Vector3 deltaPosition;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        maxDestinationIndex = moveDestination.Length;
        SetDestination(destinationIndex);

        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        MoveToDestination();
        deltaPosition = transform.position - lastPosition;
        lastPosition = transform.position;
    }

    private void MoveToDestination()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.fixedDeltaTime);
        if (Vector3.Distance(transform.position, destination) <= 0.01f)
        {
            SetDestination(++destinationIndex);
        }
    }

    private void SetDestination(int index)
    {
        destination = moveDestination[index % maxDestinationIndex];
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.TryGetComponent(out controller))
        {
            other.transform.position += deltaPosition;
        }
    }
}
