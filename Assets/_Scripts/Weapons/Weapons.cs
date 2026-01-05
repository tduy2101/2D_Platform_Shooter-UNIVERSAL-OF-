using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public int weaponLevel;
    public  List<WeaponStats> stats;

    public int damage;
    public float speed;
    public float size;
    public float amount;
    public float range;

    [System.Serializable] 
    public class WeaponStats {
        public int damage;
        public float speed;
        public float size;
        public float amount;
        public float range;
    }

}
