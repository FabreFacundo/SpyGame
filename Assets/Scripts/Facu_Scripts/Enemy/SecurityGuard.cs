using UnityEngine;

public class SecurityGuard : Enemy
{

    [Header("Animator reference")]
    [SerializeField] private Animator _animator;
    [Header("Animation attributes")]
    [SerializeField] private float _animationChangeFactor;
    [SerializeField] private GameObject _visionCone;



    private Enemy_agent _agent;
    private Rigidbody _rb;
    private float _animationBlend;
    private Enemy_Ragdoll _ragdoll;
    
    private void Start()
    {
        _agent = GetComponent<Enemy_agent>();
        _rb = GetComponent<Rigidbody>();
        _ragdoll = GetComponentInChildren<Enemy_Ragdoll>();


    }

    private void Update()
    {
        // modifica la velocidad del animator en funcion del estado del enemigo
        if (_agent.ActualState == Enemy_agent.ENEMY_STATE.ATTACKING)
        {
            _animator.speed = 1;
            _animationBlend += _animationChangeFactor * Time.deltaTime;
            _animationBlend = Mathf.Clamp(_animationBlend, 0, 1);
            _animator.SetFloat("Walk_Run", _animationBlend);
        }
        else
        {
            _animator.speed = _agent.Agent.velocity.magnitude / _agent.Agent.speed;
            _animator.SetFloat("Walk_Run", 0);
            _animationBlend = 0;
        }
    }
    public override void Attack() // dispara la animacion de ataque
    {
        _animator.SetTrigger("attack");
    }

    public override void Neutralize() // desactiva el agente y modifica rigidbody para que el enemigo se caiga por el suelo 
    {
         base.Neutralize();
        _agent.Agent.enabled = false;
        _agent.enabled = false;
        _ragdoll.ActivateRagdoll();
        _visionCone.SetActive(false);
    }
}
