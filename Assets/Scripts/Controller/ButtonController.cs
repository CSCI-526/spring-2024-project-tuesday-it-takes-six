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
    private Vector3 pressedSize = new Vector3(0.8120f, 0.036081f);
    [SerializeField]
    private GameObject nextLevelDoor;

    private bool isPressed = false;
    
  
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {

        Debug.Log("Button: Corpse enter collision detection area");
        Collider2D collider = collision.collider;
        if (collider.CompareTag("Corpse") && !isPressed)
        {
            GameObject Enemy = collider.gameObject.transform.parent.gameObject;
            // Method 1: put the Enemy directly on to the button
            collider.gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + gameObject.transform.localScale.y * 2);

            isPressed = true;
            transform.localScale = pressedSize;
        }
    }*/

   

    private void OnTriggerEnter2D(Collider2D collider)
    {
        isPressed = true;
        transform.localScale = pressedSize;
        Debug.Log("Button: Button is pressed");
        if (GlobalData.TimeTenseData.IsPresent())
            nextLevelDoor.SendMessage("setDoorOpen");


    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        isPressed = true;
        transform.localScale = pressedSize;
        Debug.Log("Button: Button is being pressed");
        if (GlobalData.TimeTenseData.IsPresent())
            nextLevelDoor.SendMessage("setDoorOpen");

    }


    private void OnTriggerExit2D(Collider2D collider)
    {
        isPressed = false;
        transform.localScale = size;
        Debug.Log("Button: Button is unpressed.");
        if (GlobalData.TimeTenseData.IsPresent())
            nextLevelDoor.SendMessage("setDoorClosed");

       
        
    }


    private void FixedUpdate()
    {
      
    }

    public bool IsPressed()
    {
        return isPressed;
    }
}
