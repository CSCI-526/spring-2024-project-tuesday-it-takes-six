using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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


    void OnCollisionEnter2D(Collision2D collision)
    {
        // if it is not open, ignore any entering
        if (!isOpen) return;
        Collider2D collider = collision.collider;

        // if the collider is not player, ignore them
        if (!collider.CompareTag("Player")) return;

        NextLevel();
    }

    private void NextLevel()
    {
        // TODO: proceed to next level
        SceneManager.LoadScene("YouWin");
    }
}
