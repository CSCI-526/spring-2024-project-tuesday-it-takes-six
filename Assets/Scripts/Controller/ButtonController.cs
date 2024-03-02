using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class ButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    // private bool isPressed = false;
    [SerializeField]
    private Vector3 size = new Vector3(0.8120f, 0.1636081f);
    [SerializeField]
    private Vector3 Pressedsize = new Vector3(0.8120f, 0.036081f);
    [SerializeField]
    private GameObject nextLevelDoor;
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        
        transform.localScale = Pressedsize;
        if (GlobalData.TimeTenseData.IsPresent())
            nextLevelDoor.SendMessage("setDoorOpen");
        
        
    }

    private void OnTriggerStay2D(Collider2D collider)
    {

        transform.localScale = Pressedsize;
        if (GlobalData.TimeTenseData.IsPresent())
            nextLevelDoor.SendMessage("setDoorOpen");

    }


    private void OnTriggerExit2D(Collider2D collider)
    {
        // isPressed = false;
       
        transform.localScale = size;
        if (GlobalData.TimeTenseData.IsPresent())
            nextLevelDoor.SendMessage("setDoorClosed");
        
        
    }
}
