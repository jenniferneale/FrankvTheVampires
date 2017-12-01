using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frank
{

    public class OnTriggerWalk : MonoBehaviour
    {
        public Animator animator;
        public Transform target;
        
        public GameObject parent;
        public int speed = 5;
        List<GameObject> targetsInRange = new List<GameObject>();

        //For Monster
        public float torque = 12;
        public float velo = 10;
        MonsterStats monStats;
        public Rigidbody rg;
        

        // Use this for initialization
        void Start()
        {
            animator = this.GetComponentInParent<Animator>();
            parent = this.transform.parent.gameObject;
            if (parent.tag == "Enemy") target = GameObject.FindWithTag("Player").transform;
            monStats = this.GetComponentInParent<MonsterStats>();
            rg = this.GetComponent<Rigidbody>();
            if (rg == null) rg = this.GetComponentInParent<Rigidbody>();
            }

        // Update is called once per frame
        void Update()
        {
            if (rg && parent.tag == "Monster")
            {
                if (target == null)
                {
                    targetsInRange.RemoveAll(item => item == null);
                    if (targetsInRange.Count > 0)
                    {
                        if (targetsInRange[0] == null) print("found null: " + targetsInRange[0] + ", this: " + this.gameObject.name);
                        target = targetsInRange[0].transform;
                    }
                    else target = GameObject.FindWithTag("Player").transform;
                }
                   
                Vector3 targetDir = target.position - transform.position;
                rg.AddTorque(targetDir * torque + new Vector3(1, 1, 1));
                rg.AddForce(targetDir * velo * Time.deltaTime);
            }
            else if (animator.GetInteger("distance") == 1)
            {
                Vector3 targetDir = target.position - transform.position;
                float step = speed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0F);
                parent.transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, parent.transform.position.y, target.position.z), step);
                parent.transform.rotation = Quaternion.LookRotation(newDir);
            }
            
        }

        private void OnTriggerEnter(Collider other)
        {//Monsters prefer to follow Enemies; Enemies prefer to follow Monsters; Both will follow anyone
            if (other.gameObject.tag == "Ground") return;
            if (parent.tag == "Enemy" && other.gameObject.tag == "Limb") return;
            GameObject oth = other.gameObject;
            if (other.gameObject.tag == "Trigger") oth = oth.transform.parent.gameObject;
            if(animator != null) animator.SetInteger("distance", 1);
            targetsInRange.Add(oth);
            if ((parent.tag == "Monster" && oth.tag == "Enemy") ||
                (parent.tag == "Enemy" && oth.tag == "Monster")) target = oth.transform;
            else if (target == null) target = oth.transform;
        }

        private void OnTriggerExit(Collider other)
        {
            GameObject oth = other.gameObject;
            if (other.gameObject.tag == "Trigger") oth = oth.transform.parent.gameObject;
            if (targetsInRange.Contains(oth)) targetsInRange.Remove(oth);
            if (target == oth.transform)
            {
                if (targetsInRange.Count > 0) target = targetsInRange[0].transform;
                else target = null;
            }
            if (targetsInRange.Count <= 0 && animator != null)
            {
                animator.SetInteger("distance", 2);
            }
        }
    }
}