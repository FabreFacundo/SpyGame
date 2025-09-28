using UnityEngine;

public class HideLocker : MonoBehaviour
{
    [SerializeField] private string _popUpMessage = "Press 'E' to hide"; 
    private PlayerManager _playerManager;
    private PlayerInputs _inputs;
    private bool _isPlayerHidden = false;


    private void Start()
    {
        _playerManager = GameManager.instance.PlayerManager;
        _inputs = GameManager.instance.Inputs;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(_playerManager.PlayerTag))
        {
            GameManager.instance.UIManager.PopUpMesssageTimed(_popUpMessage);
        }
    }




    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(_playerManager.PlayerTag))
        {
            if(_inputs.IsInteractClicked && !_isPlayerHidden)
            {
                _isPlayerHidden = !_isPlayerHidden;
                _playerManager.Movement.enabled = false;
                _playerManager.Rigid_Body.isKinematic = true;
                foreach ( Renderer r in _playerManager.Renderers)
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
                _playerManager.Rigid_Body.isKinematic = false;
                foreach (Renderer r in _playerManager.Renderers)
                {
                    r.enabled = true;
                }
                _playerManager.PlayerObject.layer = _playerManager.PlayerLayer;
                _playerManager.GFX.layer = _playerManager.PlayerLayer;
            }

        }
    }

}
