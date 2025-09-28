using UnityEngine;

public class DoorMechanism : MonoBehaviour
{
    [Header("Door Attributes")]
    [SerializeField] private float _doorheigt;
    [Header("Key Attributes")]
    [SerializeField] private Item _keyItem;
    [SerializeField] private int _securityLevel;
    [Header("Door Movement Attributes")]
    [SerializeField] private float _openedTime = 5f;
    [SerializeField] private float _openSpeed = 5f;
    [SerializeField] private float _closeSpeed = 5f;
    [Header("Player Layer")]
    [SerializeField] private LayerMask _detectionLayer;



    private Inventory _playerInventory;
    private UIManager _uiManager;
    private bool _isOpen = false;
    private Vector3 _doorHeightOffset;
    private Vector3 _startPosition;
   
    private void Start()
    {
        // calcula la posicion a la que se movera la puerta al abrirse
        _doorHeightOffset = transform.position + transform.up * _doorheigt;
        _playerInventory = GameManager.instance.Inventory;
        _startPosition = transform.position;
        _uiManager = GameManager.instance.UIManager;

    }


    private void Update()
    {
        // mueve la puerta hacia arriba si esta abierta, o hacia abajo si esta cerrada
        if (_isOpen)
        {
            transform.position = Vector3.MoveTowards(transform.position, _doorHeightOffset, _openSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, _startPosition, _closeSpeed * Time.deltaTime);
        }
        if (Vector3.Distance(transform.position, _doorHeightOffset) < 0.1f)
        {
            Invoke("CloseDoor", _openedTime);
        }
      
  

    }

    private void CloseDoor()
    {
        _isOpen = false;
    }   


    private void OnCollisionEnter(Collision collision)
    {
      
        // Si el objeto que entra en el trigger es el jugador,
        // verifica si tiene la llave correcta para abrir la puerta
        if (((1 << collision.gameObject.layer) & _detectionLayer) != 0)
        {
            if (_playerInventory.Items.ContainsKey(_keyItem))
            {
               if(_playerInventory.Items[_keyItem] >= _securityLevel)
                {
                    _isOpen = true;
                }
                else
                {
                    _uiManager.PopUpMesssageTimed("You need a key with security level: " + _securityLevel);
                }
            }
            else
            {
                if(_securityLevel == 0)
                {
                    _isOpen = true;
                    return;
                }
                _uiManager.PopUpMesssageTimed("You need a key with security level: " + _securityLevel);
            }
        }
        

    }


}
