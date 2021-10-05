using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class navWallinEditor : MonoBehaviour
{
    // Start is called before the first frame update


    void Awake()
    {
        if (!this.GetComponent<BoxCollider>()) return;
        this.GetComponent<BoxCollider>().isTrigger = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (!this.GetComponent<BoxCollider>()) return;
        if (!this.GetComponent<NavMeshSurface>()) return;

        this.GetComponent<BoxCollider>().center = this.GetComponent<NavMeshSurface>().center;
        this.GetComponent<NavMeshSurface>().size = new Vector3(this.GetComponent<NavMeshSurface>().size.x, Mathf.Clamp(this.GetComponent<NavMeshSurface>().size.y, 0, 1.5f), this.GetComponent<NavMeshSurface>().size.z);
        
        this.GetComponent<BoxCollider>().size = this.GetComponent<NavMeshSurface>().size;

    }


}
