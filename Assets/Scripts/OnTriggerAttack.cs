using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Frank
{

    public class OnTriggerAttack : MonoBehaviour
    {
        private Animator animator;
        DateTime lastAttackTime = new DateTime(1999, 12, 1);
        MonsterStats stats;
        GameObject parent;
        public Transform target = null;
        List<GameObject> enemiesInRange = new List<GameObject>();

        // Use this for initialization
        void Start()
        {
            animator = this.GetComponentInParent<Animator>();
            stats = this.GetComponentInParent<MonsterStats>();
            parent = this.transform.parent.gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            if (CanAttack())
            {
                if (target == null) {
                    enemiesInRange.RemoveAll(isNull);
                    if (enemiesInRange.Count > 0)
                    {
                        if (enemiesInRange[0] == null) print("found null: " + enemiesInRange[0] + ", this: " + parent.name);
                        target = enemiesInRange[0].transform;
                    }
                    else
                    {
                        if (animator != null && animator.GetInteger("distance") == 0)
                            animator.SetInteger("distance", 2);
                        return;
                    }
                }                    
                float amount = 0;
                if (parent.tag == "Enemy") amount = 1;
                else if (stats != null) amount = stats.damageMod;
                target.SendMessage("Damage", amount);
                print(parent + " damaged " + target.gameObject.name + " for " + amount);
                lastAttackTime = DateTime.Now;
            }
        }

        private bool isNull(GameObject o)
        {
            return o == null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Ground") return;
            if (parent.tag == "Enemy" && other.gameObject.tag == "Limb") return;
            GameObject oth = other.gameObject;
            if (other.gameObject.tag == "Trigger") oth = oth.transform.parent.gameObject;
            if (!(parent.tag == "Monster" && oth.tag == "Player"))
            {
                if(animator != null) animator.SetInteger("distance", 0);
                enemiesInRange.Add(oth);
                if ((parent.tag == "Monster" && oth.tag == "Enemy") ||
            (parent.tag == "Enemy" && oth.tag == "Monster")) target = oth.transform;
                else if (target == null) target = oth.transform;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            GameObject oth = other.gameObject;
            if (other.gameObject.tag == "Trigger") oth = oth.transform.parent.gameObject;
            if (enemiesInRange.Contains(oth))
            {
                enemiesInRange.Remove(oth);
                if (target == oth.transform)
                {
                    enemiesInRange.RemoveAll(isNull);
                    if (enemiesInRange.Count > 0) target = enemiesInRange[0].transform;
                    else target = null;
                }
                if (enemiesInRange.Count <= 0 && animator != null) animator.SetInteger("distance", 1);
            }
        }



        bool CanAttack()
        {
            DateTime curr = DateTime.Now;
            TimeSpan elapsed = curr - lastAttackTime;
            return elapsed.Seconds > 1;
        }
    }
}