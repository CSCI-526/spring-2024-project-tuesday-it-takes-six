using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Game;


public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private bool isOpen = false;
    void Start()
    {
        if(isOpen) GetComponent<SpriteRenderer>().color = Color.green;
        else GetComponent<SpriteRenderer>().color = Color.black;
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


    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Collision enter");
        // if it is not open or the collider is not player, ignore any entering
        if (!isOpen || !collider.CompareTag("Player")) return;

        CompleteThisLevel();
    }


    private void OnTriggerStay2D(Collider2D collider)
    {
        Debug.Log("Collision enter");
        // if it is not open or the collider is not player, ignore any entering
        if (!isOpen || !collider.CompareTag("Player")) return;

        CompleteThisLevel();

    }

    private void CompleteThisLevel()
    {
        GlobalData.AnalyticsManager.Send("levelCompleted");
        SceneManager.LoadScene("YouWin");
    }
}
