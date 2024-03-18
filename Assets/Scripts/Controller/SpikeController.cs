using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;
        if (!collider.CompareTag("Player")) return;

        GlobalData.PlayerStatusData.KillPlayer();
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
