using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponCore : MonoBehaviour
{
    public int currentWeaponID;
    public weaponLibrary weaponLib;
    public gunAnimationController gunAnimationCont;
    public GameObject weaponBarrel ,bulletHit;
    public bool triggerDown , shootReady;
    public int elapseCycleTime , ejectFrames , totalCycleTime;
    public float baseAimConeAccuracy, moveAimConeAccuracy , currentAimCone;
    public float finalAimCone;
    public float maxShotDistance;
    public int currentAmmo, magCapacity , ammoPool;
    public int bashDuration, bashStep;
    public Vector3 shootAfterSpreadDirection;
    public int weaponDamage;
    public float bashDistance , bashForce;
    public float recoilStacks;
    public float stability ,recoilBase , currentRecoil;
    public int recoilStacksStep, recoilTime;
    public enum reState { no , eject , insert }
    public reState gunState;
    public bool startBash;
    public int reloadStep, reloadTime, reloadInsertTime;
    public Vector3 forwardVector;
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
   



        Debug.DrawRay(weaponBarrel.transform.position, weaponBarrel.transform.forward, Color.black);
        if(gunState == reState.no)
        {
            tryFireWeapon();
            cycleCycle();
        }
        
        gunBash();

        switch(gunState)
        {

            case reState.eject:
                reloadStep++;
                if (reloadStep >= reloadInsertTime) 
                {
                    gunState = reState.insert;
                    actuallyReload();
                }
                
                break;
            case reState.insert:
                reloadStep++;
                if (reloadStep == reloadTime)
                {
                    reloadStep = 0;
                    gunAnimationCont.callReloadEndAnimation();
                    gunState = reState.no;
                }
                    break;

        }

        handleRecoil();
        finalAimCone = currentAimCone + recoilStacks;

    }

    public void setAimCone(float deviation)
    {
        currentAimCone = deviation;
    }

    public float getGunAccuracyNow(float baseAcc , float maxAcc)
    {
        finalAimCone = Mathf.Clamp(finalAimCone, 0, moveAimConeAccuracy);
        return ((finalAimCone/moveAimConeAccuracy)*(maxAcc - baseAcc));
    }

    public void handleRecoil()
    {
        if(recoilStacks > 0)
        {
            //doRecoil
            recoilStacksStep++;
            recoilStacksStep = Mathf.Clamp(recoilStacksStep, 0, recoilTime);
            if(recoilStacksStep == recoilTime)
            {
                recoilStacks -= stability;
                recoilStacks = Mathf.Clamp(recoilStacks, 0, 999);
                recoilStacksStep = 0;
            }
        }
    }

    public void gunBash()
    {
        if(startBash)
        {
            if(gunState == reState.insert)
            {
              
                    reloadStep = 0;
                    gunAnimationCont.callReloadEndAnimation();
                    gunState = reState.no;
                
            }
            bashStep++;
            bashStep = Mathf.Clamp(bashStep, 0, bashDuration);
            RaycastHit bashHit;
            if(Physics.Raycast(weaponBarrel.transform.position, transform.forward, out bashHit, bashDistance))
            {
                switch (bashHit.collider.gameObject.tag)
                {
                    case "zombieHead":

                        bashHit.collider.gameObject.GetComponent<zombieReferenceComp>().zombie.doStun(bashForce);
                        break;
                    case "zombieBody":

                        bashHit.collider.gameObject.GetComponent<zombieReferenceComp>().zombie.doStun(bashForce);
                        break;
                }
            }
            Debug.DrawRay(weaponBarrel.transform.position, weaponBarrel.transform.forward , Color.cyan);

            

            if(bashStep == bashDuration)
            {
                bashStep = 0;
                startBash = false;
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

        
        forwardVector = Vector3.forward;
        float deviation = Random.Range(0f, finalAimCone);
        float angle = Random.Range(0f, 360f);
        forwardVector = Quaternion.AngleAxis(deviation, Vector3.up) * forwardVector;
        forwardVector = Quaternion.AngleAxis(angle, Vector3.forward) * forwardVector;
        forwardVector = Camera.main.transform.rotation * forwardVector;

        recoilStacks += recoilBase;


        //Calculate Direction with Spread
        //Vector3 shootAfterSpreadDirection = weaponBarrel.transform.forward + new Vector3(-x, -y, -z);

        RaycastHit weaponHit;
        if (Physics.Raycast(Camera.main.transform.position, forwardVector, out weaponHit, maxShotDistance))
        {
            GameObject hitPoint = Instantiate(bulletHit , weaponHit.point, Quaternion.identity);
            hitPoint.transform.SetParent(weaponHit.transform);
            switch (weaponHit.collider.gameObject.tag) 
            {
                case "zombieHead":

                    weaponHit.collider.gameObject.GetComponent<zombieReferenceComp>().zombie.takeDamage(weaponDamage , 100);
                    break;
                case "zombieBody":
                    weaponHit.collider.gameObject.GetComponent<zombieReferenceComp>().zombie.takeDamage(weaponDamage , 1);
                    break;
            }
            
               
            
        }

        
      
        

    

    }

    public void reload()
    {
        if(ammoPool - magCapacity > 0)
        {
            gunAnimationCont.callReloadAnimation();
            gunState = reState.eject;
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

    public void actuallyReload()
    {
        currentAmmo = magCapacity;
        ammoPool -= magCapacity;
        ammoPool = Mathf.Clamp(ammoPool, 0, 1000);
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
