using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frank
{

    public class Roll : MonoBehaviour
    {
        public float torque = 12;
        public float velo = 10;
        public Transform target;
        public Transform objToMove;
        Rigidbody rg;
        MonsterStats monStats;

        // Use this for initialization
        void Start()
        {
            rg = this.GetComponent<Rigidbody>();
            objToMove = this.transform;
            if (rg == null)
            {
                rg = this.GetComponentInParent<Rigidbody>();
                objToMove = this.transform.parent.transform;
            }
            monStats = this.GetComponentInParent<MonsterStats>();
        }

        // Update is called once per frame
        void Update()
        {
            
            if (rg)
            {
                Vector3 targetDir = target.position - transform.position;
                //float step = torque /* * 2 * Time.deltaTime*/;
                //Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir /*- new Vector3(0, targetDir.y, 0)*/, step, 0.0F);
                //objToMove.rotation = Quaternion.LookRotation(newDir);
                //rg.AddTorque(transform.forward * torque);
                rg.AddTorque(targetDir * torque + new Vector3(1,1,1));
                rg.AddForce(targetDir * velo);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.name.Contains("Zombie")) target = other.transform;
        }
        
    }
}