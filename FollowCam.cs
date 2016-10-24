using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour
{
    static public FollowCam S;  //A FollowCam Singleton.  
    public GameObject poi; //point of interest for camera to follow
    public float camZ; //desired Z pos of the camera
    public float easing = 0.05f;  //should be between 0 and 1
    public Vector2 minXY;

    void Awake()
    {
        S = this;
        camZ = transform.position.z;
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        Vector3 destination;
        //If there is no poi, return camera to [0,0,0] position
        if (poi == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            destination = poi.transform.position;
            //If poi is a projectile, check to see if it is at rest
            if (poi.tag == "Projectile")
            {
                if(poi.GetComponent<Rigidbody>().IsSleeping())
                {
                    poi = null;
                    return;
                }
            }
        }
        //Move camera to position of object, but leave z-axis alone.
        //destination = poi.transform.position;
        //Limit the camera's x and y values to minimum values
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        //Interpolate from current camera position to object position
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = camZ;
        transform.position = destination;
        //Set the orthographicSize of the camera to keep the ground in view
        Camera.main.orthographicSize = destination.y + 10;
	
	}
}
