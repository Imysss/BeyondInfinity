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
        Vector3 nextPosition = Vector3.MoveTowards(_rigid.position, destination, moveSpeed * Time.fixedDeltaTime);

        //이동 전 → 정확한 delta 계산
        deltaPosition = nextPosition - _rigid.position;

        //실제 이동
        _rigid.position = nextPosition;
        lastPosition = nextPosition;
        
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
