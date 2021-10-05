using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class wallNavMeshBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.GetComponentInParent<zombieAI>())
        {
            if (col.gameObject.transform.position.y < this.transform.position.y)
            {
                Debug.Log(col.gameObject.transform.position.y + " + " + this.transform.position.y + " going up");
                col.GetComponentInParent<zombieAI>().tryWallSubroutine();
            }
                
        }
    }
}
