using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallTriggerBehaviour : MonoBehaviour
{


    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponentInParent<zombieAI>())
        {

            zombieAI _zombie = col.gameObject.GetComponentInParent<zombieAI>();
         //   if(_zombie.zombieAgent.isOnOffMeshLink) 
          //  {
                _zombie.zVisualsBehaviour.stopFollow();
          //  }
            
        }
    }
}
