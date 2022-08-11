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

public class Leaderboard : MonoBehaviour
{
    public Dropdown existingEnvironments;
    public Text leaderboard;
    public Button loadButton;
    public string[] maps;
    // Start is called before the first frame update
    void Start()
    {

        maps = File.ReadAllLines(Application.dataPath + "/maps.json");
        List<string> options = new List<string>();
        for (int i=0; i<maps.Length; i++)
        {
            options.Add(maps[i]);
        }
        existingEnvironments.ClearOptions();
        existingEnvironments.AddOptions(options);

        try{
			loadButton.GetComponent<Button>().onClick.AddListener(loadMapLeaderboard);
		} catch (Exception e){
			UnityEngine.Debug.Log("loadModel button error");
		}
        
    }

    void loadMapLeaderboard()
    {
        try
        {
            string[] individualScores = File.ReadAllLines(Application.dataPath + "/" + maps[existingEnvironments.value]  + "/leaderboard.json");
            string leaderboardString = "";
            for(int i=0; i<individualScores.Length; i++)
            {
                string[] temp = individualScores[i].Split(',');
                leaderboardString = leaderboardString + temp[0] + ":     " + temp[1] + "\n";
            }
            leaderboard.text = leaderboardString;
        }
        catch
        {
            UnityEngine.Debug.Log("what");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
