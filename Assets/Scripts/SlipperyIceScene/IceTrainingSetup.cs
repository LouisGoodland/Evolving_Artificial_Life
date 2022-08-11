using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using System.Diagnostics;


public class IceTrainingSetup : MonoBehaviour
{
    public Dropdown existingEnvironments;
    public Dropdown existingProfiles;
    public string[] maps;
    public string[] profiles;
    public Button loadMapProfilesButton;
    public Button loadSpecificProfile;

    public GameObject environmentPrefab;
    
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
            loadMapProfilesButton.GetComponent<Button>().onClick.AddListener(loadMapProfiles);
        } catch {
            UnityEngine.Debug.Log("error with load map profiles button");
        }
        try{
			loadSpecificProfile.GetComponent<Button>().onClick.AddListener(loadMap);
		} catch {
			UnityEngine.Debug.Log("loadAttributes button error");
		}

        /*

        try{
			loadModelButton.GetComponent<Button>().onClick.AddListener(loadModel);
		} catch {
			UnityEngine.Debug.Log("loadModel button error");
		}

        try{
            loadMapProfilesButton.GetComponent<Button>().onClick.AddListener(loadMapProfiles);
        } catch {
            UnityEngine.Debug.Log("error with load map profiles button");
        }
        */
    }

    void loadMapProfiles()
    {
        //F:\Unity\2020.3.19f1\Evolving_Artificial_Life\results
        //UnityEngine.Debug.Log();

        profiles = File.ReadAllLines(Application.dataPath + "/" + maps[existingEnvironments.value] + "/profiles.json");
        List<string> options = new List<string>();
        for (int i=0; i<profiles.Length; i++)
        {
            options.Add(profiles[i]);
        }
        existingProfiles.ClearOptions();
        existingProfiles.AddOptions(options);

        loadSpecificProfile.transform.localPosition = new Vector3(-423, 197, 0);
        existingProfiles.transform.localPosition = new Vector3(-557, 197, 0);
    }

    void loadMap()
    {
        //reads all of the existing trained models
        string[] existingModels = File.ReadAllLines(Application.dataPath + "/" + maps[existingEnvironments.value] + "/trained_models.json");
        
        //makes a new array 
        string[] newModels = new string[existingModels.Length + 1];
        for(int i=0; i<existingModels.Length; i++)
        {
            newModels[i] = existingModels[i];
        }
        newModels[newModels.Length - 1] = profiles[existingProfiles.value];

        string newModelsString = "";
        for(int i=0; i<newModels.Length; i++)
        {
            newModelsString = newModelsString + newModels[i] + "\n";
        }

        //writes models
        File.WriteAllText(Application.dataPath + "/" + maps[existingEnvironments.value] + "/trained_models.json", newModelsString);

        //runs the correct environment
        Process.Start(Application.dataPath + "/" + maps[existingEnvironments.value] + "/" + profiles[existingProfiles.value] + "/run.bat");
        StartCoroutine(waiter());

        for(int i = -1; i < 3; i++)
        {
            for(int j = -1; j < 3; j++)
            {
                GameObject a = Instantiate(environmentPrefab) as GameObject;
                a.name = "environment" + i.ToString() + j.ToString();
                a.transform.position = new Vector3(i * 4500, j * 2250, 0);
                //loads the map for scene
                a.transform.GetChild(0).gameObject.GetComponent<SlipperyIceAgent>().updateAttributes(maps[existingEnvironments.value], profiles[existingProfiles.value]);
                a.GetComponent<Save>().loadButtonPress2(maps[existingEnvironments.value]);
            }
        }
    }

    IEnumerator waiter()
	{
		yield return new WaitForSeconds(25);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
