using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class iceView : MonoBehaviour
{
    
    public float[] wallDistances()
    {
        //makes a new list of wall distances
        int[] wallDistances = new int[9];
        
        for(int width = -1; width < 2; width++)
        {
            for(int height = -1; height < 2; height++)
            {
                if(height != 0 || width != 0){

                    wallDistances[(width * 3) + height + 4] = (int)this.GetComponent<iceView>().findNearestWall(width,height);
                }
            }
        }
        float[] wallDistancesFloat = new float[9];
        string test = "";
        for (int i=0; i < 9; i++)
        {
            wallDistancesFloat[i] = (float)wallDistances[i];
            test = test + wallDistances[i] + ", ";
        }
        //UnityEngine.Debug.Log(test);

        return wallDistancesFloat;
    }


    public float findNearestWall(int directionUp, int directionSide)
    {

        //Gets the x and y dimensions
        float xDim = this.GetComponent<BoxCollider2D>().bounds.size[0];
        float yDim = this.GetComponent<BoxCollider2D>().bounds.size[1];

        //gets the additional position change in the x and y dimensions
        float additionalX = directionSide * (xDim / 2) + (directionSide * 1);
        //Debug.Log(additionalX);
        float additionalY = directionUp * (yDim / 2) + (directionUp * 1);
        //Debug.Log(additionalY);

        //sets up the raycasting details
        Vector3 additionalPosition = new Vector3(additionalX, additionalY, 0);
        
        Vector3 direction = new Vector3(2000 * directionSide, 2000 * directionUp, 0);

        RaycastHit2D ray = Physics2D.Raycast(transform.position + additionalPosition, direction);

        Vector3 directionConverted = new Vector3(ray.distance * directionSide, ray.distance * directionUp);
        
        
        try{
            //if they are in the same scene
            if(ray.transform.parent.gameObject.name == this.transform.parent.gameObject.name)
            {
                
                if(directionSide != 0 && directionUp != 0)
                {
                    float tempCalc = Mathf.Sqrt((ray.distance * ray.distance) / 2);
                    directionConverted = new Vector3(tempCalc * directionSide, tempCalc * directionUp);
                }
                Debug.DrawRay(transform.position + additionalPosition, directionConverted);
                return ray.distance;
            } else 
            {
                return 1000f;
            }
            
        } catch {
            return 1000f;
        }
    }

    public Vector2 getNextCheckpointLocation(int checkpointsPassed)
    {
        GameObject nextCheckPoint = null;
        string checkpointName = "Checkpoint (" + checkpointsPassed.ToString() + ")";
        string environmentName = this.transform.parent.gameObject.name;


        GameObject[] list = GameObject.FindGameObjectsWithTag("CheckPoint");


        foreach(GameObject t in list)
        {
            if(t.name == checkpointName)
            {
                if(t.gameObject.transform.parent.gameObject.name == environmentName)
                {
                    nextCheckPoint = t;
                }
                
            }

        }
        
        if(nextCheckPoint != null)
        {
            Debug.DrawLine(this.transform.position, nextCheckPoint.GetComponent<BoxCollider2D>().ClosestPoint(this.transform.position), Color.green);
            Vector2 vector2DPosition = new Vector2(this.transform.position[0], this.transform.position[1]);
            //Debug.Log(nextCheckPoint.GetComponent<BoxCollider2D>().ClosestPoint(this.transform.position) - vector2DPosition);
            Vector3 toRound = nextCheckPoint.GetComponent<BoxCollider2D>().ClosestPoint(this.transform.position) - vector2DPosition;
            Vector2 toReturn = new Vector2((int)toRound[0], (int)toRound[1]);
            //UnityEngine.Debug.Log(toReturn);
            return toReturn;
        } 
        else
        {
            return new Vector3 (0,0,0);
        }
    }
}
