using UnityEngine;

public class Player : MonoBehaviour
{
    
    public void OnDie()
    {
        //Destroy for now, but later will play death animations
        Destroy(gameObject);
    }
    
}
