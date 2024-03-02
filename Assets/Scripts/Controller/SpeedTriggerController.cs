using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedTriggerController : MonoBehaviour
{
    [SerializeField]
    private Vector2 Velocity; // speed of the trigger field can be set in inspector
    
    private HashSet<GameObject> Objects = new HashSet<GameObject>(); // objects in speed zoom that need to be triggered velocity
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // So far the speed trigger is only applied to enemy corpse
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Corpse"))
        {
            Debug.Log("Speed Trigger: An Enemy Corpse enter a speed trigger zoom");
            // check if the child object is corpse incase it is body
            Objects.Add(collider.gameObject);
        }

    }


    private void OnTriggerExit2D(Collider2D collider)
    {
        // remove the object from the hashset
        // assume only coprse are removed
        if(collider.CompareTag("Corpse"))
        {
            Debug.Log("Speed Trigger: An Enemy Corpse leaves a speed trigger zoom");
            Objects.Remove(collider.gameObject);
        }
        
    }


    private void FixedUpdate()
    {
        
        foreach (GameObject o in Objects)
        {
            o.transform.parent.gameObject.GetComponent<Rigidbody2D>().velocity = Velocity;
            /*o.transform.gameObject.GetComponent<Rigidbody2D>().velocity = Velocity;*/
        }


    }
}
