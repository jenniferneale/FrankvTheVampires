using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

namespace Frank
{

    public class PlayerMovement : MonoBehaviour
    {
        public float velo = 400;
        public float turnSpeed = 40;
        DateTime lastJumpTime = new DateTime(1999, 12, 1);
        float modifier = 0;
        float panSpeed = 50;
        bool isDead = false;
        SkinnedMeshRenderer renderer;
        public GameObject attackTarget;
        float attackDamage = 2;
        public float health = 10;
        Inventory inventoryScript;
        public Rigidbody rg;
        Animator animator;
        // Use this for initialization
        void Start()
        {
            renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            inventoryScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Inventory>();
            rg = this.GetComponent<Rigidbody>();
            animator = this.GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            float horz = Input.GetAxis("Horizontal");
            float vert = Input.GetAxis("Vertical");
            bool leftPan = Input.GetKey(KeyCode.Q);
            bool rightPan = Input.GetKey(KeyCode.E);
            
            rg.AddForce(transform.forward * velo * (float)Math.Round(vert));
            if (vert != 0) animator.SetBool("walk", true);
            if (rg.velocity == Vector3.zero) animator.SetBool("walk", false);
            if(horz != 0)
            {
                transform.Rotate(0, horz * velo * Time.deltaTime, 0);
            }
            else rg.angularVelocity = Vector3.zero;
            //If player has fallen well below ground level, restart the level
            if (this.transform.position.y < -20)
            {
                print("player fell off");
                Die();
            }

            if (Grounded(rg) && Input.GetKeyDown(KeyCode.Space))
            {
                print("space pressed");
                attackTarget.SendMessage("Damage", attackDamage);
                print(this.name + " damaged " + attackTarget.gameObject.name + " for " + attackDamage);
                animator.SetBool("attack", true);
                lastJumpTime = DateTime.Now;
                StartCoroutine(TurnOffAttack());
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                print("Pressed M for Monster!");
                if(inventoryScript.inv.Count > 0)
                {
                    inventoryScript.GenerateMonster(inventoryScript.inv);
                    inventoryScript.inv = new System.Collections.Generic.List<LimbStats>();
                }
                
            }

            }

        bool Grounded(Rigidbody rg)
        {
            DateTime curr = DateTime.Now;
            TimeSpan elapsed = curr - lastJumpTime;
            return elapsed.Seconds > 1 || rg.velocity.y == 0;
        }

        IEnumerator TurnOffAttack()
        {
            yield return new WaitForSeconds(.6f);
            animator.SetBool("attack", false);
        }

        public void Damage(float amount)
        {
            if (isDead) return;
            health -= amount;

            if (health <= 0)
            {
                health = 0f;
                Die();
            }
            StartCoroutine(Flasher(renderer));
        }

        IEnumerator Flasher(SkinnedMeshRenderer renderer)
        {
            foreach(Material m in renderer.materials) m.color = Color.red;
            yield return new WaitForSeconds(.1f);
            foreach (Material m in renderer.materials) m.color = Color.grey;
        }

        public void Die()
        {
            if (isDead) return;
            isDead = true;
            int c = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(c);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Trigger") attackTarget = other.gameObject.transform.parent.gameObject;
            else attackTarget = other.gameObject;
        }
    }
}