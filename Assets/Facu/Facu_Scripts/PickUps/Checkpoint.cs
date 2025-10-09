using UnityEngine;

public class Checkpoint : MonoBehaviour
{
   
    private PlayerManager _playerManager;

    private void Start()
    {
        _playerManager = GameManager.instance.PlayerManager;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_playerManager.CompareLayer(other.gameObject.layer))
        {
            GameManager.instance.SetCheckpoint(other.transform.position);
        }

    }

}
