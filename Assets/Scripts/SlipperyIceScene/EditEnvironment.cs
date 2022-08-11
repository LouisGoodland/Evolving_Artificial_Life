using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Diagnostics;
using System;
using System.Text.RegularExpressions;

public class EditEnvironment : MonoBehaviour
{
    //arraylists of 
    [SerializeField] public List<GameObject> checkPoints = new List<GameObject>();
    [SerializeField] public List<GameObject> wallPieces = new List<GameObject>();
    [SerializeField] public GameObject startPoint;

    //gameobjects
    public GameObject containerPrefab;
    public GameObject wallPiece;
    public GameObject checkpointPiece;
    public GameObject spawnSheet;

    public bool hasSelected;
    public GameObject selected;

    //type of piece
    public bool isWallPiece;
    public Toggle isWallPieceToggle;

    public bool isSelecting;
    public Toggle isSelectingToggle;

    //Dimensions
    public float size1;
    public Slider size1Slider;
    public float size2;
    public Slider size2Slider;

    public Button deleteButton;
    public Button deselectButton;

    public Button saveButton;
    public Button loadButton;

    public Text checkPointIntText;
    public Button orderUp;
    public Button orderDown;

    public InputField environmentName;
    public Text displayErrorMessage;

    //orientation
    public int orientation;
    public int amountOfCheckpoints;

    public string path = "";
    public string persistentPath = "";

    void Start()
    {

        try{
			deleteButton.GetComponent<Button>().onClick.AddListener(DeleteButtonPress);
		} catch (Exception e){
			UnityEngine.Debug.Log("delete issue");
		}

        try{
			deselectButton.GetComponent<Button>().onClick.AddListener(DeselectButtonPress);
		} catch (Exception e){
			UnityEngine.Debug.Log("deselect issue");
		}

        try{
			saveButton.GetComponent<Button>().onClick.AddListener(saveButtonPress);
		} catch (Exception e){
			UnityEngine.Debug.Log("save issue");
		}

        try{
			orderUp.GetComponent<Button>().onClick.AddListener(orderUpButtonPress);
		} catch (Exception e){
			UnityEngine.Debug.Log("OrderUp issue");
		}

        try{
			orderDown.GetComponent<Button>().onClick.AddListener(orderDownButtonPress);
		} catch (Exception e){
			UnityEngine.Debug.Log("OrderUp issue");
		}

    }

    void orderDownButtonPress()
    {
        //Moves existing up
        String currentCheckPointString = Regex.Match(selected.name, @"\d+").Value;
        int currentCheckPoint = Int32.Parse(currentCheckPointString) - 1;
        int replacementInt = currentCheckPoint + 1;

        GameObject toGoDown = GameObject.Find("Checkpoint (" + currentCheckPoint + ")");

        toGoDown.name = "Checkpoint (" + replacementInt + ")";
        selected.name = "Checkpoint (" + currentCheckPoint + ")";
        checkPointIntText.text = selected.name;

    }

    void orderUpButtonPress()
    {
        //Moves existing up
        String currentCheckPointString = Regex.Match(selected.name, @"\d+").Value;
        int currentCheckPoint = Int32.Parse(currentCheckPointString) + 1;
        int replacementInt = currentCheckPoint - 1;

        GameObject toGoDown = GameObject.Find("Checkpoint (" + currentCheckPoint + ")");

        toGoDown.name = "Checkpoint (" + replacementInt + ")";
        selected.name = "Checkpoint (" + currentCheckPoint + ")";
        checkPointIntText.text = selected.name;

    }

    void DeleteButtonPress()
	{
        if (selected.tag == "CheckPoint")
        {
            checkPointDeletion();
        }
        if (selected.name != "SquareStart")
        {
            if(selected.tag != "CheckPoint")
            {
                wallPieces.Remove(selected);
            }
            Destroy(selected);
            selectionRemoval();
        }
		
	}

