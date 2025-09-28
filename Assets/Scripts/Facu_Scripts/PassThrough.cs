using Unity.VisualScripting;
using UnityEngine;

public class PassThrough : MonoBehaviour
{
 
    [SerializeField] private Transform _nextSide;
    [SerializeField] private Vector3 _newSize = Vector3.one;
    [SerializeField] private float _transitSpeed = 2;
    [SerializeField] private float _minimalDistance = 0.2f;
    [SerializeField] private float _sizeChangeSpeed = 1f;
    [SerializeField] private float _rotationSpeed = 25f;

    private PlayerManager _playerManager;
    private PlayerInputs _inputs;
    private bool _isPassing = false;
    private Transform _playerTransform;
    private Rigidbody _playerRigidbody;
    private Vector3 _destination;
    private UIManager _uiManager;



    private void Start()
    {
        _uiManager = GameManager.instance.UIManager;
        _playerManager = GameManager.instance.PlayerManager;
        _inputs = GameManager.instance.Inputs;
        _playerTransform = _playerManager.PlayerObject.transform;
        _playerRigidbody = _playerManager.Rigid_Body;
        _nextSide.position = new Vector3(_nextSide.position.x, _playerTransform.position.y, _nextSide.position.z);
    }

    public void FixedUpdate()
    {
        if(_isPassing)
        {


            if (Vector3.Distance(_playerTransform.position, _nextSide.position) < _minimalDistance)
            {
                _playerTransform.localScale = Vector3.MoveTowards(_playerTransform.localScale, Vector3.one, _sizeChangeSpeed * Time.fixedDeltaTime);
                if (_playerTransform.localScale == Vector3.one)
                {
                    _playerTransform.position = _nextSide.position;
                    _isPassing = false;
                    _playerManager.Movement.enabled = true;
                    _playerRigidbody.isKinematic = false;
                    _playerManager.ActiveCollider.isTrigger = false;
                }
            }
            else
            {
                _playerTransform.localScale = Vector3.MoveTowards(_playerTransform.localScale, _newSize, _sizeChangeSpeed * Time.fixedDeltaTime);
                _playerRigidbody.MoveRotation(Quaternion.Slerp(_playerTransform.rotation, transform.rotation, _rotationSpeed * Time.fixedDeltaTime));
                _destination = Vector3.MoveTowards(_playerTransform.position, _nextSide.position, _transitSpeed * Time.fixedDeltaTime);
                _playerRigidbody.MovePosition(_destination);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_playerManager.CompareLayer(other.gameObject.layer))
        {
            _uiManager.PopUpMessage("Press E to pass through");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(_playerManager.CompareLayer(other.gameObject.layer))
        {
            if (_inputs.IsInteractClicked)
            {
                if(Vector3.Dot(-transform.forward, (_playerTransform.position - transform.position)) < 0)
                {
                    return;
                }

                _playerManager.Movement.enabled = false;
                _playerRigidbody.isKinematic = true;
                _playerManager.ActiveCollider.isTrigger = true;
                _isPassing = true;
               
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (_playerManager.CompareLayer(other.gameObject.layer))
        {
            _uiManager.HideMessage();
        }
    }

    private void OnDrawGizmosSelected()
    {
        if(_nextSide == null) return;
        Gizmos.color = Color.green;
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().size);
        Gizmos.color = Color.red;
        Gizmos.DrawCube(_nextSide.position, _nextSide.GetComponent<BoxCollider>().size);
        Gizmos.color = Color.white;
    }
}