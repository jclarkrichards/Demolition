using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S;  //Singleton
    public float minDist = 0.1f;
    public LineRenderer line;
    GameObject _poi;
    public List<Vector3> points;

    void Awake()
    {
        S = this;
        //Get a reference to the LineRenderer
        line = GetComponent<LineRenderer>();
        //Disable the LineRenderer until it's needed
        line.enabled = false;
        //Initialize the list of points
        points = new List<Vector3>();
    }

    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                //When _poi is set to something new, it resets everything
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    public void Clear() //Can be used to clear the line directly
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint() //This is called to add a point to the line
    {
        Vector3 pt = _poi.transform.position;
        if(points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            //If the point isn't far enough from the last point, then no point in adding it
            return;
        }
        if (points.Count == 0)
        {
            //If this is the launch point...
            Vector3 launchPos = SlingShot.S.launchPoint.transform.position;
            Vector3 launchPosDiff = pt - launchPos;

            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.SetVertexCount(2);
            //Sets the first two points
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            //Enable the LineRenderer
            line.enabled = true;
        }
        else
        {
            //Normal behavior of adding a point
            points.Add(pt);
            line.SetVertexCount(points.Count);
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
        
    }

    //Returns location of the most recently added point
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                //If there are no points...
                return Vector3.zero;
            }
            return points[points.Count - 1];
        }
    }



	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if(poi == null)
        {
            //If there is no poi, search for one
            if (FollowCam.S.poi != null)
            {
                if (FollowCam.S.poi.tag == "Projectile")
                {
                    poi = FollowCam.S.poi;
                }
                else
                {
                    return; //Couldn't find a poi
                }
            }
            else
            {
                return; //Couldn't find a poi
            }
        }
        //If there is a poi, it's loc is added every FixedUpdate
        AddPoint();
        if(poi.GetComponent<Rigidbody>().IsSleeping())
        {
            poi = null;
        }
	
	}
}
