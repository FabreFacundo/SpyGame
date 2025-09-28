using UnityEngine;

public class HitDetection : MonoBehaviour
{
    [Header("Hit Detection attributes")]
    [SerializeField] private float _damage = 25f;


    private PlayerManager _playerManager;

    private void Start()
    {
        _playerManager = GameManager.instance.PlayerManager;
    }

    private void OnTriggerEnter(Collider other) 
    {
        // verifica si el objeto que colisiona es el jugador
        if (_playerManager.CompareLayer(other.gameObject.layer))
        {
            // aplica el daño al jugador
            _playerManager.Health.TakeDamage(_damage);
        }
    }

}
