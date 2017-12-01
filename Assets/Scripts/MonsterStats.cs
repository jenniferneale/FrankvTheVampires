using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frank
{

    public class MonsterStats : MonoBehaviour
    {

        public float speedMod = 0; //Leg++, Arm+
        public float accMod = 0; //Head+
        public float healthMod = 0; //Torso+++, all others+
        public float damageMod = 0; //Foot++, Hand++, all others+
        
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Damage(float amount)
        {
            healthMod -= amount;
            if (healthMod <= 0) Die();
        }

        public void Die()
        {
            print("Monster died "+ this.transform.parent.gameObject.name);
            Destroy(this.transform.parent.gameObject);
        }
    }

}