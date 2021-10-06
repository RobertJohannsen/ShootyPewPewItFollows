using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieHeadBehaviour : MonoBehaviour
{
    public GameObject player;
    public GameObject zHead;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("zombieCont").gameObject.GetComponent<zombieController>().player;

        zHead = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.parent.parent.GetComponent<zombieVisualsBehaviour>().followTarget)
        {
            if ((this.transform.position - player.transform.position).magnitude < this.transform.parent.parent.parent.gameObject.GetComponent<zombieAI>().zombieAwakeDetectionRadius)
            {
                this.transform.LookAt(player.transform);
            }

            Debug.DrawRay(this.transform.position, this.transform.forward, Color.red);
        }

        
       
    }
}
