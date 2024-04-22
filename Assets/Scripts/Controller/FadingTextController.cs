using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Once player moves, the text starts to fade
public class FadingTextController : MonoBehaviour
{
    [SerializeField]
    float fadeDuration = 3;
    private SpriteRenderer spriteRenderer;
    private TMP_Text textMeshPro;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = transform.Find("C").gameObject.GetComponent<SpriteRenderer>();
        textMeshPro = transform.Find("ZoomInOutText").gameObject.GetComponent<TMP_Text>();
        Debug.Log("TextMeshPro:", textMeshPro);
    }

    // Update is called once per frame
    void Update()
    {
        // detect user input
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
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
            textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
