using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Diagnostics;
using System;
using System.Text.RegularExpressions;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.UI;

using Unity.Barracuda;

public class SlipperyIceAgent : Agent
{
    public string mapFile;
    public Text rewardText;

    Rigidbody2D m_Rigidbody;

    int checkpointsPassed;
    int checkpointCount;

    public GameObject checkPoint;
    public Vector3 startPos;

    public GameObject velocityCube;
    public Vector2 velocityCubeStartPosition;

    public float checkPointReward;
    public float wallReward;
    public float checkPointProximityReward;
    public float lapReward;
    public float lapTimerReward;
    public float distanceRewardPoint;

    public Button restartButton;
    public Button loadAttributes;
    public InputField environmentName;
    public InputField userName;
    
    public Dropdown existingEnvironments;
    public Dropdown existingProfiles;
    public Dropdown existingModels;

    public string[] maps;
    public string[] profiles;
    public Button loadMapProfilesButton;
    public bool isTraining;


    public Button loadModelButton;
    public NNModel model;
    //public Model modelLoaded;

    public void setScoringUp(string mapFile, InputField userName)
    {
        this.mapFile = mapFile;
        this.userName = userName;
    }

    public override void OnEpisodeBegin()
    {   
        
        m_Rigidbody = GetComponent<Rigidbody2D>();
        checkpointsPassed = 0;
        velocityCubeStartPosition = new Vector2(velocityCube.transform.localPosition[0], velocityCube.transform.localPosition[1]);
    }

    public void setAttributes(Vector3 startPos, int checkpointCount)
    {
        this.startPos = startPos;
        this.checkpointCount = checkpointCount;
    }

    void Start()
    {
        
        
        //UnityEngine.Debug.Log("loaded all attributes");

        /*

        checkPointReward = 0;
        wallReward = 0;
        checkPointProximityReward = 0;
        lapReward = 0;
        lapTimerReward = 0;
        distanceRewardPoint = 0;
        */



        try{
			restartButton.GetComponent<Button>().onClick.AddListener(resetSystem);
		} catch {
			//UnityEngine.Debug.Log("no reset button");
		}

        
        /*
        try{
			loadModelButton.GetComponent<Button>().onClick.AddListener(loadModel);
		} catch {
			//UnityEngine.Debug.Log("loadModel button error");
		}
        */
        try{
            loadMapProfilesButton.GetComponent<Button>().onClick.AddListener(loadMapProfiles);
        } catch {
            //UnityEngine.Debug.Log("error with load map profiles button");
        }

        this.GetComponent<Timer>().startTiming();
        
    }

    void loadMapProfiles()
    {
        //UnityEngine.Debug.Log("Map profiles loaded from agent");
        profiles = File.ReadAllLines(Application.dataPath + "/" + maps[existingEnvironments.value] + "/profiles.json");
        List<string> options = new List<string>();
        for (int i=0; i<profiles.Length; i++)
        {
            options.Add(profiles[i]);
        }
        existingProfiles.ClearOptions();
        existingProfiles.AddOptions(options);
    }

