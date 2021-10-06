using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponLibrary : MonoBehaviour
{
    public enum weaponType { pistol, shotgun, smg, rifle };
    public enum weaponArchetype { highfire, slowfire, special };
    public enum cycleType { auto, semi, pump, revolver, rpg };

    public class weapon
    {
        public weaponType type;
        public weaponArchetype archetype;
        public int ejectFrames, cycleFrames, maxRecoil, upRecoilFrames, downRecoilFrames, exitVelocity;
        public int magSize, totReloadFrames, startFrames, reloadPercent;
        public int baseDamage, shotCount, shellAmount, shotSpreadAngle, damage;
        public int penetrationAmount;
        public string gunname;
        public cycleType cycleType;
        public GameObject shell, bulletType;
    }
}
