using UnityEngine;
using System.Collections;



public class CameraFollowsPlayer : MonoBehaviour {

    public GameObject objToFollow;

    // Use this for initialization
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) objToFollow = player;
    }

    // Update is called once per frame
    void Update()
    {
        if (objToFollow != null)
        {
            Vector3 pos = objToFollow.transform.position;
            this.transform.position = new Vector3(pos.x, pos.y + 5, pos.z - 5);
            /*transform.position = (new Vector3(
   Mathf.Cos(35),
   Mathf.Sin(0),
   Mathf.Sin(0)
));*/
            transform.forward = objToFollow.transform.position - transform.position;
            this.transform.rotation = Quaternion.Euler(new Vector3(35,0,0));
            
        }
        else print("no object for camera to follow");
    }
}
