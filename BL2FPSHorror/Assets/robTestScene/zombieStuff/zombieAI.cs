using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class zombieAI : MonoBehaviour
{
    [Header("Assignables")]
    public LineRenderer lr;
    public GameObject player ,eyes ,root;
    public NavMeshAgent zombieAgent;
    public Vector3 overrideTarget;
    public LayerMask levelGeo;

    [Header("Status")]
    public bool isActive;
    public bool inRange, showPath;
    public bool stunned;
    public float currentSpeed;
    public float speedTopBracket;
    public bool isMove;
    public bool startBreaking;


    public int Hp, MaxHp;

    public float radiusFromTarget;

    [Header("Behaviour")]
    public AnimationCurve accelerationCurve;
    public int timeToMaxAcceleration, moveTimeElapsed;
    public float timeInTopSpeed, maxTimeInTopSpeed;
    public float distanceToPlayer;
    public float distanceToSlowdown;
    public float inRangeSlowdownSpeed;
    public float maxAcceleration;
    public float accelerationFloor = 3;
    public float baseSpeed;
    public float normalRunningTurningSpeed;
    public float inRangeOfPlayerTurningSpeed;

    public float rotationSpeed;
    public float FOV, FOVradius, playerDetectionRadius;
    public int attackStep, attackWindUp;
    public int damage;
    public float alertRadius;
    public int stunStep, stunDur;






    // Start is called before the first frame update
    private void Awake()
    {
        Hp = MaxHp;
        zombieAgent = this.GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        stunned = false;
        zombieAgent.updateUpAxis = false;
        zombieAgent.stoppingDistance = 2;
        visualisePath();
    }

    // Update is called once per frame
    void Update()
    {
        
        distanceToPlayer = (this.transform.position - player.transform.position).magnitude;

        if ((this.transform.position - player.transform.position).magnitude < alertRadius) 
        {
            isActive = true;
        }

        currentSpeed = zombieAgent.velocity.magnitude;

        isMove = currentSpeed == 0 ? false : true;

        float topTierSpeedThres = ((Mathf.Clamp(speedTopBracket, 0, 100)) / 100) * zombieAgent.speed;
        if (currentSpeed > topTierSpeedThres) 
        {
            timeInTopSpeed += Time.deltaTime;
            timeInTopSpeed = Mathf.Clamp(timeInTopSpeed, 0, maxTimeInTopSpeed);

            if ((distanceToPlayer < distanceToSlowdown) && (timeInTopSpeed == maxTimeInTopSpeed))
            {
                startBreaking = true;
            }
            root.transform.localRotation = Quaternion.Euler(22, 0, 0);
            
        }
        else
        {
            root.transform.localRotation = Quaternion.Euler(0, 0, 0);
            timeInTopSpeed = 0;
        }

        if(startBreaking)
        {
            if(currentSpeed > inRangeSlowdownSpeed)
            {
                zombieAgent.speed = inRangeSlowdownSpeed;
            }
            else
            {
                startBreaking = false;
                zombieAgent.speed = baseSpeed;
            }
        }

        if(isMove)
        {
            moveTimeElapsed++;
            moveTimeElapsed = Mathf.Clamp(moveTimeElapsed, 0, timeToMaxAcceleration);
        }
        else
        {
            moveTimeElapsed = 0;
        }

        if (distanceToPlayer < distanceToSlowdown)
        {
            zombieAgent.angularSpeed = inRangeOfPlayerTurningSpeed;
        }
        else
        {
            zombieAgent.angularSpeed = normalRunningTurningSpeed;
        }
            zombieAgent.acceleration = Mathf.Clamp((accelerationCurve.Evaluate(moveTimeElapsed / timeToMaxAcceleration)) * maxAcceleration ,accelerationFloor , maxAcceleration);
            
        

            zombieAgent.destination = player.transform.position;
        Debug.DrawRay(this.transform.position, this.transform.forward);
        //Debug.Log(zombieAgent.velocity.magnitude);
        visualisePath();
        checkFOV();
        CheckForAttack();
        CheckForStun();
    }

    void checkFOV()
    {
        Vector3 targetDir = player.transform.position - transform.position;
        float angleToPlayer = (Vector3.Angle(targetDir, transform.forward));
        Vector3 targetDirFromEyes = player.transform.position - eyes.transform.position;
        RaycastHit eyeHit;

       

        if (targetDirFromEyes.magnitude < FOVradius)
        {
            Debug.DrawRay(eyes.transform.position, targetDirFromEyes, Color.cyan);
            Physics.Raycast(eyes.transform.position, targetDirFromEyes, out eyeHit , 100f);

            if (eyeHit.collider.gameObject.GetComponent<moveCont>())
            {
                if (targetDir.magnitude < FOVradius)
                {
                    Vector3 dir = player.transform.position - transform.position;
                    dir.y = 0;//This allows the object to only rotate on its y axis
                    Quaternion rot = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);

                    if (angleToPlayer >= -FOV / 2 && angleToPlayer <= FOV / 2)
                    {
                        
                    }
                    else
                    {
                       
                    }


                }
            }

        }



       
    }

    void visualisePath()
    {
        if(showPath)
        {
            lr.positionCount = zombieAgent.path.corners.Length;
            lr.SetPositions(zombieAgent.path.corners);
            lr.enabled = true;
        }
        else
        {
            lr.enabled = false;
        }
    }
    #region old stuff
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

  

    #endregion


}
