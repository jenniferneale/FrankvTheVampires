using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ungamed.Dismember;

namespace Frank
{

    public class LimbStats : MonoBehaviour
    {
        public DAMAGETYPE bodyPart;
        public float speedMod = 0; //Leg++, Arm+
        public float accMod = 0; //Head+
        public float constMod = 1; //Torso+++, all others+
        public float damageMod = 1; //Foot++, Hand++, all others+

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GenerateStats(GenericDismembering gD)
        {
            bodyPart = gD.bodyPart;
            if (gD != null)
            {
                float percent = (((GenericDismembering)gD).initialHealth - gD.baseHealth) / gD.higherHealthRange;
                switch (gD.bodyPart)
                {
                    case DAMAGETYPE.BODY:
                        constMod += percent * gD.higherHealthRange;
                        break;
                    case DAMAGETYPE.LEG:
                        speedMod += percent * gD.higherHealthRange;
                        break;
                    default:
                        return;
                }
            }
            else print("Using GenerateStats on LimbStats on GameObj that has no GenericDismembering");
        }
    }
}