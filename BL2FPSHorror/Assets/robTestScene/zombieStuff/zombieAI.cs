using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class zombieAI : MonoBehaviour
{
    public zombieCore core;
    public GameObject targetPointer, eyes;
    public Transform target ,player;
    public Vector3 overrideTarget;
    public NavMeshAgent zombieAgent;
    public float radiusFromTarget;
    public LayerMask levelGeo;
    public bool isActive;
    public float FOV , FOVradius ,zombieAwakeDetectionRadius;
    public enum zombieState {walk , run , wallClimb , fall , attackObj }
    public zombieState currentSubroutine;
    public zombieVisualsBehaviour zVisualsBehaviour;

    public bool inRange;
    public int attackStep, attackWindUp, attackCooldownStep, attackCooldown;
    public int damage;

    public int Hp, MaxHp;

    public bool stunned;
    public int stunStep, stunDur;

    private Vector3 LedgePoint;
    private RaycastHit wallCheck, ledgeHit;
    public float wallCheckHeight;
    
    // Start is called before the first frame update
    private void Awake()
    {
        Hp = MaxHp;
        zombieAgent = this.GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        stunned = false;
        player = GetComponentInParent<zombieController>().player.transform;
        zombieAgent.updateUpAxis = false;
        zVisualsBehaviour = this.GetComponentInChildren<zombieVisualsBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if((this.transform.position - player.transform.position).magnitude < 20) {
            isActive = true;
        }
        if (isActive && !stunned)
        doSubRoutines();
       
        checkFOV();

        CalculateWallSubPos();

        CheckForAttack();
        CheckForStun();
    }

    void CheckForStun()
    {
        if(stunned)
        {
            stunStep++;
            stunStep = Mathf.Clamp(stunStep, 0, stunDur);

            if(stunStep == stunDur)
            {
                stunned = false;
                zVisualsBehaviour.restartFollow();
            }
        }
    }
    public void takeDamage( int damage)
    {
        Hp -= damage;
        Hp = Mathf.Clamp(Hp, 0, MaxHp);

        if(Hp == 0) 
        {
            Destroy(this.gameObject);
        }
    }

    public void doStun()
    {
        stunStep = 0;
        stunned = true;
        calculateStunPos(); //knock back
    }

    void calculateStunPos()
    {
        zVisualsBehaviour.stopFollow();
    }

    void CheckForAttack()
    {
        inRange = false;
        if (((this.transform.position - player.transform.position).magnitude < 2.5) && !stunned)
        {
            inRange = true;
        }

        if(inRange)
        {
            attackStep++;
            attackStep = Mathf.Clamp(attackStep, 0, attackWindUp);
         
        }

        if (attackStep == attackWindUp)
        {
            attackStep = 0;
            doAttack();
        }
    }

    private void doAttack()
    {
        player.GetComponent<moveCont>().takeDamage(damage);
        Debug.Log("hit player");
    }

    private void doSubRoutines()
    {
        switch (currentSubroutine)
        {
            case zombieState.walk:
                walkSubRoutine();
                break;
            case zombieState.run:
                break;
            case zombieState.wallClimb:
                wallClimbSubRoutine();
                break;
            case zombieState.fall:
                break;
            case zombieState.attackObj:
                break;
        }
    }

    private void walkSubRoutine()
    {
        Debug.DrawRay(this.transform.position, this.transform.forward, Color.green);
        zombieAgent.stoppingDistance = radiusFromTarget;
        Vector3 temp = this.transform.position - target.position;
        zombieAgent.destination = target.position;
       // targetPointer.transform.position = (temp.normalized * radiusFromTarget) + target.position;

        if (this.transform.position == zombieAgent.destination)
        {
            Debug.Log("at destination");
        }
    }

    public void tryWallSubroutine()
    {
        if(currentSubroutine == zombieState.walk)
        {
            CalculateWallSubPos();
            //currentSubroutine = zombieState.wallClimb;
            //targetPointer.transform.position = overrideTarget;
        }
    }
    public void CalculateWallSubPos()
    {
        Physics.Raycast(eyes.transform.position, eyes.transform.forward * 100, out wallCheck, 100f, levelGeo, QueryTriggerInteraction.Ignore) ; // the raycast going in the direction youre looking
            Debug.DrawRay(eyes.transform.position, eyes.transform.forward * 100);

            RaycastHit ceilingCheck;
            Physics.Raycast(wallCheck.point , this.transform.up , out ceilingCheck , 100f ,levelGeo);;
            Debug.DrawRay(wallCheck.point , this.transform.up *100, Color.red);
                if(ceilingCheck.distance != 0)
                {
            
                 LedgePoint = new Vector3(wallCheck.point.x, wallCheck.point.y + 750, wallCheck.point.z);

                }
                else
                {
                 LedgePoint = new Vector3(wallCheck.point.x, wallCheck.point.y + 750, wallCheck.point.z);
                }

            // the vector of the line goinf down to find the ledge
            //Debug.DrawRay(LedgePoint, -transform.up, Color.cyan);
            Physics.Raycast(LedgePoint, -transform.up, out ledgeHit , levelGeo); // the point of the ledge grab
        //Debug.Log(ledgeHit.point);


            Debug.DrawRay(ledgeHit.point + (-wallCheck.normal), Vector3.up, Color.blue);
        //overrideTarget = ledgeHit.point + (-wallCheck.normal);
        //Debug.Log("override set to " + overrideTarget);




    }

    void wallClimbSubRoutine()
    {
        zombieAgent.destination = overrideTarget;
        if(zombieAgent.transform.position == zombieAgent.destination)
        {
            currentSubroutine = zombieState.walk;
            Debug.Log("subroutine over");
        }
    }

    void checkFOV()
    {
        Vector3 targetDir = player.position - transform.position;
        float angleToPlayer = (Vector3.Angle(targetDir, transform.forward));
        
        if(targetDir.magnitude < FOVradius) 
        {
            if (angleToPlayer >= -FOV / 2 && angleToPlayer <= FOV / 2)
            {
                //do stuff
            }// 180° FOV
                
        }
    }
        


}