    void DeselectButtonPress()
	{
        if (selected.tag == "CheckPoint")
        {
            selected.GetComponent<Renderer>().material.SetColor("_Color", Color.green);
        } else 
        {
            selected.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
        }
		selectionRemoval();
	}

    void checkPointDeletion()
    {
        //renaming the checkpoints
        GameObject[] list = GameObject.FindGameObjectsWithTag("CheckPoint");
        
        //collecting int for the checkpoint
        String checkPointIntSelectedString = Regex.Match(selected.name, @"\d+").Value;
        int checkPointIntSelected = Int32.Parse(checkPointIntSelectedString);

        //list to be filled
        List<GameObject> filteredCheckpoints = new List<GameObject>();

        foreach(GameObject t in list) 
        {
            //collecting int for the checkpoint
            String currentCheckPointString = Regex.Match(t.name, @"\d+").Value;
            int currentCheckPoint = Int32.Parse(currentCheckPointString);

            if(currentCheckPoint > checkPointIntSelected)
            {
                filteredCheckpoints.Add(t);
            }
        }
        
        foreach(GameObject t in filteredCheckpoints) 
        {
            //collecting int for the checkpoint
            String currentCheckPointString = Regex.Match(t.name, @"\d+").Value;
            int currentCheckPoint = Int32.Parse(currentCheckPointString);
            int newCheckPointInt = currentCheckPoint - 1;
            t.name = "Checkpoint (" + newCheckPointInt + ")";
        }
        
        

        amountOfCheckpoints = amountOfCheckpoints - 1;
        checkPoints.Remove(selected);

    }

    void selectionRemoval()
    {
        hasSelected = false;
        deleteButton.transform.position = new Vector3(9999999f, 9999999f, 99999999f);
        deselectButton.transform.position = new Vector3(9999999f, 9999999f, 99999999f);
        selected = null;
    }

    

