using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class FadingFloorController : MonoBehaviour
{
    [SerializeField]
    [Range(0, 3)]
    private float fadeDuration = 1;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;

        // ignore non player collision
        if (!collider.CompareTag("Player")) return;

        Side side = Utils.DetectCollisionSide(collision, transform);
        if (side == Side.TOP)
        {
            StartCoroutine(FadeOut());
        }
    }


    IEnumerator FadeOut()
    {
        float currentTime = 0;
        Color originalColor = spriteRenderer.color;
        while (currentTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.gameObject.SetActive(false);
    }
}
