using UnityEngine;

public abstract class Enemy : MonoBehaviour
{

    private CheckPointManager _checkPointManager;
 

    protected Enemy_agent _agent;



    protected virtual void Start()
    {
        _agent = GetComponent<Enemy_agent>();
        _checkPointManager = GameManager.instance.CheckPointManager;
    }
    public abstract void Attack();
    public virtual void Neutralize()
    {
         GetComponent<Collider>().enabled = false;
        _checkPointManager.DeleteEnemy(gameObject.name);
        _agent.Agent.enabled = false;
        _agent.enabled = false;
        GetComponentInChildren<Enemy_Survilance>().enabled = false;
        GetComponentInChildren<Enemy_Survilance>().gameObject.GetComponent<MeshRenderer>().enabled = false;
    }
   

    
}
