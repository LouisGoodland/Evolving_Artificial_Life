using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Diagnostics;
using System;
using System.Text.RegularExpressions;

public class Save : MonoBehaviour
{
    public Button loadButton;
    public GameObject wallPiece;
    public GameObject checkpointPiece;

    [SerializeField] public GameObject startPoint;
    
    public Dropdown existingEnvironments;
    string[] maps;

    void Start()
    {
        try{
			loadButton.GetComponent<Button>().onClick.AddListener(loadButtonPress);
		} catch (Exception e){
			UnityEngine.Debug.Log("load issue");
		}

        try
        {
            maps = File.ReadAllLines(Application.dataPath + "/maps.json");
            List<string> options = new List<string>();
            for (int i=0; i<maps.Length; i++)
            {
                options.Add(maps[i]);
            }
            existingEnvironments.ClearOptions();
            existingEnvironments.AddOptions(options);
        }
        catch
        {
            UnityEngine.Debug.Log("No dropdown load e.t.c.");
        }
        
    }
    
    void loadButtonPress()
    {
        
        string checkPointFile = File.OpenText(Application.dataPath + "/" + maps[existingEnvironments.value]  + "/Map/checkPointsString.json").ReadToEnd();
        string[] checkpointsString = checkPointFile.Split('\n');
        convertToObject(checkpointsString, "c");
        
        string wallPieceFile = File.OpenText(Application.dataPath + "/" + maps[existingEnvironments.value]  + "/Map/wallPiecesString.json").ReadToEnd();
        string[] wallPiecesString = wallPieceFile.Split('\n');
        convertToObject(wallPiecesString, "w");

        string startFile = File.OpenText(Application.dataPath + "/" + maps[existingEnvironments.value]  + "/Map/spawnString.json").ReadToEnd();
        string[] startString = startFile.Split(',');
        
        startPoint.transform.localPosition = new Vector3(float.Parse(startString[0]), float.Parse(startString[1]), float.Parse(startString[2]));
        startPoint.transform.parent = this.transform;

        //load doesn't work cause this
        this.transform.Find("ZoomSquare").GetComponent<SlipperyIceAgent>().setAttributes(startPoint.transform.localPosition, checkpointsString.Length - 1);
        
    }

    public void loadButtonPress2(string map)
    {
        
        string checkPointFile = File.OpenText(Application.dataPath + "/" + map  + "/Map/checkPointsString.json").ReadToEnd();
        string[] checkpointsString = checkPointFile.Split('\n');
        convertToObject(checkpointsString, "c");
        
        string wallPieceFile = File.OpenText(Application.dataPath + "/" + map  + "/Map/wallPiecesString.json").ReadToEnd();
        string[] wallPiecesString = wallPieceFile.Split('\n');
        convertToObject(wallPiecesString, "w");

        string startFile = File.OpenText(Application.dataPath + "/" + map  + "/Map/spawnString.json").ReadToEnd();
        string[] startString = startFile.Split(',');
        
        startPoint.transform.localPosition = new Vector3(float.Parse(startString[0]), float.Parse(startString[1]), float.Parse(startString[2]));
        startPoint.transform.parent = this.transform;

        //load doesn't work cause this
        this.transform.Find("ZoomSquare").GetComponent<SlipperyIceAgent>().setAttributes(startPoint.transform.localPosition, checkpointsString.Length - 1);
        
    }

    void convertToObject(string[] objectsArray, string type)
    {
        for(int i=0; i< objectsArray.Length; i++)
        {
            GameObject a;
            string[] objectAttributes =  objectsArray[i].Split(',');
            
            if(objectAttributes.Length > 2)
            {
                
                if(type == "c")
                {
                    a = Instantiate(checkpointPiece) as GameObject;
                    a.gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 1f, 0));
                
                } else 
                {
                    a = Instantiate(wallPiece) as GameObject;
                }
                
                a.transform.parent = this.transform;
                a.name = objectAttributes[0];
                a.transform.localPosition = new Vector3(float.Parse(objectAttributes[1]), float.Parse(objectAttributes[2]), float.Parse(objectAttributes[3]));
                a.transform.localScale = new Vector3(float.Parse(objectAttributes[4]), float.Parse(objectAttributes[5]), float.Parse(objectAttributes[6]));
            }
        }
    }
    
}
