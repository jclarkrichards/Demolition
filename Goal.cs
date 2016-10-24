using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour
{
    static public bool goalMet = false;

    void OnTriggerEnter(Collider other)
    {
        //When the trigger is triggered
        //Check to see if hit by a projectile
        if (other.gameObject.tag == "Projectile")
        {
            goalMet = true;
            // change the alpha of the material to show that you hit the goal
            Color c = GetComponent<Renderer>().material.color;
            c.a = 1;
            
            GetComponent<Renderer>().material.color = c;
        }
    }
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
