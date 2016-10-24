using UnityEngine;
using System.Collections;

public enum GameMode
{
    idle,
    playing,
    levelEnd,

}

public class MissionDemolition : MonoBehaviour
{
    static public MissionDemolition S; //Singleton
    public GameObject[] castles;  //an array of castles
    public Vector3 castlePos;  //The locations to place the castles
    public int level;  //The current level
    public int levelMax;  //The number of levels
    public int shotsTaken;
    public GameMode mode = GameMode.idle;
    public string showing = "Slingshot";  //FollowCam mode
    public GameObject castle;  //The current castle

	// Use this for initialization
	void Start ()
    {
        S = this;
        level = 0;
        levelMax = castles.Length;
        StartLevel();
	
	}

    void StartLevel() //Call everytime a new level starts
    {
        //Get rid of the old castle if one exists
        if (castle != null)
        {
            Destroy(castle);
        }

        //Destroy old projectiles if they exist
        GameObject[] gos = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject g in gos)
            Destroy(g);

        //Instantiate the new castle
        castle = Instantiate(castles[level]) as GameObject;
        castle.transform.position = castlePos;
        shotsTaken = 0;

        //Reset the camera
        //SwitchView("Both");
        ProjectileLine.S.Clear();

        //Reset the goal
        Goal.goalMet = false;

        mode = GameMode.playing;
    }
	
	// Update is called once per frame
	void Update ()
    {
        //Check for level end
        if (mode == GameMode.playing && Goal.goalMet)
        {
            //Change mode to levelEnd
            mode = GameMode.levelEnd;
            //Zoom out
            //SwitchView("Both");
            //Start  the next level in 2 seconds
            Invoke("NextLevel", 2f);
        }
	
	}

    void NextLevel()
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }

    //Static method that allows code anywhere to increment shots taken
    public static void ShotFired()
    {
        S.shotsTaken++;
    }
}