    // Update is called once per frame
    void Update()
    {
        //attribute collection
        
        isWallPiece = isWallPieceToggle.isOn;
        isSelecting = isSelectingToggle.isOn;
        size1 = size1Slider.value;
        size2 = size2Slider.value;

        //if the object has been selected, move it to the location
        if(hasSelected)
        {
            selected.transform.localScale = new Vector3(size1, size2, 1);
            if(Input.GetMouseButton(0))
            {
                RaycastHit hit2;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit2))
                {  
                    selected.transform.position = hit2.point + new Vector3(0f, 0f, -0.1f);
                }
            }
        }

        else 
        {
            if (Input.GetMouseButtonDown(0))
            {

                if(isSelecting)
                {
                    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                    RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                    if (hit.collider != null)
                    {
                        UnityEngine.Debug.Log("hit!!!");
                        
                        size1Slider.value = hit.collider.gameObject.transform.localScale.x;
                        size2Slider.value = hit.collider.gameObject.transform.localScale.y;

                        checkPointIntText.text = hit.collider.gameObject.name;

                        deleteButton.transform.position = new Vector3(1807f, -210f, -0.1f);
                        deselectButton.transform.position = new Vector3(1807f, -810f, -0.1f);

                        selected = hit.collider.gameObject;
                        hit.collider.gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(1f, 0, 0));
                        hasSelected = true;

                    }
                } 
                else
                {
                    UnityEngine.Debug.Log("spawning object");
                    RaycastHit hit2;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit2))
                    {  
                        spawnObject(hit2.point);
                    }
                }
            }
        }
        
    }

    void spawnObject(Vector3 spawnPoint)
    {
        GameObject a;
        if(isWallPiece)
        {
            a = Instantiate(wallPiece) as GameObject;

            a.transform.localScale = new Vector3(size1, size2, 1);
            a.transform.localPosition = spawnPoint + new Vector3(0f, 0f, -0.1f);
            wallPieces.Add(a);
        } 
        else 
        {
            a = Instantiate(checkpointPiece) as GameObject;
            a.gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(0, 1f, 0));
            a.name = "Checkpoint (" + amountOfCheckpoints + ")";
            amountOfCheckpoints = amountOfCheckpoints + 1;
            
            a.transform.localScale = new Vector3(size1, size2, 1);
            a.transform.localPosition = spawnPoint + new Vector3(0f, 0f, -0.1f);
            checkPoints.Add(a);

        }
        
    }

    void saveButtonPress()
    {
        

        if(checkPoints.Count > 0)
        {
            string checkPointsString = "";
            for(int i=0; i< checkPoints.Count; i++)
            {
                checkPointsString = checkPointsString + checkPoints[i].name + "," +
                checkPoints[i].transform.localPosition[0] + "," + checkPoints[i].transform.localPosition[1] + "," + checkPoints[i].transform.localPosition[2] + "," + 
                checkPoints[i].transform.localScale[0] + "," + checkPoints[i].transform.localScale[1] + "," + checkPoints[i].transform.localScale[2] + "," + "\n";
            }

            string wallPiecesString = "";

            for(int i=0; i< wallPieces.Count; i++)
            {
                wallPiecesString = wallPiecesString + wallPieces[i].name + "," +
                wallPieces[i].transform.localPosition[0] + "," + wallPieces[i].transform.localPosition[1] + "," + wallPieces[i].transform.localPosition[2] + "," + 
                wallPieces[i].transform.localScale[0] + "," + wallPieces[i].transform.localScale[1] + "," + wallPieces[i].transform.localScale[2] + "," + "\n";
            }
            
            string spawnString = startPoint.transform.localPosition[0] + "," + startPoint.transform.localPosition[1] + "," + startPoint.transform.localPosition[2];

            //formats the name so it's valid
            environmentName.text = environmentName.text.Replace(' ', '_');
            if(environmentName.text == "")
            {
                environmentName.text = "blank";
            }

            //add to list of existing maps
            string[] maps = File.ReadAllLines(Application.dataPath + "/maps.json");

            //checks if the map name already exists
            bool toAdd = true;
            for(int i=0; i<maps.Length; i++)
            {
                if(environmentName.text == maps[i])
                {
                    toAdd = false;
                }
            }


            //makes a new map
            if(toAdd)
            {
                
                string[] newMaps = new string[maps.Length + 1];
                for(int i=0; i<maps.Length; i++)
                {
                    newMaps[i] = maps[i];
                }
                newMaps[maps.Length] = environmentName.text;

                string newMapString = "";
                for(int i=0; i<newMaps.Length; i++)
                {
                    newMapString = newMapString + newMaps[i] + "\n";
                }

                File.WriteAllText(Application.dataPath + "/maps.json", newMapString);
                displayErrorMessage.text = "Saved as: " + environmentName.text;
            } 
            else
            {
                environmentName.text = "";
                displayErrorMessage.text = "Name already exists, try another";
            }
            

            var folder = Directory.CreateDirectory(Application.dataPath + "/" + environmentName.text);
            var folder2 = Directory.CreateDirectory(Application.dataPath + "/" + environmentName.text + "/Map");

            File.WriteAllText(Application.dataPath + "/" + environmentName.text + "/Map/checkPointsString.json", checkPointsString);
            File.WriteAllText(Application.dataPath + "/" + environmentName.text + "/Map/wallPiecesString.json", wallPiecesString);
            File.WriteAllText(Application.dataPath + "/" + environmentName.text + "/Map/spawnString.json", spawnString);
            
            string blank = "";
            File.WriteAllText(Application.dataPath + "/" + environmentName.text + "/leaderboard.json", blank);
            File.WriteAllText(Application.dataPath + "/" + environmentName.text + "/profiles.json", blank);
            File.WriteAllText(Application.dataPath + "/" + environmentName.text + "/trained_models.json", blank);
        }
        else
        {
            displayErrorMessage.text = "Please add at least 1 checkpoint";
        }
    }
}   
