using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

public class ButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    // private bool isPressed = false;
    private Vector3 size = new Vector3(0.8120f, 0.1636081f);
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        transform.localScale = Pressedsize;
        if(GlobalData.Instance.tt == TimeTense.PRESENT)
            nextLevelDoor.SendMessage("setDoorOpen");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        transform.localScale = Pressedsize;
        if (GlobalData.Instance.tt == TimeTense.PRESENT)
            nextLevelDoor.SendMessage("setDoorOpen");
    }


    void OnTriggerExit2D(Collider2D other)
    {
        // isPressed = false;
        transform.localScale = size;
        if (GlobalData.Instance.tt == TimeTense.PRESENT)
            nextLevelDoor.SendMessage("setDoorClosed");
    }
}
