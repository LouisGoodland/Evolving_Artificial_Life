using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Diagnostics;
using System;
using System.Text.RegularExpressions;
public class EditLearningParams : MonoBehaviour
{
    public Button saveButton;

    //attributes for behaviour.yaml file
    public InputField Learning_rate;
    public InputField Num_layers;
    public InputField Max_steps;
    public InputField Hidden_units;
    //help buttons
    public Button help6;
    public Button help7;
    public Button help8;
    public Button help9;

    //reward config toggles
    public Toggle wallRewardToggle;
    public Toggle checkpointRewardToggle;
    public Toggle checkpointProximityRewardToggle;
    public Toggle LapRewardToggle;
    public Toggle LapTimeRewardToggle;
    //reward config amounts
    public InputField wallRewardAmount;
    public InputField checkpointRewardAmount;
    public InputField checkpointProximityRewardAmount;
    public InputField LapRewardAmount;
    public InputField LapTimeRewardAmount;
    //help buttons
    public Button help1;
    public Button help2;
    public Button help3;
    public Button help4;
    public Button help5;

    public Text infoBox;

    public Slider checkpointProximityRewardPivotSlider;
    public Text checkPointRewardSliderInfo;

    public Dropdown existingEnvironments;
    public InputField profileName;
    public Text displayErrorMessage;
    
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
			saveButton.GetComponent<Button>().onClick.AddListener(SaveButtonPress);
		} catch (Exception e){
			UnityEngine.Debug.Log("saveButton issue");
		}
        
        try{
			help1.GetComponent<Button>().onClick.AddListener(help1Press);
		} catch (Exception e){
			UnityEngine.Debug.Log("help1 issue");
		}

        try{
			help2.GetComponent<Button>().onClick.AddListener(help2Press);
		} catch (Exception e){
			UnityEngine.Debug.Log("help2 issue");
		}

        try{
			help3.GetComponent<Button>().onClick.AddListener(help3Press);
		} catch (Exception e){
			UnityEngine.Debug.Log("help3 issue");
		}

        try{
			help4.GetComponent<Button>().onClick.AddListener(help4Press);
		} catch (Exception e){
			UnityEngine.Debug.Log("help4 issue");
		}

        try{
			help5.GetComponent<Button>().onClick.AddListener(help5Press);
		} catch (Exception e){
			UnityEngine.Debug.Log("help5 issue");
		}

        try{
			help6.GetComponent<Button>().onClick.AddListener(help6Press);
		} catch (Exception e){
			UnityEngine.Debug.Log("help6 issue");
		}

        try{
			help7.GetComponent<Button>().onClick.AddListener(help7Press);
		} catch (Exception e){
			UnityEngine.Debug.Log("help7 issue");
		}

        try{
			help8.GetComponent<Button>().onClick.AddListener(help8Press);
		} catch (Exception e){
			UnityEngine.Debug.Log("help8 issue");
		}

        try{
			help9.GetComponent<Button>().onClick.AddListener(help9Press);
		} catch (Exception e){
			UnityEngine.Debug.Log("help9 issue");
		}
    }

    void help1Press()
    {
        infoBox.text = "Determine what reward is given if the agent collides with a wall." +
        " The suggestion is that this should be a negative value e.g. -5";
    }

    void help2Press()
    {
        infoBox.text = "Determine what reward is given if the agent collides with a checkpoint." +
        " The suggestion is that this should be a positive value e.g. 5";
    }

    void help3Press()
    {
        infoBox.text = "Determine the rate in which reward is given based on how close the agent is to a checkpoint." +
        " The suggestion is that this should be a small positive reward, e.g. 0.5";
    }

    void help4Press()
    {
        infoBox.text = "Determine a fixed reward that is given if the agent reaches all checkpoints." +
        " The suggestion is that this should a large positive value e.g. 20";
    }

    void help5Press()
    {
        infoBox.text = "Determine how much of a reward is given based on the laptime the agent achieves" +
        " The suggestion is that this should be a large positive value e.g. 20";
    }

    void help6Press()
    {
        infoBox.text = "Number of Hidden Units. This is the number of neurons in the network. Recommended to set this to around 128." +
        " The hidden units represents the complexity of the network. The higher the hidden units the more complex the network is ";
    }

    void help7Press()
    {
        infoBox.text = "Learning Rate. It is recommended that this is a low number (0.003~)." +
        " The learning rate is how quickly the agent adapts to the information collected. A too high number" +
        " will result in a quick adaptation but could result in over learning (Where it adapts to the situation too specifically)."
        + " A too low learning rate could mean that the behaviour doesn't change fast enough.";
    }

    void help8Press()
    {
        infoBox.text = "Max Steps. This is how long the agent will learn for. The more steps you can run the better as the agent" +
        " will have longer to adapt. 1000000 is the minimum recommended amount and will take roughly 40 mins to train.";
    }

    void help9Press()
    {
        infoBox.text = "Num of layers. This is similar to hidden units but instead it is how many layers of the network there are (see diagram)." +
        " It is recommended that this is set to 2 or around that";
    }

    void Update()
    {
        checkPointRewardSliderInfo.text = "Start giving reward when: " + checkpointProximityRewardPivotSlider.value + " units away";
    }

    void SaveButtonPress()
    {
        string attributes = "";
        //checking
        try
        {
            float check1 = float.Parse(wallRewardAmount.text);
            float check2 = float.Parse(checkpointRewardAmount.text);
            float check3 = float.Parse(checkpointProximityRewardAmount.text);
            float check4 = float.Parse(LapRewardAmount.text);
            float check5 = float.Parse(LapTimeRewardAmount.text);
            float check6 = checkpointProximityRewardPivotSlider.value;

            if(wallRewardToggle.isOn==false)
            {
                check1 = 0f;
            }
            if(checkpointRewardToggle.isOn==false)
            {
                check2 = 0f;
            }
            if(checkpointProximityRewardToggle.isOn==false)
            {
                check3 = 0f;
            }
            if(LapRewardToggle.isOn==false)
            {
                check4 = 0f;
            }
            if(LapTimeRewardToggle.isOn==false)
            {
                check5 = 0f;
            }

            

            attributes = 
                "wallReward," + wallRewardToggle.isOn + "," + check1 + "\n" +
                "checkPointReward," + checkpointRewardToggle.isOn + "," + check2 + "\n" +
                "checkpointProximityReward," + checkpointProximityRewardToggle.isOn + "," + check3
                 + "," + checkpointProximityRewardPivotSlider.value + "\n" +
                "LapRewardAmount," + LapRewardToggle.isOn + "," + check4 + "\n" +
                "LapTimeRewardAmount," + LapTimeRewardToggle.isOn + "," +  check5;
            
            
        }
        catch
        {
            UnityEngine.Debug.Log("Error with reward allocation, Make sure all are filled in");
            attributes = 
                "wallReward," + wallRewardToggle.isOn + ",0" + "\n" +
                "checkPointReward," + checkpointRewardToggle.isOn + ",0" + "\n" +
                "checkpointProximityReward," + checkpointProximityRewardToggle.isOn + ",0,100" + "\n" +
                "LapRewardAmount," + LapRewardToggle.isOn + ",0" + "\n" +
                "LapTimeRewardAmount," + LapTimeRewardToggle.isOn + ",0";
            
        }

        string behaviourYaml = "";

        try
        {
            float check1 = float.Parse(Learning_rate.text);
            int check2 = int.Parse(Hidden_units.text);
            int check3 = int.Parse(Num_layers.text);
            int check4 = int.Parse(Max_steps.text);

            behaviourYaml = "behaviors:" + "\n" +
            "  My Behavior:" + "\n" +
            "    trainer_type: ppo" + "\n" + 
            "    hyperparameters:" + "\n" + 
            "      batch_size: 64" + "\n" + 
            "      buffer_size: 12000" + "\n" + 
            "      learning_rate: " + Learning_rate.text + "\n" +
            "      beta: 0.001" + "\n" + 
            "      epsilon: 0.2" + "\n" + 
            "      lambd: 0.99" + "\n" + 
            "      num_epoch: 3" + "\n" + 
            "      learning_rate_schedule: linear" + "\n" +
            "    network_settings:" + "\n" + 
            "      normalize: true" + "\n" + 
            "      hidden_units: " + Hidden_units.text + "\n" + 
            "      num_layers: " + Num_layers.text + "\n" + 
            "      vis_encode_type: simple" + "\n" + 
            "    reward_signals:" + "\n" + 
            "      extrinsic:" + "\n" + 
            "        gamma: 0.99" + "\n" + 
            "        strength: 1.0" + "\n" + 
            "    keep_checkpoints: 5" + "\n" + 
            "    max_steps: " + Max_steps.text + "\n" + 
            "    time_horizon: 1000" + "\n" + 
            "    summary_freq: 10000";
        }
        catch
        {
            behaviourYaml = "behaviors:" + "\n" +
            "  My Behavior:" + "\n" +
            "    trainer_type: ppo" + "\n" + 
            "    hyperparameters:" + "\n" + 
            "      batch_size: 64" + "\n" + 
            "      buffer_size: 12000" + "\n" + 
            "      learning_rate: 0.0003" + "\n" +
            "      beta: 0.001" + "\n" + 
            "      epsilon: 0.2" + "\n" + 
            "      lambd: 0.99" + "\n" + 
            "      num_epoch: 3" + "\n" + 
            "      learning_rate_schedule: linear" + "\n" +
            "    network_settings:" + "\n" + 
            "      normalize: true" + "\n" + 
            "      hidden_units: 128" + "\n" + 
            "      num_layers: 2" + "\n" + 
            "      vis_encode_type: simple" + "\n" + 
            "    reward_signals:" + "\n" + 
            "      extrinsic:" + "\n" + 
            "        gamma: 0.99" + "\n" + 
            "        strength: 1.0" + "\n" + 
            "    keep_checkpoints: 5" + "\n" + 
            "    max_steps: 10000000" + "\n" + 
            "    time_horizon: 1000" + "\n" + 
            "    summary_freq: 10000";
        }
        
        //validation for profile name
        profileName.text = profileName.text.Replace(' ', '_');
        if(profileName.text == "")
        {
            profileName.text = "blank";
        }

        //add to list of existing maps
        string[] profiles = File.ReadAllLines(Application.dataPath + "/" + maps[existingEnvironments.value] + "/profiles.json");

        //checks if the profile name already exists
        bool toAdd = true;
        for(int i=0; i<profiles.Length; i++)
        {
            if(profileName.text == profiles[i])
            {
                toAdd = false;
            }
        }


        //makes a new profile
        if(toAdd)
        {
            
            string[] newProfiles = new string[profiles.Length + 1];
            for(int i=0; i<profiles.Length; i++)
            {
                newProfiles[i] = profiles[i];
            }
            newProfiles[profiles.Length] = profileName.text;

            string newProfilesString = "";
            for(int i=0; i<newProfiles.Length; i++)
            {
                newProfilesString = newProfilesString + newProfiles[i] + "\n";
            }

            //writes profile
            File.WriteAllText(Application.dataPath + "/" + maps[existingEnvironments.value] + "/profiles.json", newProfilesString);
            displayErrorMessage.text = "Saved as: " + profileName.text;

            var folder2 = Directory.CreateDirectory(Application.dataPath + "/" + maps[existingEnvironments.value] + "/" + profileName.text);
        
            

            //generating bat file for execution
            string batchFile = 
            "ECHO Starting!" + "\n" +
            @"cd F:\Unity\2020.3.19f1\Evolving_Artificial_Life\venv\Scripts\activate.bat" + "\n" +
            @"mlagents-learn F:\Unity\2020.3.19f1\Evolving_Artificial_Life\Assets\" + maps[existingEnvironments.value] + @"\" + profileName.text + @"\behaviour.yaml --run-id=" + "\"" + profileName.text + "\"" + " --force" + "\n" +
            "pause";
            

            File.WriteAllText(Application.dataPath + "/" + maps[existingEnvironments.value] + "/" + profileName.text + "/behaviour.yaml", behaviourYaml);
            File.WriteAllText(Application.dataPath + "/" + maps[existingEnvironments.value] + "/" + profileName.text + "/attributes.json", attributes);
            File.WriteAllText(@"C:\Users\Loudini\Desktop\run_" + maps[existingEnvironments.value] + ".bat", batchFile);
            File.WriteAllText(Application.dataPath + "/" + maps[existingEnvironments.value] + "/" + profileName.text + "/run.bat", batchFile);

            
            try{
                GameObject Cam = GameObject.Find("Main Camera");
                Cam.GetComponent<ChangeSceneScript>().batFileLocation = Application.dataPath + "/" + maps[existingEnvironments.value] + "/" + profileName.text + "/run.bat";
            } catch {
                UnityEngine.Debug.Log("failed");
            }
            

        } 
        else
        {
            profileName.text = "";
            displayErrorMessage.text = "Name already exists, try another";
        }
        
    }
}
