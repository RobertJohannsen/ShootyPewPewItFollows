using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponCore : MonoBehaviour
{
    public int currentWeaponID;
    public weaponLibrary weaponLib;
    public gunAnimationController gunAnimationCont;
    public GameObject weaponBarrel;
    public bool triggerDown , shootReady;
    public int elapseCycleTime , ejectFrames , totalCycleTime;
    public float baseAimConeAccuracy, moveAimConeAccuracy , currentAimCone;
    public float maxShotDistance;
    public int currentAmmo, magCapacity , ammoPool;
    public int bashDuration, bashStep;
    public Vector3 shootAfterSpreadDirection;
    public int weaponDamage;
    public enum reState { no , eject , insert }
    public reState gunState;
    public bool startBash;
    /// <summary>
    ///  no -> eject -> insert -> no
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        startBash = false;
        currentAmmo = magCapacity;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            triggerDown = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            triggerDown = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            bash();
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            reload();
        }
   



        Debug.DrawRay(weaponBarrel.transform.position, shootAfterSpreadDirection, Color.black);
        tryFireWeapon();
        cycleCycle();
        gunBash();

    }

    public void gunBash()
    {
        if(startBash)
        {
            bashStep++;
            bashStep = Mathf.Clamp(bashStep, 0, bashDuration);
            RaycastHit bashHit;
            Physics.Raycast(weaponBarrel.transform.position, transform.forward, out bashHit, maxShotDistance);
            Debug.DrawRay(weaponBarrel.transform.position, weaponBarrel.transform.forward , Color.cyan);

            

            if(bashStep == bashDuration)
            {
                bashStep = 0;
                startBash = false;
            }

            if (bashHit.collider.gameObject != null)
            {
                switch (bashHit.collider.gameObject.tag)
                {
                    case "zombieHead":

                        bashHit.collider.gameObject.GetComponent<zombieScriptReferenceLib>()._zombieAI.doStun();
                        break;
                    case "zombieBody":
                        bashHit.collider.gameObject.GetComponent<zombieScriptReferenceLib>()._zombieAI.doStun();
                        break;
                }
            }
           
        }
    }

    public void tryFireWeapon()
    {
        if (triggerDown && shootReady)
        {
            if (currentAmmo > 0)
            {
                if (gunState == reState.no)
                {
                    currentAmmo--;




                    fireWeapon();
                            //fire actual bullet
                            

                        

                    shootReady = false;
                    //do reload here
                  
                }

            }

        }
    }
    public void fireWeapon()
    {
        gunAnimationCont.callShootAnimation();
        float x = Random.insideUnitCircle.x * currentAimCone;
        float y = Random.insideUnitCircle.y * currentAimCone; ;
        float z = Random.Range(-currentAimCone / 1, currentAimCone/ 1);

        //Calculate Direction with Spread
        //Vector3 shootAfterSpreadDirection = weaponBarrel.transform.forward + new Vector3(-x, -y, -z);

        RaycastHit weaponHit;
        Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward , out weaponHit , maxShotDistance);

        switch (weaponHit.collider.gameObject.tag)
        {
            case "zombieHead":
                weaponHit.collider.gameObject.transform.parent.parent.parent.GetComponent<zombieAI>().takeDamage(weaponDamage);
                break;
            case "zombieBody":
                weaponHit.collider.gameObject.transform.parent.parent.parent.GetComponent<zombieAI>().takeDamage(weaponDamage);
                break;
        }

    }

    public void reload()
    {
        if(ammoPool - magCapacity > 0)
        {
            currentAmmo = magCapacity;
            ammoPool -= magCapacity;
            ammoPool = Mathf.Clamp(ammoPool, 0, 1000);
            gunAnimationCont.callReloadAnimation();
        }
        else
        {
            if(ammoPool != 0)
            {
                currentAmmo = ammoPool;
                ammoPool = 0;
            }
        }
       

        
    }

    public void bash()
    {
        gunAnimationCont.callBashAnimation();
        startBash = true;
    }

    private void cycleCycle()
    {
        if (!shootReady)
        {
            elapseCycleTime++;
            elapseCycleTime = Mathf.Clamp(elapseCycleTime , 0 , totalCycleTime);

            if (elapseCycleTime >= ejectFrames)
            {
         //       ejectCasing();
            }
            
                  
                            //spawn bullet casing
                            
                   

               

            

            if (elapseCycleTime == totalCycleTime)
            {
                elapseCycleTime = 0;
                shootReady = true;
            }
        }
    }

    private void ejectCasing()
    {
        Debug.Log("ejecting casing");
    }
}
