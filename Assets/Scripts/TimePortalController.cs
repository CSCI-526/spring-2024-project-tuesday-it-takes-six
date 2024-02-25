using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePortalController : MonoBehaviour
{
    private LineDrawer lineDrawer;
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.queriesStartInColliders = false;
        lineDrawer = GetComponent<LineDrawer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
