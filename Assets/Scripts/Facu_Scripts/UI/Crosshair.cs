using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    [Header("Crosshair image reference")]
    [SerializeField] private Image _crosshair;

    private PlayerManager _playerManager;

    private void Start()
    {
        _playerManager = GameManager.instance.PlayerManager;
        _crosshair.enabled = false;
    }

    // Activa o desactiva el crosshair segun si la camara esta en vista de hombro
    private void Update()
    {
        if (_playerManager.Movement.OnShoulderCam)
        {

            _crosshair.enabled = true;
        }
        else
        {
            _crosshair.enabled = false;
        }
    }
}
