using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePad : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3[] moveDestination;
    private Rigidbody _player;
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
    }

    private void LateUpdate()
    {
        deltaPosition = transform.position - lastPosition;
        lastPosition = transform.position;
    }

    private void MoveToDestination()
    {
        Vector3 newPosition = Vector3.MoveTowards(_rigid.position, destination, moveSpeed * Time.fixedDeltaTime);
        _rigid.MovePosition(newPosition);
        
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
        if (other.gameObject.TryGetComponent(out _player))
        {
            _player.MovePosition(_player.position + deltaPosition);
        }
    }
}
