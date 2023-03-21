using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AboveBarrelScoring : MonoBehaviour
{
    private int score;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Barrel")
        {
          IncreaseScore();
        }
    }

    public void IncreaseScore()
    {
        Debug.Log("Mario jumped the barrel!");
        score +=  100;
        Debug.Log(score);
    }
}