    public void updateAttributes(string map, string profile)
    {
        //This needs to also change
        string attributesFile = File.OpenText(Application.dataPath + "/" + map + "/" + profile + "/attributes.json").ReadToEnd();
        string[] attributesString = attributesFile.Split('\n');

        string[] splitAttribute = attributesString[0].Split(',');
        wallReward = float.Parse(splitAttribute[2]);

        splitAttribute = attributesString[1].Split(',');
        checkPointReward = float.Parse(splitAttribute[2]);

        splitAttribute = attributesString[2].Split(',');
        UnityEngine.Debug.Log(splitAttribute[2]);
        checkPointProximityReward = float.Parse(splitAttribute[2]);
        UnityEngine.Debug.Log(splitAttribute[3]);
        distanceRewardPoint = float.Parse(splitAttribute[3]);

        splitAttribute = attributesString[3].Split(',');
        lapReward = float.Parse(splitAttribute[2]);

        splitAttribute = attributesString[4].Split(',');
        lapTimerReward = float.Parse(splitAttribute[2]);
    }

    
    void Update()
    {
        rewardText.text = GetCumulativeReward().ToString();
        float laptime = this.GetComponent<Timer>().getTime();
        //resets if more than 60 seconds
        if(laptime > 60)
        {
            resetSystem();
        }
        //UnityEngine.Debug.Log(this.GetComponent<iceView>().getNextCheckpointLocation(checkpointsPassed));
        //AddReward(1f); 
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {
        
        float[] wallDistances = this.GetComponent<iceView>().wallDistances();
        sensor.AddObservation(wallDistances);


        //adds the current speed of the rigedbody
        //velocityCube.transform.localPosition = velocityCubeStartPosition + (m_Rigidbody.velocity / 2);
        
        Vector2 velocity = new Vector2((int)m_Rigidbody.velocity[0], (int)m_Rigidbody.velocity[1]);

        //UnityEngine.Debug.Log(velocity);
        sensor.AddObservation(m_Rigidbody.velocity);

        //adds the position of the next checkpoint
        Vector2 distanceToNearestCheckPoint = this.GetComponent<iceView>().getNextCheckpointLocation(checkpointsPassed);

        
        //distance based on position
        float distance = Mathf.Sqrt((distanceToNearestCheckPoint[0] * distanceToNearestCheckPoint[0]) + (distanceToNearestCheckPoint[1] * distanceToNearestCheckPoint[1]));

        //calculates a reward based on 
        float rewardPercent = 0;
        if(distanceRewardPoint != 0)
        {
            if(distance < distanceRewardPoint)
            {
                rewardPercent= distanceRewardPoint - distance / distanceRewardPoint;
            }
            else
            {
                rewardPercent = distanceRewardPoint - distance / 3605 - distanceRewardPoint;
            } 
        }
        AddReward((rewardPercent * checkPointProximityReward) / 100);
        
       
        sensor.AddObservation(this.GetComponent<iceView>().getNextCheckpointLocation(checkpointsPassed));
        
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        Vector3 force = new Vector3((actionBuffers.DiscreteActions[0] - 1) * 400, (actionBuffers.DiscreteActions[1] - 1) * 400, 0);
        m_Rigidbody.AddForce(force);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            //UnityEngine.Debug.Log(wallReward + " is the wall Reward");
            AddReward(wallReward);
            resetSystem();
        } 
        if (collision.tag == "CheckPoint")
        {
            int testing = 1;
            //UnityEngine.Debug.Log(collision.gameObject.name + "Checkpoint (" + checkpointsPassed.ToString() + ")");
            UnityEngine.Debug.Log(checkPointReward + " is the checkpoint reward");
            if(collision.gameObject.name == "Checkpoint (" + checkpointsPassed.ToString() + ")"){
                AddReward(checkPointReward);
                checkpointsPassed = checkpointsPassed + 1;
                testing = 2;
                

                //need to change to total checkpoints
                if(checkpointsPassed == checkpointCount)
                {
                    testing = 3;
                    AddReward(lapReward);
                    float laptime = this.GetComponent<Timer>().getTime();
                    AddReward(lapTimerReward/laptime);
                    if(isTraining == false)
                    {
                        addScore(laptime);
                    }
                    
                    resetSystem();
                }
            }
            //UnityEngine.Debug.Log(testing); 
        }
        
    }

    public void addScore(float laptime)
    {
        //this.userName = userName;

        string[] individualScores = File.ReadAllLines(Application.dataPath + "/" + mapFile  + "/leaderboard.json");
        int pos = 0;
        //for each score
        for(int i = 0; i < individualScores.Length; i++)
        {
            
            //if the laptime is larger than the one comparing it to
            string[] scoreBreakdown = individualScores[i].Split(',');
            try
            {
                if(float.Parse(scoreBreakdown[1]) < laptime)
                {
                    pos = pos + 1;
                }
            }
            catch
            {
                pos = i;
            }
            
        }

        //adding the scores back to the array
        string[] newScores = new string[individualScores.Length + 1];
        for(int i=0; i< pos; i++)
        {   
            newScores[i] = individualScores[i];
        }

        if(userName.text != "")
        {
            newScores[pos] = userName.text + "," + laptime.ToString();
        }
        else
        {
            newScores[pos] = "blank" + "," + laptime.ToString();
        }
        
        for(int i=pos; i< individualScores.Length; i++)
        {   
            newScores[i+1] = individualScores[i];
        }

        //converting to a string to write back
        string newScoreString = "";
        for(int i=0; i<newScores.Length; i++)
        {
            newScoreString = newScoreString + newScores[i] + "\n";
        }
        
        //This needs to change
        UnityEngine.Debug.Log(newScoreString);
        File.WriteAllText(Application.dataPath + "/" + mapFile  + "/leaderboard.json", newScoreString);
        

    }

    public void resetSystem()
    {
        try
        {
            transform.localPosition = startPos;
        } 
        catch 
        {
            transform.localPosition = new Vector3(-3, -6, 0);
        }
        m_Rigidbody.velocity = new Vector3(0,0,0);
        this.GetComponent<Timer>().resetTime();
        checkpointsPassed = 0;
        EndEpisode();
    }
}