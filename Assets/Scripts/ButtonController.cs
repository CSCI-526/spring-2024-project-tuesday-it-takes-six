using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    // Start is called before the first frame update
    // private bool isPressed = false;
    private Vector3 size = new Vector3(0.8120f, 0.1636081f);
    private Vector3 Pressedsize = new Vector3(0.8120f, 0.036081f);
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // TODO: maybe add some code here to specify what kind of objects can press the button
        // isPressed = true;
        transform.localScale = Pressedsize;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // isPressed = false;
        transform.localScale = size;
    }
}