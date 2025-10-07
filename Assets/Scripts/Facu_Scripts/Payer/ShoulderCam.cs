using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;

public class ShoulderCam : MonoBehaviour
{
    #region INSPECTOR_ATTRIBUTES

    [SerializeField] private GameObject _normalCamera;
    [SerializeField] private GameObject _shoulderCamera;


    #endregion
    #region INTERNAL_ATTRIBUTES
    private bool _isInShoulderCam = false;
    private PlayerInputs _inputs;
    private PlayerManager _playerManager;
    #endregion

    #region PROPERTIES
    public bool IsInShoulderCam => _isInShoulderCam;
    #endregion

    private void Start()
    {
        if (_normalCamera == null)
            _normalCamera = FindAnyObjectByType<CinemachineOrbitalFollow>(FindObjectsInactive.Include).gameObject;
        if(_shoulderCamera==null)
            _shoulderCamera = FindAnyObjectByType<CinemachineThirdPersonAim>(FindObjectsInactive.Include).gameObject;

        _inputs = GameManager.instance.Inputs;
        _playerManager = GameManager.instance.PlayerManager;
    }

    private void LateUpdate()
    {
        if (_inputs.IsRMBHeldPressed)
        {
            _isInShoulderCam = true;
            _normalCamera.SetActive(false);
            _shoulderCamera.SetActive(true);

          

        }
        else
        {
            _isInShoulderCam = false;
            _normalCamera.SetActive(true);
            _shoulderCamera.SetActive(false);
        }
    }
    
 

}
