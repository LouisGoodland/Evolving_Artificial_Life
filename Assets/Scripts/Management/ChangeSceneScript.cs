using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System;

public class ChangeSceneScript : MonoBehaviour
{
    public Button Env1LearningButton;
	public Button Env1TestingButton;
	public Button Env2LearningButton;
	public Button Env2TestingButton;
	public Button backToMenuButton;
	public Button startLearningButton;
	public Button toGoStepByStepButton;

	public Button startTrainingButton;

	public Button goToEditingAttributesButton;

	public Button goToExplanationsButton;

	public string batFileLocation;

	void Start () 
	{
		//UnityEngine.Debug.Log("started");
		try{
			Env1LearningButton.GetComponent<Button>().onClick.AddListener(Env1LearningButtonPress);
		} catch {
			UnityEngine.Debug.Log("couldn't find Env1LearningButton");
		}
		try{
			Env2LearningButton.GetComponent<Button>().onClick.AddListener(Env2LearningButtonPress);
		} catch {
			UnityEngine.Debug.Log("couldn't find Env2LearningButton");
		}

		try{
			Env1TestingButton.GetComponent<Button>().onClick.AddListener(Env1TestingButtonPress);
		} catch {
			UnityEngine.Debug.Log("couldn't find Env1TestingButton");
		}
		try{
			Env2TestingButton.GetComponent<Button>().onClick.AddListener(Env2TestingButtonPress);
		} catch {
			UnityEngine.Debug.Log("couldn't find Env2TestingButton");
		}

		try{
			startLearningButton.GetComponent<Button>().onClick.AddListener(startLearningButtonPress);
		} catch {
			UnityEngine.Debug.Log("couldn't find startLearningButton");
		}


		try{
			goToEditingAttributesButton.GetComponent<Button>().onClick.AddListener(goToEditingAttributes);
		} catch {
			UnityEngine.Debug.Log("couldn't find goToEditingAttributes");
		}

		try{
			toGoStepByStepButton.GetComponent<Button>().onClick.AddListener(toGoStepByStep);
		} catch {
			UnityEngine.Debug.Log("couldn't find go to step by step button");
		}

		try{
			goToExplanationsButton.GetComponent<Button>().onClick.AddListener(goToExplanations);
		} catch {
			UnityEngine.Debug.Log("couldn't find go to explanations Button");
		}
		
	}

	void goToExplanations()
	{
		SceneManager.LoadScene(8);
	}

	void toGoStepByStep()
	{
		SceneManager.LoadScene(7);
	}

	void goToEditingAttributes()
	{
		SceneManager.LoadScene(6);
	}
	

	void startLearningButtonPress()
	{
		var processInfo = new ProcessStartInfo("C:/Users/Loudini/Desktop/test.bat");
		processInfo.CreateNoWindow = false;
		processInfo.UseShellExecute = true;
		Process process = Process.Start(processInfo);
	}

	//this here works
	void Env1LearningButtonPress()
	{
		UnityEngine.Debug.Log("ev1Learning");

		Process.Start(Application.dataPath + "/example.bat");
		StartCoroutine(waiter());

		SceneManager.LoadScene(1);
	}

	//called from attributes to training
	void Env2LearningButtonPress()
	{
		SceneManager.LoadScene(4);
	}

	void Env1TestingButtonPress()
	{
		UnityEngine.Debug.Log("ev1Testing");
		SceneManager.LoadScene(2);
	}

	void Env2TestingButtonPress()
	{
		UnityEngine.Debug.Log("ev2Testing");
		SceneManager.LoadScene(5);
	}

	IEnumerator waiter()
	{
		yield return new WaitForSeconds(25);
	}
}