using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadMoveToFoodAgent : MonoBehaviour
{
    public Button loadModelButton;
    public GameObject agent;
    //public NNModel modelToLoad;
    // Start is called before the first frame update
    void Start()
    {
        loadModelButton.GetComponent<Button>().onClick.AddListener(loadModelButtonPress);
    }

    void loadModelButtonPress()
	{
        /*
        var assetPath = m_BehaviorNameOverrides[behaviorName];
        byte[] model = null;
        try
            {
                model = File.ReadAllBytes(assetPath);
            }
            catch(IOException)
            {
                Debug.Log($"Couldn't load file {assetPath}", this);
                // Cache the null so we don't repeatedly try to load a missing file
                m_CachedModels[behaviorName] = null;
                return null;
            }

        
        var asset = ScriptableObject.CreateInstance<NNModel>();
        asset.modelData = ScriptableObject.CreateInstance<NNModelData>();
        asset.modelData.Value = model;
        */
        //agent.GetComponent<MoveToFoodAgent>().SetModel();
        //string behaviorName, NNModel model, InferenceDevice inferenceDevice = default(InferenceDevice))
	}
}
