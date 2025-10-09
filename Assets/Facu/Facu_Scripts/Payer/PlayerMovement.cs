using System;
using Unity.Burst.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _walkSpeed = 8;
    [SerializeField] private float _crouchSpeed = 5;
    [SerializeField] private float _crawlSpeed = 2;
    [SerializeField] private float _sprintMultiplier = 2;
    [SerializeField] private float _standHeight = 1.8f;
    [SerializeField] private float _crouchHeight = 1;
    [SerializeField] private float _crawlHeight = 0.5f;
    [SerializeField] private float _gravity = 20f;
    [SerializeField] private float _angularSpeed = 50f;


    private bool _onShoulderCam = false;
    private bool _haveStamina;
    private float _actualSpeed;
    private float _actualHeight;
    private float _reativeSpeed;
    private Vector3 _direction;
    private quaternion _newRotation;
    private float _rotationAngle;
    private int _stance;
    private PlayerInputs _inputs;
    private CharacterController _controller;
    private PlayerAnimation _animation;
    private PlayerStamina _stamina;

    public bool OnShoulderCam => _onShoulderCam;
    public int Stance { get { return _stance; } set { _stance = math.clamp(value, 0, 2);}}
    public bool HaveStamina { get { return _haveStamina; } set { _haveStamina = value; } }

    private void Start()
    {
        _inputs = GameManager.instance.Inputs;
        _stamina = GameManager.instance.PlayerManager.Stamina;
        _controller = GetComponent<CharacterController>();
        _animation = GameManager.instance.PlayerManager.Animation;
        _actualSpeed = _walkSpeed;
        _actualHeight = _standHeight;
        ChangeStance(0);
        _stance = 0;
    }


    private void Update()
    {
        if (_inputs.IsSprintPressed)
            _actualSpeed *= _sprintMultiplier;
        if(_inputs.IsSprintHeldPressed)
        {
            _stamina.DrainStamina(Time.deltaTime);
        }
        else
        {
            ChangeStance(_stance);
            if (_inputs.IsLowStancePressed)
                StanceValue(true);
            if (_inputs.IsHighStancePressed)
                StanceValue(false);
        }

        if (_inputs.IsRMBHeldPressed)
        {
            _onShoulderCam = true;
            _rotationAngle = _inputs.MouseXAxis * Time.deltaTime;
            _direction = Camera.main.transform.forward * _inputs.XAxis + Camera.main.transform.forward * _inputs.YAxis;
        }
        else
        {
            _onShoulderCam = false;
            _rotationAngle = _inputs.XAxis * _angularSpeed * Time.deltaTime;
            _direction = transform.forward * _inputs.LateralAxis + transform.forward * _inputs.YAxis;

        }
        _newRotation = transform.rotation * Quaternion.Euler(0, _rotationAngle, 0);
        _controller.transform.rotation=_newRotation;
        _controller.Move((_direction.normalized * _actualSpeed) * Time.deltaTime);
        _reativeSpeed = _controller.velocity.magnitude / _actualSpeed;
        if(_inputs.IsSprintHeldPressed)
            ChangeAnimationOnVelocity(_reativeSpeed*2);
        else {
            ChangeAnimationOnVelocity(_reativeSpeed);
        }
        _controller.Move(transform.up * -_gravity*Time.deltaTime);
    }

    private void ChangeAnimationOnVelocity(float relativeSpeed)
    {
        if (_stance == 0)
            _animation.ChangePlayerSpeed(relativeSpeed);
        else
            _animation.ChangeAnimationSpeed(relativeSpeed);
    }
    private void StanceValue(bool lowerStance)
    {
        if (lowerStance)_stance++;
        else _stance--;
        _stance = math.clamp(_stance, 0, 2);
        ChangeStance(_stance);   
    }

    private void ChangeStance(int newValue) 
    {
        switch (newValue)
        {
            case 1:
                _actualHeight = _crouchHeight;
                _actualSpeed = _crouchSpeed;
                break;
            case 2:
                _actualHeight = _crawlHeight;
                _actualSpeed = _crawlSpeed;
                break;
            default:
                _actualHeight = _standHeight;
                _actualSpeed = _walkSpeed;
                break;
        }
        _controller.height = _actualHeight;
        _controller.center = new Vector3(_controller.center.x, _actualHeight / 2, _controller.center.z);
        _animation.ChangeStanceValue(_stance);
    }

    private void OnDisable()// Cuando el script se desactiva, se actualiza la animacion para que el jugador deje de moverse (idle)
    {
        _animation.ChangePlayerSpeed(0);

    }
}


