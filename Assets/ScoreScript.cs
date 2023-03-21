using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{

    Text currentscore;
    public BarrelDetection barrelscore;
    void Start()
    {
           currentscore = GetComponent<Text>();     
    }

    // Update is called once per frame
    void Update()
    {
        currentscore.text  = "" + barrelscore.score;
        /*For the moment, update the text 
          with the score gained by jumping barrels. Will be updated to 
          add whenever we destroy a barrel or pick items*/
    }
}
