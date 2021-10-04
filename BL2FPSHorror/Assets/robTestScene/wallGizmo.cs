using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallGizmo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.5f, 0.24f, 0.6f);
        Gizmos.DrawRay(this.transform.position, -this.transform.up);
    }
}
