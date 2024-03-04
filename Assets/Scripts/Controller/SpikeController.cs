using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        if (!collider.CompareTag("Player")) return;


        if (collider.transform.parent.TryGetComponent<PlayerController>(out PlayerController playerController))
        {
            playerController.SetDeath(true);
        }
        else
        {
            Debug.LogError("collider's parent do not have a PlayerController script attached!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
