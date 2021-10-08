using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunAnimationController : MonoBehaviour
{

    public Animator weaponAnimator;
    public weaponAnimationLibrary weaponAnimLib;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void callShootAnimation()
    {
        weaponAnimator.Play("shoot");
    }

    public void callReloadAnimation()
    {
        weaponAnimator.Play("reload");
    }

    public void callReloadEndAnimation()
    {
        weaponAnimator.Play("reloadEnd");
    }
    public void callBashAnimation()
    {
        weaponAnimator.Play("bash");
    }

 }
