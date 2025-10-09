using UnityEngine;

public class HideLocker : MonoBehaviour
{
    [SerializeField] private string _popUpMessage = "Press 'E' to hide"; 
    private PlayerManager _playerManager;
    private PlayerInputs _inputs;
    private bool _isPlayerHidden = false;
    private bool _isPlayerInZone = false;


    private void Start()
    {
        _playerManager = GameManager.instance.PlayerManager;
        _inputs = GameManager.instance.Inputs;
    }

    private void Update()
    {
        if (!_isPlayerInZone) return;
        if (_inputs.IsInteractClicked && !_isPlayerHidden)
        {
            _isPlayerHidden = !_isPlayerHidden;
            _playerManager.Movement.enabled = false;
            foreach (Renderer r in _playerManager.Renderers)
            {
                r.enabled = false;
            }
            _playerManager.PlayerObject.layer = LayerMask.NameToLayer(_playerManager.HideLayerName);
            _playerManager.GFX.layer = LayerMask.NameToLayer(_playerManager.HideLayerName);
        }
        else if (_inputs.IsInteractClicked && _isPlayerHidden)
        {
            _isPlayerHidden = !_isPlayerHidden;
            _playerManager.Movement.enabled = true;
            foreach (Renderer r in _playerManager.Renderers)
            {
                r.enabled = true;
            }
            _playerManager.PlayerObject.layer = _playerManager.PlayerLayer;
            _playerManager.GFX.layer = _playerManager.PlayerLayer;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_playerManager.PlayerTag))
        {
            GameManager.instance.UIManager.PopUpMessageTimed(_popUpMessage);
        }
    }




    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(_playerManager.PlayerTag))
        {
            _isPlayerInZone = true;
            GameManager.instance.UIManager.PopUpMessage(_popUpMessage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(_playerManager.PlayerTag))
        {
            _isPlayerInZone = false;
            GameManager.instance.UIManager.HideMessage();
        }
    }

}
