using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isOpen;
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void setDoorOpen()
    {
        isOpen = true;
        GetComponent<SpriteRenderer>().color = Color.green;
    }
    public void setDoorClosed()
    {
        isOpen = false;
        GetComponent<SpriteRenderer>().color = Color.black;
    }
    private void nextLevel()
    {
        // TODO: proceed to next level, can only proceed if the door is open
        if(isOpen)
        {

        }
    }
}
