using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public GameObject SettingsObject;

    public int foodCount;
    public int foodCountLimit;

    public Boolean earlyTrainingMode;
    public Boolean testing;

    public int respawnTime = 1;
    public int timer;

    public Collider spawnArea;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void Start()
    {
        //Debug.Log("here!");
        try{
            

            SettingsObject = GameObject.FindGameObjectWithTag("Settings");
            respawnTime = SettingsObject.GetComponent<SettingsScript>().getFoodSpawnRate();
            foodCountLimit = SettingsObject.GetComponent<SettingsScript>().getFoodCountLimit();
            earlyTrainingMode = SettingsObject.GetComponent<SettingsScript>().getIsEarlyTraining();

            if(earlyTrainingMode)
            {
                foodCountLimit = 1;
            }

        } catch (Exception e){
            //Debug.Log("didn't find settings");
            respawnTime = 60;
            foodCountLimit = 1;
            earlyTrainingMode = true;
            testing = true;
        }
        
        foodCount = 0;
        timer = 0;
        spawnArea = this.GetComponent<Collider>();
    }
    /*
    if(testing != true)
    {
        //If there should be a food spawned in early training mode, spawn it
        if(earlyTrainingMode)
        {
            Debug.Log("here!");
            if(foodCount == 0)
            {
                spawnFood(new Vector3(-50, 0, 0));
            }
        }

        else
        {
            Debug.Log("not early training");
            //if there is space to spawn food
            if(foodCount < foodCountLimit)
            {
                //if the count is high enough
                if(timer >= respawnTime)
                {
                    spawnFood(new Vector3(UnityEngine.Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x), UnityEngine.Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y), 0));
                    timer = 0;
                }
                timer = timer + 1;
            }
        }
        if(earlyTrainingMode)
        {
            Debug.Log("here!!");
            if(foodCount == 0)
            {
                spawnFood(new Vector3(-50, 0, 0));
            }
        }
    }
    */

    void Update()
    {
        
        if(earlyTrainingMode)
        {
            //Debug.Log("here!!");
            if(foodCount == 0)
            {
                //Debug.Log("min x " + spawnArea.bounds.min.x + " max " + spawnArea.bounds.max.x + " " + gameObject.transform.parent.name);
                spawnFood(new Vector3(UnityEngine.Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x), UnityEngine.Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y), 0));
            }
        }

    }

    public void decreaseCount(){
        foodCount = foodCount - 1;
    }

    public void spawnFood(Vector3 spawnPoint){

        GameObject a = Instantiate(foodPrefab) as GameObject;
        a.transform.SetParent(this.gameObject.transform.parent);
        a.transform.position = spawnPoint;
        foodCount = foodCount + 1;

        if(testing == true)
        {
            //Debug.Log("in testing mode");
            a.GetComponent<Food>().testingMode();
        }

        //had to put here due to not wanting to send back an object
        if(earlyTrainingMode)
        {
            if(testing != true)
            {
                a.GetComponent<Food>().earlyTrainingEnergyMode();
            }
        }
    }

    public void resetFood(){
        Food[] allFood = GameObject.FindObjectsOfType<Food>();
        foreach (Food currentFood in allFood)
        {
            if(currentFood.transform.IsChildOf(this.gameObject.transform.parent))
            {
                Debug.Log("deleted food");
                decreaseCount();
                currentFood.overrideDestroy();
            }
        }
    }
}
