using UnityEngine;
using System.Collections;

public class CloudCrafter : MonoBehaviour
{
    public int numClouds = 40;
    public GameObject[] cloudPrefabs; //5 prefabs to choose from
    public Vector3 cloudPosMin;
    public Vector3 cloudPosMax;
    public float cloudScaleMin = 1;
    public float cloudScaleMax = 5;
    public float cloudSpeedMult = 0.5f;
    public GameObject[] cloudInstances;

    void Awake()
    {
        //Make array to hold all the clouds
        cloudInstances = new GameObject[numClouds];
        //Find the CloudAnchor parent
        GameObject anchor = GameObject.Find("CloudAnchor");
        //Make all of the clouds
        GameObject cloud;
        for (int i=0; i<numClouds; i++)
        {
            //Randomly choose a cloud type
            int prefabNum = Random.Range(0, cloudPrefabs.Length);
            //Make an instance of the chosen cloud type
            cloud = Instantiate(cloudPrefabs[prefabNum]) as GameObject;
            //Position the cloud
            Vector3 pos = Vector3.zero;
            pos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            pos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //Scale the cloud
            float scaleU = Random.value; //between 0 and 1
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //Smaller clouds should be closer to the ground
            pos.y = Mathf.Lerp(cloudPosMin.y, pos.y, scaleU);
            //smaller clouds should be further away
            pos.z = 100 - 90 * scaleU;
            //Apply to the cloud instance
            cloud.transform.position = pos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            //Make the cloud a child of the anchor
            cloud.transform.parent = anchor.transform;
            //Add the cloud to the array of cloud instances
            cloudInstances[i] = cloud;

        }
    }

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Iterate over all of the clouds
        foreach(GameObject cloud in cloudInstances)
        {
            //Get the clouds scale and position
            float scaleVal = cloud.transform.localScale.x;
            Vector3 pos = cloud.transform.position;
            //Move clouds to the left, Move larger clouds faster
            pos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;
            //If cloud moves too far to the left, then wrap it around to the right
            if (pos.x <= cloudPosMin.x)
            {
                pos.x = cloudPosMax.x;
            }
            //Apply the position change to the cloud
            cloud.transform.position = pos;
        }
	
	}
}
