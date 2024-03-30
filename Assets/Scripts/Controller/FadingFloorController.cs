using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Game;
using UnityEngine;

public class FadingFloorController : MonoBehaviour
{
    [SerializeField]
    [Range(0, 3)]
    private float fadeDuration = 1;

    private SpriteRenderer spriteRenderer;

    private const float SHAKE_AMOUNT = 0.02f;
    private Vector3 originalPosition;

    private bool isStepped = false;


    // Start is called before the first frame update
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(Shake());
    }

    private void OnEnable()
    {
        StartCoroutine(Shake());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if (isStepped)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider = collision.collider;

        // ignore non player collision
        if (!collider.CompareTag("Player")) return;

        Side side = Utils.DetectCollisionSide(collision, transform);
        if (side == Side.TOP)
        {
            StopCoroutine(Shake());
            isStepped = true;
            StartCoroutine(FadeOut());
        }
    }


    private IEnumerator Shake()
    {
        originalPosition = transform.localPosition;

        while (true)
        {
            float x = originalPosition.x + Random.Range(-SHAKE_AMOUNT, SHAKE_AMOUNT);
            float y = originalPosition.y + Random.Range(-SHAKE_AMOUNT, SHAKE_AMOUNT);

            transform.localPosition = new Vector3(x, y, originalPosition.z);

            yield return null;
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

        gameObject.SetActive(false);
    }
}
