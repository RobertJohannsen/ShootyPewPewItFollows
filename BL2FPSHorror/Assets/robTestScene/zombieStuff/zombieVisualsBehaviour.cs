using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieVisualsBehaviour : MonoBehaviour
{
    public Transform target;
    public bool followTarget;
    public Rigidbody rb;
    public GameObject rootObj;
    public float followProffiency;
    public float lockRotationx, lockRotationy, lockRotationz;
    public GameObject visualsParent;
    public GameObject head, feet;
    public bool visualsGrounded;
    public LayerMask levelGeo;
    // Start is called before the first frame update
    void Start()
    {
        rootObj = this.gameObject;
        visualsParent = this.gameObject.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        yesFollow();
        checkGround();
        //lockRotation();

        if(Input.GetKeyDown(KeyCode.R))
        {
            restartFollow();
        }
    }

    void lockRotation()
    {
        this.transform.localRotation = Quaternion.Euler(Mathf.Clamp(this.transform.localRotation.x , -lockRotationx , lockRotationx)
            , Mathf.Clamp(this.transform.localRotation.y, -lockRotationy, lockRotationy)
            , Mathf.Clamp(this.transform.localRotation.z, -lockRotationz, lockRotationz));
    }

    public void restartFollow()
    {
        standUp();
        this.transform.parent = visualsParent.transform;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        followTarget = true;
    }

    void yesFollow()
    {
        if (followTarget)
        {
            rootObj.GetComponent<Rigidbody>().velocity = (target.position - rootObj.transform.position) * followProffiency;
        }
    }

    public void stopFollow()
    {
        this.transform.parent = null;
        this.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            followTarget = false;
        Debug.Log("stopped follow");
    }

    public void standUp()
    {
        this.transform.rotation = Quaternion.Euler(0, -90, 0);
    }

    public void checkGround()
    {
        visualsGrounded = Physics.CheckSphere(feet.transform.position , 2f , levelGeo); 
      
    }



}
