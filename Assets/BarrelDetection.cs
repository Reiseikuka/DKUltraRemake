using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDetection : MonoBehaviour
{
    public int score;
    /*To store the value gained  either by jumping a barrel, 
    picking items or mashing barrels with hammer */

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Barrel")
        {
          BarrelScore();
        }
        //If Mario has jump a barrel, increase Score

        //TODO: If Mario has picked up an item, increase score according the case(kind of item)
    }

    public void BarrelScore()
    {
        Debug.Log("Mario jumped the barrel!");
        score +=  100;
        Debug.Log(score);
    }


}
