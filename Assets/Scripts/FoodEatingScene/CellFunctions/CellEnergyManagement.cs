using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CellEnergyManagement : MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject SettingsObject;
    public GameObject localFoodSpawner;

    public GameObject healthBar;

    public Text energyInt;

    float baseEnergyMultiplier;
    float movementEnergyMultiplier;
    float energyLevel;

    public bool hasTrained;

    void Start()
    {
        energyLevel = 25000f;
        try{
            SettingsObject = GameObject.FindGameObjectWithTag("Settings");
            this.baseEnergyMultiplier = SettingsObject.GetComponent<SettingsScript>().getBaseEnergyMultiplier();
            this.movementEnergyMultiplier = SettingsObject.GetComponent<SettingsScript>().getMovementEnergyMultiplier();
        } catch (Exception e){
            //Debug.Log("didn't find settings");
            baseEnergyMultiplier = 1f;
            movementEnergyMultiplier = 1f;
        }
    }
    
    //This sets the energy level back to its default
    public void reset()
    {
        energyLevel = 25000f;
    }

    //This provides general energy loss (metabolic rate)
    void Update(){
        //Metabolic rate
        if(hasTrained)
        {
            UnityEngine.Debug.Log("here");
            energyLevel = energyLevel - (0.005f * 2500f * (baseEnergyMultiplier/10));
        }
        else
        {
            energyLevel = energyLevel - (0.005f * 2500f * baseEnergyMultiplier);
        }

        try
        {
            //moving the health bar
            healthBar.transform.localScale = new Vector2(energyLevel / 200, 12);
            //positionTransform = -185 + (energyLevel / 400);
            healthBar.transform.localPosition = new Vector2((float)(-185 + (energyLevel / 400)) , 170.8f);
        } catch (Exception e){
            
        }
        
    }

    public float getEnergyLevel(){
        return energyLevel;
    }

    //Calculates the energy loss for moving
    public void movementEnergyLoss(float movementEffort){
        energyLevel = energyLevel - (movementEffort * movementEnergyMultiplier);
    }

    //If the cell collects food
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided with something");
        if(collision.gameObject.name == "Circle(Clone)")
        {
            Debug.Log(collision.gameObject.GetComponent<Food>().getEnergyAmount());
            energyLevel = energyLevel + collision.gameObject.GetComponent<Food>().getEnergyAmount();
            collision.gameObject.GetComponent<Food>().overrideDestroy();
            localFoodSpawner.GetComponent<FoodSpawner>().decreaseCount();
            this.GetComponent<MoveToFoodAgent>().AddReward(1f);
        }
    }
}
