using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{

    public Text currentscore;
    public Text highscoreText;

    private int highscore = 100;
    private int playerscore;

    public BarrelDetection barrelscore;

    private void Start()
    {
           currentscore = GetComponent<Text>(); 

           playerscore =  int.Parse(currentscore.text);
           highscoreText.text = highscore.ToString();
    }

    // Update is called once per frame
    private void Update()
    {
        currentscore.text  = "" + barrelscore.score;
        /*For the moment, update the text 
          with the score gained by jumping barrels. Will be updated to 
          add whenever we destroy a barrel or pick items*/
        //GameScore = int.Parse(currentscore.text);
        playerscore =  int.Parse(currentscore.text);

        if (playerscore > highscore)
        {
            highscore = playerscore;
            highscoreText.text  = currentscore.text;
        }
    }

}
