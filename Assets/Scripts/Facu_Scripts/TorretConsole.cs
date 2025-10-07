using UnityEngine;

public class TorretConsole : MonoBehaviour
{
    private EnemyManager _enemyManager;
    private PlayerInputs _inputs;
    private PlayerManager _playerManager;
    private UIManager _uiManager;  
    private bool _isPlayerInZone = false;  
    private bool _isDisabled = false;

    private void Start()
    {
        _playerManager = GameManager.instance.PlayerManager;
        _inputs = GameManager.instance.Inputs;
        _uiManager = GameManager.instance.UIManager;
    }


    private void Update()
    {
        if (_inputs.IsInteractClicked && _isPlayerInZone && !_isDisabled)
        {
            foreach (Enemy enemy in _enemyManager.Enemies)
            {
                if (enemy is EnemyTurret turret)
                {
                    turret.Neutralize();
                }
            }
            _uiManager.PopUpMessageTimed("All torrets where disabled!");
            _isDisabled = true;
        }
    }

 
    private void OnTriggerStay(Collider other)
    {
        _enemyManager = GameManager.instance.EnemyManager;
        if (_playerManager.CompareLayer(other.gameObject.layer))
        {
            _uiManager.PopUpMessage("Press E to disable all torrets");
            _isPlayerInZone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (_playerManager.CompareLayer(other.gameObject.layer))
        {
            _uiManager.HideMessage();
            _isPlayerInZone = false;
        }
    }
}
