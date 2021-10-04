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

        this.GetComponent<BoxCollider>().center = Vector3.zero;
        this.GetComponent<NavMeshSurface>().center = Vector3.zero;
        this.GetComponent<BoxCollider>().size = this.GetComponent<NavMeshSurface>().size;
    }


}
