using UnityEngine;

public class MovePad : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private Vector3[] moveDestination;
    
    private Rigidbody _player;
    private Rigidbody _rigid;
    
    private Vector3 destination;
    private Vector3 deltaPosition;
    private int currentIndex;

    #region Unity Methods
    private void Awake()
    {
        _rigid = GetComponent<Rigidbody>();
        _rigid.isKinematic = true;
    }

    private void Start()
    {
        destination = moveDestination[currentIndex];
    }

    private void FixedUpdate()
    {
        MoveToDestination();
    }
    
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.TryGetComponent(out _player))
        {
            _player.position += deltaPosition;
        }
    }
    #endregion

    #region Movement Logic
    private void MoveToDestination()
    {
        Vector3 nextPosition = Vector3.MoveTowards(_rigid.position, destination, moveSpeed * Time.fixedDeltaTime);

        //이동 전 → 정확한 delta 계산
        deltaPosition = nextPosition - _rigid.position;

        //실제 이동
        _rigid.position = nextPosition;
        
        if (Vector3.Distance(transform.position, destination) < 0.01f)
        {
            currentIndex = (currentIndex + 1) % moveDestination.Length;
            destination = moveDestination[currentIndex];
        }
    }
    #endregion
}
