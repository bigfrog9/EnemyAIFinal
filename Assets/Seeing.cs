using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class Seeing : MonoBehaviour
{
    public State state;



    public GameObject Player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject==Player)
        {
            Debug.Log("SEEN!");
            state.Seen = true;
            
        }
    }

    public void OnTriggerExit(Collider other)
   {
         if (other.gameObject == Player)
         {
            Debug.Log("Unseen");
            state.Seen = false;
         }
    }
}
