using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindObjectScript : MonoBehaviour
{
    public Vector2 getNearestItem(GameObject itemToGetTagOf)
    {
        float distanceToClosestItem = Mathf.Infinity;
        GameObject closestItem = null;
        GameObject[] allItems = GameObject.FindGameObjectsWithTag(itemToGetTagOf.tag);

        foreach (GameObject currentItem in allItems)
        {
            if(currentItem.transform.IsChildOf(this.gameObject.transform.parent))
            {
                float distanceToItem = (currentItem.transform.position - this.transform.position).sqrMagnitude;
                if (distanceToItem < distanceToClosestItem)
                {
                    distanceToClosestItem = distanceToItem;
                    closestItem = currentItem;
                }
            }
        }
        if(closestItem != null)
        {
            if(itemToGetTagOf.tag=="Food")
            {
                //Debug.DrawLine(this.transform.position, closestItem.transform.position, Color.green);
            }
            else
            {
                Debug.DrawLine(this.transform.position, closestItem.transform.position, Color.red);
            }
            
            Vector3 forSimplification = closestItem.transform.localPosition - this.transform.localPosition;
            Vector2 forSimplification1 = new Vector2(forSimplification[0], forSimplification[1]);
            return forSimplification1;
        } else 
        {
            return new Vector2 (0,0);
        }
    }

    public float[] directions()
    {
        float[] toReturn = new float[4];

        toReturn[0] = findNearestWall(0,1);
        toReturn[1] = findNearestWall(0,-1);
        toReturn[2] = findNearestWall(1,0);
        toReturn[3] = findNearestWall(-1,0);
        return toReturn;
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

        //UnityEngine.Debug.Log(ray.transform.parent.transform.parent.gameObject.name + "," + this.transform.parent.gameObject.name);
        
        
        try{
            //if they are in the same scene
            if(ray.transform.parent.transform.parent.gameObject.name == this.transform.parent.gameObject.name)
            {
                
                if(directionSide != 0 && directionUp != 0)
                {
                    float tempCalc = Mathf.Sqrt((ray.distance * ray.distance) / 2);
                    directionConverted = new Vector3(tempCalc * directionSide, tempCalc * directionUp);
                }

                //Debug.DrawRay(transform.position + additionalPosition, directionConverted);

                return ray.distance;
            } else 
            {
                return 3000f;
            }
            
        } catch {
            return 3000f;
        }
    }
}
