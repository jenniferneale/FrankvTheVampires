using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using Ungamed.Dismember;

namespace Frank
{
    public enum limbFiles
    {
        [Description("Torso")]
        TORSO,
        [Description("Hip_L")]
        LLEG
    }

    public class Inventory : MonoBehaviour
    {
        public List<LimbStats> inv = new List<LimbStats>();
        public System.Random rand = new System.Random();
        

        // Use this for initialization
        void Start()
        {
            //GameObject torso = Instantiate(Resources.Load("Limbs Library/Torso")) as GameObject;
            //Add(torso);
            //limbsLibrary = Resources.LoadAll("Limbs Library",typeof(GameObject)) as GameObject[];
            //rand = new System.Random();
            
        }

        // Update is called once per frame
        void Update()
        {
           //Testy();
        }

        public void GenerateLootLimb(GenericDismembering gD)
        {
            print("gD body part " + gD.bodyPart);
            LimbStats ls = new LimbStats();
            ls.GenerateStats(gD);
            print("ls body part " + ls.bodyPart);
            inv.Add(ls);
            print("inv " + inv.Count);
        }

        public GameObject InstantiateLootLimb(LimbStats ls)
        {
            string prefabString = "";
            switch (ls.bodyPart)
            {
                case DAMAGETYPE.BODY:
                    prefabString = "Torso";
                    break;
                case DAMAGETYPE.LEG:
                    prefabString = "LLeg";
                    break;
                case DAMAGETYPE.THIGH:
                    prefabString = "LLeg";
                    break;
                case DAMAGETYPE.HAND:
                    prefabString = "LHand";
                    break;
                case DAMAGETYPE.HEAD:
                    prefabString = "Head";
                    break;
                case DAMAGETYPE.ARM:
                    prefabString = "RArm";
                    break;
                case DAMAGETYPE.FOOT://Not currently using these
                    prefabString = "LLeg";
                    break;
                default:
                    print("Limbs Library/" + prefabString);
                    break;
            }
            //print("Limbs Library/" + prefabString);
            GameObject g = Instantiate((GameObject)Resources.Load("Limbs Library/" +prefabString, typeof(GameObject)));
            g.name = "limb";
            return g;
        }

        public GameObject GenerateMonster(List<LimbStats> limbKeys)
        {
            GameObject monster = new GameObject("Monster");
            monster.transform.position = new Vector3(0, 0, 0);
            monster.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
            Rigidbody mrg = monster.AddComponent<Rigidbody>();
            //mrg.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            monster.AddComponent<MeshCollider>();
            monster.tag = "Monster";

            //Make and add walk trigger with Roll script
            GameObject walkTrigger = Instantiate((GameObject)Resources.Load("GeneralWalkTrigger"));
            walkTrigger.transform.SetParent(monster.transform,false);            

            //Add monster stats
            MonsterStats stats = monster.AddComponent<MonsterStats>();

            //Add attack trigger
            GameObject attackTrigger = Instantiate((GameObject)Resources.Load("AttackTrigger"));
            attackTrigger.transform.SetParent(monster.transform, false);

            //For each limb key in inventory, add a limb to the monster and adjust its stats
            for (int i=0; i<limbKeys.Count; i++)
            {
                LimbStats key = limbKeys[i];
                GameObject li = InstantiateLootLimb(key);
                li.transform.SetParent(monster.transform,false);
                li.transform.position = new Vector3(rand.Next(-25,25)*.01f, rand.Next(-25, 25) * .01f, rand.Next(-25, 25) * .01f);
                li.transform.rotation = Quaternion.Euler(new Vector3(rand.Next(0, 360), rand.Next(0, 360), rand.Next(0, 360)));
                Rigidbody r = li.transform.GetComponentInChildren<Rigidbody>();
                MeshCollider mc = li.transform.GetComponentInChildren<MeshCollider>();
                BoxCollider bc = li.transform.GetComponentInChildren<BoxCollider>();
                Destroy(r);
                Destroy(mc);
                Destroy(bc);
                stats.accMod += key.accMod;
                stats.speedMod += key.speedMod;
                stats.healthMod += key.constMod;
                stats.damageMod += key.damageMod;
            }
            monster.transform.position = (GameObject.FindWithTag("Player").transform.position + new Vector3(3, 0, 3));
            print("monster at " + monster.transform.position.x);
            return monster;
        }

        public bool Testy()
        {

            if (Input.GetKeyDown(KeyCode.Space))
            {
                print("space pressed");
                if(inv.Count > 1)
                {
                    GenerateMonster(inv);

                }else
                {
                    GameObject target = GameObject.FindGameObjectWithTag("TestTarget");
                    target.GetComponent<GenericDismembering>().SendMessage("ApplyDamage", 1f);
                }                
            }
            return Input.GetKeyDown(KeyCode.Space);
        }
    }//end class

}//end namespace