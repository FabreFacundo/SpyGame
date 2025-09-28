using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Attributes")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _shootPosition;
    [SerializeField] private float _shootDistance = 10f;
    [SerializeField] private float _throwForce = 100f;
    [SerializeField] private LayerMask _enemyLayer;

    [Header("Prefabs")]
    [SerializeField] private GameObject _SmokeGranadePrefab;

    [Header("Animation Atributtes")]
    [SerializeField] private float _attackAnimationSpeed = 2f;

    [Header("Inventory items")]
    [SerializeField] private Item _dart;
    [SerializeField] private Item _smokeGranade;

    private Vector3 _cameraPoint;
    private RaycastHit _hit;
    private PlayerManager _playerManager;
    private PlayerInputs _inputs;
    private Inventory _inventory;
    private GameObject _granade;
    private bool _isAttacking = false;  

    public bool IsAttacking { get { return _isAttacking; } set { _isAttacking = value; } }

    private void Start()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }
        _playerManager = GameManager.instance.PlayerManager;
        _inputs = GameManager.instance.Inputs;
        _inventory = GameManager.instance.Inventory;    
    }
    private void Update()
    {
        // Dispara un dardo si se hace click izquierdo y la camara esta en vista de hombro
        if (_inputs.IsLMBClicked && _playerManager.Movement.OnShoulderCam ) 
        {
            // verifica que haya dardos en el inventario
            if (_inventory.Items.ContainsKey(_dart) && _inventory.Items[_dart]>0 && !_isAttacking)
            {
                _playerManager.Movement.StanceStep = 0; 
                _playerManager.Animation.SetAttackTrigger();
                _playerManager.GFX.transform.Rotate(0,-90,0);
                _playerManager.GunGFX.SetActive(true);
                _isAttacking = true;
                _playerManager.Movement.enabled = false;
                _playerManager.Animation.ChangeAnimationSpeed(_attackAnimationSpeed);
            }
        }
        // Lanza una granada de humo si se hace click derecho y hay granadas en el inventario
        if (_inputs.IsThrowClicked && _inventory.Items.ContainsKey(_smokeGranade) && !_isAttacking )
        {
            // verifica que haya granadas en el inventario
            if (_inventory.Items[_smokeGranade] > 0)
                ThrowGranade();
         
        }
    }

    // Lanza una granada de humo desde la posicion de disparo hacia adelante con una fuerza determinada
    private void ThrowGranade()
    {
        _granade=Instantiate(_SmokeGranadePrefab, _shootPosition.position, Quaternion.identity);
        _granade.GetComponent<SmokeGranade>().Trhow();
        _granade.GetComponent<Rigidbody>().AddForce(_camera.transform.forward * _throwForce);
        _inventory.RemoveItem(_smokeGranade);
    }
    // Dispara un rayo desde la posicion de disparo hacia el punto que mira la camara
    public void Attack()
    {
        _cameraPoint = GetCameraPoint();
        if (Physics.Raycast(_shootPosition.position, (_cameraPoint - _shootPosition.position), out _hit, _shootDistance, _enemyLayer))
        {
            // Si el rayo colisiona con un enemigo, deshabilita al enemigo y remueve un dardo del inventario
            _hit.collider.GetComponentInParent<Enemy>().Neutralize();
        }
        _inventory.RemoveItem(_dart);
    }


    // Devuelve el punto en el que la camara esta mirando, si no hay colision devuelve Vector3.zero
    Vector3 GetCameraPoint()
    {
        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit))
        {
           
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }
}
