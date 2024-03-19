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

    private const float SHAKE_AMOUNT = 1.0f;
    private const float SHAKE_DURATION = 0.5f;
    private bool isShaking = false;
    private Vector3 originalPosition;


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
            StartShake();
        }
    }


    public void StartShake()
    {
        if (!isShaking)
        {
            StartCoroutine(Shake());
        }
    }

    private IEnumerator Shake()
    {
        isShaking = true;
        originalPosition = transform.localPosition;

        float elapsedTime = 0f;

        while (elapsedTime < SHAKE_DURATION)
        {
            float x = originalPosition.x + Random.Range(-SHAKE_AMOUNT, SHAKE_AMOUNT) * Time.deltaTime;
            float y = originalPosition.y + Random.Range(-SHAKE_AMOUNT, SHAKE_AMOUNT) * Time.deltaTime;

            transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
        isShaking = false;

        // Call FadeOut after shaking
        StartCoroutine(FadeOut());
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
