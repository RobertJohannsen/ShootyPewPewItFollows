using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieCore : MonoBehaviour
{
    RaycastHit wallCheck ,ledgeHit;
    public float wallCheckHeight;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 calculateWallClimb()
    {
        Physics.Raycast(this.transform.position, this.transform.forward * 100, out wallCheck, 5); // the raycast going in the direction youre looking

        Vector3 LedgePoint = new Vector3(wallCheck.point.x, wallCheck.point.y + wallCheckHeight, wallCheck.point.z); // the vector of the line goinf down to find the ledge
                                                                                                                        // Debug.DrawRay(LedgePoint, -transform.up * 100, Color.cyan);
        Physics.Raycast(LedgePoint, -transform.up * 100, out ledgeHit); // the point of the ledge grab

        Debug.DrawRay(ledgeHit.point, -wallCheck.normal * 0.5f, Color.green);

        Debug.DrawRay(ledgeHit.point + (-wallCheck.normal), Vector3.up, Color.green);

        return Vector3.zero;
    }
}
