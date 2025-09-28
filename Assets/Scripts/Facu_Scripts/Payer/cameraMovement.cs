using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;

public class cameraMovement : MonoBehaviour
{
    #region INSPECTOR_ATTRIBUTES



    [Header("Axis direction attributes")]
    [SerializeField] private float _maxAngleOfCamera = 45;
    [Header("Camera attributes")]
    [SerializeField] private LayerMask _collisionLayers;
    [SerializeField][Range(25, 120)] private float _aimingFOV;
    [SerializeField] private Vector3 _cameraOffset = new Vector3(0,0.5f,0);

    [Header("References")]
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _shoulderCameraPosition;
    #endregion
    #region INTERNAL_ATTRIBUTES
   
    private PlayerInputs _inputs;
    private Vector3 _startPosition;
    private float _normalFOV;
    private float _verticalReference;
    private Quaternion _startRotation;
    private float _xAxis = 0;
    private float _yAxis = 0;
    private Camera _camera;
    private Vector3 _offset;
    private Vector3 _collisionPosition;
    private Rigidbody _rb;
    #endregion

    void Start()
    {
        _startPosition = transform.localPosition;
        _startRotation = transform.localRotation;
        _camera = Camera.main;
        _normalFOV = _camera.fieldOfView;
        Input.ResetInputAxes();
        _inputs = GameManager.instance.Inputs;
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        #region MOUSE_AXIS_PARAMETRIZATION
        _yAxis = _inputs.MouseYAxis * Time.deltaTime;
        _xAxis = _inputs.MouseXAxis * Time.deltaTime;
        #endregion

    }

    void LateUpdate()
    {


        #region VERTICAL_CLAMPING
        if (_inputs.IsRMBHeldPressed)
        {
            _verticalReference = Vector3.Angle(_shoulderCameraPosition.up, transform.forward);
          
            if (_verticalReference <= 90 - _maxAngleOfCamera )
            {
                _yAxis = Mathf.Clamp(_yAxis, 0, 1);
            }
            else if (_verticalReference >= 90 + _maxAngleOfCamera)
            {
                _yAxis = Mathf.Clamp(_yAxis, -1, 0);
            }
            
        }
        else
        {
            _verticalReference = Vector3.Angle(_target.up, (transform.position - _target.position).normalized);
            if (_verticalReference < _maxAngleOfCamera / 2)
            {
                _yAxis = Mathf.Clamp(_yAxis, -1, 0);
            }
            else if (_verticalReference >= (90 - _maxAngleOfCamera / 2))
            {
                _yAxis = Mathf.Clamp(_yAxis, 0, 1);
            }
        }
            
        #endregion
        #region SHOULDER_CAM_TO_NORMAL_CAM_TRANSITION && ROTATION_APPLICATION
      
        if (_inputs.IsRMBClicked)
        {
            transform.localPosition = _shoulderCameraPosition.localPosition;
            transform.localRotation = _shoulderCameraPosition.localRotation;
        }
        if (_inputs.IsRMBHeldPressed)
        {
            transform.localPosition = _shoulderCameraPosition.localPosition;
            _target.Rotate(_target.up ,  _xAxis);
            _camera.fieldOfView = _aimingFOV;
            transform.RotateAround(transform.position, transform.right, _yAxis);
        }
        else if (_inputs.IsRMBReleased)
        {
            transform.localPosition = _startPosition;
            transform.localRotation = _startRotation;
            _camera.fieldOfView = _normalFOV;
        }
        else
        {
            transform.RotateAround(_target.position, _target.up, _xAxis);
            transform.RotateAround(_target.position, transform.right, _yAxis);
            transform.LookAt(_target.position+_cameraOffset);
        }
        #endregion
        /*if (Physics.SphereCast(transform.position, _collisionDetectionRadious, transform.forward, out RaycastHit hitInfo, _collisionDetectionRadious,_collisionLayers))
        {
            _collisionPosition = hitInfo.point + (hitInfo.normal * _collisionDetectionRadious);
            transform.position = Vector3.Lerp(_collisionPosition,transform.position, 20 * Time.fixedDeltaTime);
        }*/

       

    }

}
