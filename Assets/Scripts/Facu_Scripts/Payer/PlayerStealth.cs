using Unity.VisualScripting;
using UnityEngine;

public class PlayerStealth : MonoBehaviour
{
    #region INSPECTIOR_ATTRIBUTES
    [Header("Stealth attributes")]
    [SerializeField] float _stealthMultiplier = 5;
    [SerializeField] LayerMask _detectionLayer;
    #endregion
    #region INTERNAL_ATTRIBUTES
    private CharacterController _controller;
    float _detectionRadious;
    Collider[] _colliders;
    private Enemy_Survilance _enemySurvilance;
    #endregion
    private void Start()
    {
        _controller = GameManager.instance.PlayerManager.Controller;
    }

    private void FixedUpdate()
    {
        // calcula el radio de deteccion en base a la velocidad del jugador y chequea si hay enemigos en ese radio
        _detectionRadious = _controller.velocity.magnitude * _stealthMultiplier;
        _colliders = Physics.OverlapSphere(transform.position, _detectionRadious , _detectionLayer);
        if (_colliders.Length > 0)
        {
           foreach(Collider enemyEaring in _colliders) // por cada enemigo en el radio de deteccion le avisa que hay ruido
            {
                _enemySurvilance = enemyEaring.gameObject.GetComponentInChildren<Enemy_Survilance>();
                if (_enemySurvilance == null)
                {   
                    _enemySurvilance = enemyEaring.gameObject.GetComponentInParent<Enemy_Survilance>();
                    if(_enemySurvilance == null)
                    {
                    Debug.LogWarning("Unity es una mierda!!!!!!!!!");
                        continue;

                    }
                }

                _enemySurvilance.NoiseDetected(transform.position);
          
            }
        }
    }
    private void OnDrawGizmos() // dibuja el radio de deteccion en el editor
    {
        if (_controller == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, _detectionRadious);
    }
}
