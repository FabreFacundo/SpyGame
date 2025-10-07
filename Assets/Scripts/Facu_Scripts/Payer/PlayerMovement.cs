using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   

    #region INSPECTOR ATTRIBUTES

    [Header("Displacement attributes")]
    [SerializeField] public float _maxNormalVelocity = 10;
    [SerializeField] public float _maxCrouchVelocity = 4;
    [SerializeField] public float _maxCrawlVelocity = 2;
    [SerializeField] public float _maxSprintVelocity = 25;
    [SerializeField] public float _minNormalVelocity = 1;
    [SerializeField][Range(0, 100)] private float _scrollWheelAceleration = 1;
    [SerializeField] private float _maxForceApplied = 500;
    [SerializeField] private float _minForceApplied = 150;

    [Header("Rotation attributes")]
    [SerializeField][Range(0, 360)] private float _maxAngularSpeed = 180;
    [SerializeField][Range(0, 360)] private float _minAngularSpeed = 25;


    [Header("Colliders references")]
    [SerializeField] private CapsuleCollider _walkCollider;
    [SerializeField] private CapsuleCollider _crouchCollider;
    [SerializeField] private CapsuleCollider _crawlCollider;



    #endregion
    #region INTERNAL_ATTRIBUTES
    private int _stanceStep = 0;
    private bool _haveStamina = true;
    private bool _isRuning = false;
    private float _forceVectorMagnitude;
    private float _relativeSpeed;
    private float _angularSpeed;
    private float _rotationAngle;
    private float _maxVelocity;
    private float _speedInterpolation;
    private Vector3 _moveV;
    private Vector3 _moveH;
    private Vector3 _forceVector;
    private Quaternion _rotation = Quaternion.identity;
    private bool _onShoulderCam = false;
    private Rigidbody _rb;
    private PlayerManager _playerManager;
    private PlayerInputs _inputs;
    #endregion
    #region PROPERTIES
    public float Speed
    {
        get { return _maxForceApplied; }
        set { _maxForceApplied = value < 0 ? 0 : value; }
    }
    public bool OnShoulderCam
    {
        get { return _onShoulderCam; }
    }

    public bool HaveStamina
    {
        set { _haveStamina = value; }
    }
    #endregion

    public int StanceStep { get { return _stanceStep; } set { _stanceStep = value; } }
    void Start()
    {
        _maxVelocity = _maxNormalVelocity;
        _playerManager = GameManager.instance.PlayerManager;
        _inputs = GameManager.instance.Inputs;
        _playerManager.ActiveCollider = _walkCollider;
        _rb = _playerManager.Rigid_Body;
        _rb.maxLinearVelocity = _maxVelocity;
        _rb.freezeRotation = true;
    }

    void Update()
    {


        #region LATERAL_ROTATION
        // Si no se esta apuntando con la camara en vista de hombro, rota al jugador segun el input del mouse
        if (!_inputs.IsRMBHeldPressed)
        {
            _onShoulderCam = false;
            _rotationAngle = _inputs.XAxis * _angularSpeed * Time.deltaTime;
   
        }
        else
        {
            _onShoulderCam = true;
            _rotationAngle = _inputs.MouseXAxis * Time.deltaTime;
        }
            _rotation = transform.rotation * Quaternion.Euler(0, _rotationAngle, 0);
        #endregion
        #region LATERAL_DISPLACEMENT
        
            _moveH = transform.right * _inputs.LateralAxis;
        #endregion
        #region FORCE_VECTOR_CALCULATION

        _moveV = transform.forward * _inputs.YAxis;
        _forceVector = (_moveH + _moveV).normalized * _forceVectorMagnitude;

        #endregion
        #region STANCE_MODIFICATION_&_COLLISION
        // Si se esta esprintando y hay estamina, aumenta la velocidad y fuerza aplicada
        if (_inputs.IsSprintHeldPressed && _haveStamina)
        { 
            _forceVectorMagnitude = _maxForceApplied;
            _angularSpeed = _minAngularSpeed;
            _maxVelocity = _maxSprintVelocity;
            _stanceStep = 0;
            _isRuning = true;
            _playerManager.Stamina.DrainStamina(Time.deltaTime);
        }
        // Si no se esta esprintando, ajusta la velocidad y fuerza aplicada segun el input del scroll del mouse
        else
        {
            _isRuning = false;
            _maxVelocity = _maxNormalVelocity;
            _speedInterpolation += (_inputs.SpeedAxis * _scrollWheelAceleration * Time.deltaTime);
            _speedInterpolation = Mathf.Clamp(_speedInterpolation, 0f, 1f);
            _forceVectorMagnitude = Mathf.Lerp(_minForceApplied, _maxForceApplied, _speedInterpolation);
            _angularSpeed = Mathf.Lerp(_maxAngularSpeed, _minAngularSpeed, _speedInterpolation);

            // Cambia la postura del jugador si se presionan las teclas correspondientes
            if (_inputs.IsLowStancePressed)
            {
                _stanceStep++;
            }
            if (_inputs.IsHighStancePressed)
            {
                _stanceStep--;
            }
            _stanceStep = math.clamp(_stanceStep, 0, 2);

        }
        // Ajusta el collider y la velocidad maxima segun la postura actual
        switch (_stanceStep)
        {
            case 0:
                _playerManager.Animation.ChangeAnimationSpeed(1);

                _walkCollider.enabled = true;
                _crawlCollider.enabled = false;
                _crouchCollider.enabled = false;
                _playerManager.ActiveCollider= _walkCollider;
                break;
            case 1:
                _maxVelocity = _maxCrouchVelocity;
                _crouchCollider.enabled = true;
                _walkCollider.enabled = false;
                _crawlCollider.enabled = false;
                _playerManager.ActiveCollider = _crouchCollider;
                break;
            case 2:
                _maxVelocity = _maxCrawlVelocity;
                _crawlCollider.enabled = true;
                _crouchCollider.enabled = false;
                _walkCollider.enabled = false;
                _playerManager.ActiveCollider = _crawlCollider;
                break;

        }
        _rb.maxLinearVelocity = _maxVelocity;
        // Actualiza la animacion segun la postura actual
        _playerManager.Animation.ChangeStanceValue(_stanceStep);

        #endregion
     

    }
    // Fisicas
    void FixedUpdate()
    {
        // Aplica la fuerza calculada al rigidbody del jugador
        _rb.AddForce(_forceVector * Time.fixedDeltaTime );
        // Rota al jugador si no esta apuntando con la camara en vista de hombro
      
            _rb.MoveRotation(_rotation);
        

        _relativeSpeed = (_rb.linearVelocity.magnitude) / _maxVelocity;
        // Actualiza la velocidad de la animacion segun la velocidad relativa
        // Si se esta esprintando, aumenta la variable del blend tree para que la animacion sea mas rapida
        if (_isRuning) 
            _playerManager.Animation.ChangePlayerSpeed(Mathf.Lerp(0,2, _relativeSpeed*2));
        else if(_stanceStep == 0)
            _playerManager.Animation.ChangePlayerSpeed( _relativeSpeed);
        else // Si se esta agachado o gateando, reduce la velocidad de la animacion
            _playerManager.Animation.ChangeAnimationSpeed(_relativeSpeed); 
    }
    
    private void OnDisable()// Cuando el script se desactiva, se actualiza la animacion para que el jugador deje de moverse (idle)
    {
        _playerManager.Animation.ChangePlayerSpeed(0);

    }
}
