using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieController : MonoBehaviour
{
    public float radiusFromTarget;
    public bool aiActive, fakebutton;
    private bool fakebuttonprev;
    public GameObject player;

    public enum switchTargetState { pointer , player};
    public switchTargetState targetState;

    // Start is called before the first frame update
    void Start()
    {
        updateZombies();
    }

    // Update is called once per frame
    void Update()
    {

        if(fakebutton != fakebuttonprev)
        {
            updateZombies();
            fakebutton = false;
        }
        fakebuttonprev = fakebutton;
    }

    void updateZombies()
    {
        Debug.Log("updated zombies");
        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).GetComponent<zombieAI>())
            {
                zombieAI _zombie = gameObject.transform.GetChild(i).GetComponent<zombieAI>();

                _zombie.target = targetStateGameObject().transform;
                _zombie.radiusFromTarget = radiusFromTarget;

            }
        }
    }

    GameObject targetStateGameObject()
    {
        switch (targetState)
        {
            case switchTargetState.player:
                return player;
                break;
                return player;
                break;
        }

        return player;
    }
}
