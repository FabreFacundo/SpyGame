using UnityEngine;

public class TorretConsole : MonoBehaviour
{
    private EnemyManager _enemyManager;
    private PlayerInputs _inputs;
    private PlayerManager _playerManager;
    private UIManager _uiManager;   

    private void Start()
    {
        _playerManager = GameManager.instance.PlayerManager;
        _inputs = GameManager.instance.Inputs;
        _uiManager = GameManager.instance.UIManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_playerManager.CompareLayer(other.gameObject.layer))
        {
            _uiManager.PopUpMessage("Press E to disable all torrets");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        _enemyManager = GameManager.instance.EnemyManager;
        if (_playerManager.CompareLayer(other.gameObject.layer))
        {
            if(_inputs.IsInteractClicked)
                foreach (Enemy enemy in _enemyManager.Enemies)
                {
                    if (enemy is EnemyTurret turret)
                    {
                        turret.Neutralize();
                    }
                }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (_playerManager.CompareLayer(other.gameObject.layer))
        {
            _uiManager.HideMessage();
        }
    }
}
