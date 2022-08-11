using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{
    private static Boolean objectMade;
    

    int foodSpawnRate;
    float baseEnergyMultiplier;
    float movementEnergyMultiplier;
    int foodCountLimit;
    Boolean earlyTraining;

    public Slider foodSpawnRateSlider;
    public Slider baseEnergyMultiplierSlider;
    public Slider movementEnergyMultiplierSlider;
    public Slider foodCountSlider;

    public Text foodSpawnRateText;
    public Text baseEnergyMultiplierText;
    public Text movementEnergyMultiplierText;
    public Text foodCountText;
    
    void Awake()
    {
        Debug.Log("heree");
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(objectMade);
        if(objectMade == false){
            earlyTraining = false;
            DontDestroyOnLoad(this.gameObject);
            objectMade = true;
        } else {
            Destroy(this.gameObject);
        }
    }

    void fetchMenuItems()
    {
        Debug.Log("fetchin menu items");
    }

    //constantly checks the value
    void Update()
    {
        Debug.Log("updating text");
        Debug.Log(baseEnergyMultiplierSlider.value);
        baseEnergyMultiplier = baseEnergyMultiplierSlider.value;
        baseEnergyMultiplierText.text = baseEnergyMultiplier.ToString();

        foodCountLimit = (int)foodCountSlider.value;
        foodCountText.text = foodCountLimit.ToString();

        foodSpawnRate = (int)foodSpawnRateSlider.value;
        foodSpawnRateText.text = foodSpawnRate.ToString();
        
        movementEnergyMultiplier = movementEnergyMultiplierSlider.value;
        movementEnergyMultiplierText.text = movementEnergyMultiplier.ToString();

    }

    public int getFoodSpawnRate()
    {
        return foodSpawnRate;
    }

    public float getBaseEnergyMultiplier()
    {
        return baseEnergyMultiplier;
    }

    public float getMovementEnergyMultiplier()
    {
        return movementEnergyMultiplier;
    }

    public int getFoodCountLimit()
    {
        return foodCountLimit;
    }

    public Boolean getIsEarlyTraining()
    {
        return earlyTraining;
    }

}
