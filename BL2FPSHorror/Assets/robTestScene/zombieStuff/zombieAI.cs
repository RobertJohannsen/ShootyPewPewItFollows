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
    public bool inRange;
    public int attackStep, attackWindUp, attackCooldownStep, attackCooldown;
    public int damage;
    public float 

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
    }

    // Update is called once per frame
    void Update()
    {
        if((this.transform.position - player.transform.position).magnitude < ) {
            isActive = true;
        }
        if (isActive && !stunned)
       
        checkFOV();

        
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
