using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using Unity.Barracuda;
using UnityEditor;

public class IceAgentModelLoader : MonoBehaviour
{
    //public NNModel modelToLoad;
    public Dropdown existingEnvironments;
    public Dropdown existingModels;
    public string[] maps;
    public string[] models;
    public Button loadMapProfilesButton;
    public Button loadSpecificModels;

    public InputField userName;
    

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
            loadMapProfilesButton.GetComponent<Button>().onClick.AddListener(loadMapModels);
        } catch {
            UnityEngine.Debug.Log("error with load map profiles button");
        }
        
        try{
			loadSpecificModels.GetComponent<Button>().onClick.AddListener(loadMapAndModel);
		} catch {
			UnityEngine.Debug.Log("loadAttributes button error");
		}
    }

    void loadMapModels()
    {
        /*
        models = File.ReadAllLines(Application.dataPath + "/" + maps[existingEnvironments.value] + "/trained_models.json");
        List<string> options = new List<string>();
        for (int i=0; i<models.Length; i++)
        {
            options.Add(models[i]);
        }
        existingModels.ClearOptions();
        existingModels.AddOptions(options);
        */
        loadMapAndModel();
    }

    void loadMapAndModel()
    {
        //F:\Unity\2020.3.19f1\Evolving_Artificial_Life\
        //Unity.Barracuda.NNModel nnmodel = (Unity.Barracuda.NNModel)AssetDatabase.LoadAssetAtPath("Assets/Models/v1.4.14/basic_agent.onnx", typeof(Unity.Barracuda.NNModel)); 
        //Agent.SetModel("basic_agent", nnmodel);

        //UnityEngine.Debug.Log(AssetDatabase.FindAssets("Assets/My Behaviour.onnx"));
        //UnityEngine.Debug.Log(AssetDatabase.LoadAssetAtPath(@"results/" + models[existingModels.value] + "/My Behaviour.onnx", typeof(Unity.Barracuda.NNModel)));
        //NNModel modelToLoad = (NNModel)AssetDatabase.LoadAssetAtPath(@"results/" + models[existingModels.value] + "/My Behaviour.onnx", typeof(NNModel));
        //NNModel modelToLoad = (NNModel)AssetDatabase.LoadAssetAtPath("My Behaviour.onnx", typeof(NNModel));
        //UnityEngine.Debug.Log(modelToLoad);
        GameObject a = Instantiate(environmentPrefab) as GameObject;
        //a.transform.GetChild(0).gameObject.GetComponent<SlipperyIceAgent>().SetModel("name", test);
        //a.transform.GetChild(0).gameObject.GetComponent<SlipperyIceAgent>().BehaviourType = 2;
        a.transform.position = new Vector3(0,0,0);
        a.GetComponent<Save>().loadButtonPress2(maps[existingEnvironments.value]);



        a.transform.GetChild(0).gameObject.GetComponent<SlipperyIceAgent>().setScoringUp(maps[existingEnvironments.value], userName);
        
        

    }
}
