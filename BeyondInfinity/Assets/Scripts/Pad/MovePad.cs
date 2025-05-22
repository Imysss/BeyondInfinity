using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePad : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector3[] moveDestination;
    [SerializeField] private Rigidbody _player;
    private Rigidbody _rigid;
    
    [SerializeField] private int currentIndex = 0;
    private Vector3 destination;

    private Vector3 platformVelocity;
    
    private Vector3 lastPosition;
    private Vector3 deltaPosition;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _rigid.isKinematic = true;
    }

    private void Start()
    {
        destination = moveDestination[currentIndex];
        lastPosition = transform.position;
    }

    private void FixedUpdate()
    {
        MoveToDestination();
    }

    private void MoveToDestination()
    {
        deltaPosition = transform.position - lastPosition;
        lastPosition = transform.position;

        // Transform으로 직접 위치 이동
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.fixedDeltaTime);

        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            currentIndex = (currentIndex + 1) % moveDestination.Length;
            destination = moveDestination[currentIndex];
        }
    }
    
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.TryGetComponent(out _player))
        {
            _player.position += deltaPosition;
        }
    }
}
