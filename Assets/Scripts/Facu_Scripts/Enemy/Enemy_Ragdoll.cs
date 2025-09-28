using System.Collections;
using UnityEngine;

public class Enemy_Ragdoll : MonoBehaviour
{

    [SerializeField] private Transform _rigReference;
    [SerializeField] private float _timeToFreeze = 3f;
    private Animator _animator;
    private Rigidbody[] _rigidbodies;
    
    private void Start()
    {
       
        _animator = GetComponent<Animator>();
        _rigidbodies = _rigReference.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody rb in _rigidbodies)
        {
            rb.isKinematic = true;
        }
    }

 

    public void ActivateRagdoll()
    {
        _animator.enabled = false;
        foreach (Rigidbody rb in _rigidbodies)
        {
            rb.isKinematic = false;
           
        }
        Invoke(nameof(FreezeRigidBody), _timeToFreeze);
    }

    public void FreezeRigidBody()
    {
        _animator.enabled = false;
        foreach (Rigidbody rb in _rigidbodies)
        {
            rb.isKinematic = true;
            rb.gameObject.GetComponent<Collider>().isTrigger=true;
        }
    }

}
