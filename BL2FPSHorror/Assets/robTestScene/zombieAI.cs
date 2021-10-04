using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class zombieAI : MonoBehaviour
{
    public zombieCore core;
    public GameObject targetPointer;
    public Transform target;
    private NavMeshAgent zombieAgent;
    public float radiusFromTarget;
    // Start is called before the first frame update
    private void Awake()
    {
        zombieAgent = this.GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        zombieAgent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(this.transform.position, this.transform.forward, Color.green);
        zombieAgent.stoppingDistance = radiusFromTarget;
        Vector3 temp = this.transform.position - target.position;
        zombieAgent.destination = target.position;
        targetPointer.transform.position = (temp.normalized * radiusFromTarget) + target.position;;

        if (this.transform.position == zombieAgent.destination)
        {
            Debug.Log("at destination");
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if
    }


}
