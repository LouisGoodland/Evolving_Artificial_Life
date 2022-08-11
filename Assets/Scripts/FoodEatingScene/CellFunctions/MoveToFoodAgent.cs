using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.Barracuda;

public class MoveToFoodAgent : Agent
{
    public GameObject foodSpawner;
    public float previousReward;

    public GameObject wallPiece;
    public GameObject foodPiece;

    public bool hasTrained;
    public NNModel model;

    public override void OnEpisodeBegin()
    {
        this.GetComponent<CellEnergyManagement>().reset();
        transform.localPosition = new Vector3(0, 0, 0);
    }
    
    public override void CollectObservations(VectorSensor sensor)
    {

        Vector2 toFood = this.GetComponent<FindObjectScript>().getNearestItem(foodPiece);
        float distance = Mathf.Sqrt((toFood[0] * toFood[0]) + (toFood[1] * toFood[1]));
        int distanceRewardPoint = 390;

        
        float[] observations = this.GetComponent<FindObjectScript>().directions();
        //UnityEngine.Debug.Log(toFood);
        //UnityEngine.Debug.Log(observations[0] + ", " + observations[1] + ", " + observations[2] + ", " + observations[3] + ",F: " + toFood[0] + ", " + toFood[1]);
        sensor.AddObservation(toFood);
        sensor.AddObservation(observations);
        //sensor.AddObservation(this.GetComponent<CellEnergyManagement>().getEnergyLevel());
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {

        //Debug.Log(actionBuffers.ContinuousActions[1]);
        //Debug.Log(actionBuffers.ContinuousActions[0]);
        //transform.position = transform.position + new Vector3(actionBuffers.ContinuousActions[0], actionBuffers.ContinuousActions[1], 0);
        transform.position = transform.position + new Vector3(actionBuffers.DiscreteActions[0] - 1, actionBuffers.DiscreteActions[1] - 1, 0);
        this.GetComponent<CellEnergyManagement>().movementEnergyLoss(Mathf.Max(actionBuffers.ContinuousActions[0], actionBuffers.ContinuousActions[1]));

        float energy = this.GetComponent<CellEnergyManagement>().getEnergyLevel();

        if (energy > 50000)
        {
            Debug.Log("won!");
            //AddReward(5000000f);
            resetSystem();
        }

        if (energy < 0)
        {
            Debug.Log("lost");
            //SetReward(-50000f);
            resetSystem();
        }

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        continuousActionsOut[0] = Input.GetAxis("Horizontal");
        continuousActionsOut[1] = Input.GetAxis("Vertical");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall")
        {
            Debug.Log("lost");
            AddReward(-1f);
            resetSystem();
        }
    }

    public void resetSystem()
    {
        foodSpawner.GetComponent<FoodSpawner>().resetFood();
        EndEpisode();
    }

    void Update()
    {
        /*
        if(hasTrained)
        {
            //string[] inputNames = model.inputs;   // query model inputs
            //string[] outputNames = model.outputs; // query model outputs
            //UnityEngine.Debug.Log(inputNames[0]);
        }
        */
    }

}
