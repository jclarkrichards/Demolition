using UnityEngine;
using System.Collections;

public class SlingShot : MonoBehaviour
{
    static public SlingShot S; //Singleton
    public GameObject launchPoint;
    public GameObject projectile;
    public GameObject prefabProjectile;
    //public bool ______________;
    public Vector3 launchPos;
    public bool aimingMode;
    public float velocityMult = 4f;
    

    void Awake()
    {
        S = this;
        launchPoint = GameObject.Find("LaunchPoint");
        launchPoint.SetActive(false);
        launchPos = launchPoint.transform.position;
        print(launchPos);
    }

	// Use this for initialization
	void Start ()
    {
      
        
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(aimingMode)
        {
            //Convert mouse to world coordinates
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0f;
            //Find the difference from the launchPos to the mouse Position
            Vector3 diff = mousePos - launchPos;
            //Limit the diff to be within the radius of the slingshot collider
            float maxDiff = this.GetComponent<SphereCollider>().radius;
            if(diff.magnitude > maxDiff)
            {
                diff.Normalize();
                diff *= maxDiff;
            }
            
            //Move the projectile to this new position
            Vector3 projPos = launchPos + diff;
            projectile.transform.position = projPos;
            //What happens when the mouse is now released?
            if (Input.GetMouseButtonUp(0))
            {
                aimingMode = false;
                projectile.GetComponent<Rigidbody>().isKinematic = false;
                projectile.GetComponent<Rigidbody>().velocity = -diff * velocityMult;
                FollowCam.S.poi = projectile;  //follow the projectile as it flies
                projectile = null;
                MissionDemolition.ShotFired();
                
            }
        }
	
	}

    void OnMouseEnter()
    {
        //print("Entering Slingshot area");
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        //print("Leaving Slingshot area");
        launchPoint.SetActive(false);
    }

    void OnMouseDown() //Mouse is over the slingshot and the mouse button is pressed
    {
        aimingMode = true;
        //Instantiate(prefabProjectile, launchPos, Quaternion.identity);
        projectile = Instantiate(prefabProjectile) as GameObject;
        projectile.transform.position = launchPos;
        projectile.GetComponent<Rigidbody>().isKinematic = true;
        print(projectile.transform.position);

    }
}
