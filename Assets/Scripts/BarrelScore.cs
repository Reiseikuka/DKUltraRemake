using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelScore : MonoBehaviour
{
    public GameObject FloatingText;


    public void ShowFloatingText()
    {
        Instantiate(FloatingText, transform.position, Quaternion.identity, transform);
    }
}
