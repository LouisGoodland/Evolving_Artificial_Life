using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Food : MonoBehaviour
{
    public GameObject SettingsObject;

    private float energyValue;
    // Start is called before the first frame update
    void Start()
    {
        //makes the food the correct size
        this.gameObject.transform.localScale = new Vector2(15, 15);
        //Sets energy value
        try{
            SettingsObject = GameObject.FindGameObjectWithTag("Settings");
            if(SettingsObject.GetComponent<SettingsScript>().getIsEarlyTraining())
            {
                energyValue = 5000000f;
            }
            else 
            {
                energyValue = 2250f;
            }
        } 
        //no settings object, must be testing
        catch (Exception e)
        {
            //Debug.Log("here");
            energyValue = 2250f;
        }
        
    }

    public void earlyTrainingEnergyMode(){
        energyValue = 500000f;
        this.transform.localPosition = new Vector2(-50, 0);
    }

    public void testingMode(){
        //Debug.Log("regular energy");
        energyValue = 2250f;
    }

    public float getEnergyAmount(){
        return energyValue;
    }

    public void overrideDestroy(){
        Destroy(this.gameObject);
    }
}
