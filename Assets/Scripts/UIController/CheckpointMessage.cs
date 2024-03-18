using UnityEngine;
using TMPro; // Import the TMPro namespace
using System.Collections;
using Game;

public class CheckpointMessage : MonoBehaviour
{
    public TMP_Text checkpointText; // Reference to the "Checkpoint Reached!" TMP_Text component
    public TMP_Text savingProgressText; // Reference to the "Saving progress..." TMP_Text component
    public Vector3 screenOffset = new Vector3(0, 30, 0); // Offset of the checkpoint text in screen space
    public float fadeDuration = 2f; // Duration of the fade effect in seconds for checkpoint text
    public float displayDuration = 2f; // Duration to display the "Saving progress..." text

    private Camera mainCamera; // Reference to the main camera
    private Subscriber<Vector3?> subscriber;

    private void Start()
    {
        checkpointText.gameObject.SetActive(false); // Hide checkpoint text initially
        savingProgressText.gameObject.SetActive(false); // Hide saving progress text initially
        mainCamera = Camera.main; // Get the main camera

        subscriber = GlobalData.CheckPointData.CreateLastCheckPointPositionSubscriber();
        subscriber.Subscribe(OnCheckPointEnter);
    }

    public void OnDestroy()
    {
        subscriber.Unsubscribe(OnCheckPointEnter);
    }

    private void OnCheckPointEnter(Vector3? position)
    {
        if (position is null) return;

        PositionTextAboveCheckpoint();
        checkpointText.gameObject.SetActive(true); // Show the checkpoint text
        StartCoroutine(FadeOutText(checkpointText)); // Start fading out the checkpoint text

        // Display the saving progress text without fading
        savingProgressText.gameObject.SetActive(true);
        Invoke("HideSavingProgressText", displayDuration); // Hide the saving progress text after the duration
    }

    void PositionTextAboveCheckpoint()
    {
        // Convert the world position of the checkpoint to a screen position for the checkpoint text
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(transform.position) + screenOffset;
        checkpointText.transform.position = screenPosition;
    }

    IEnumerator FadeOutText(TMP_Text text)
    {
        float currentTime = 0;
        Color originalColor = text.color;
        while (currentTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, currentTime / fadeDuration);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            currentTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        text.gameObject.SetActive(false); // Optionally hide the text after fading out
        text.color = originalColor; // Reset the color to ensure it's visible next time
    }

    void HideSavingProgressText()
    {
        savingProgressText.gameObject.SetActive(false); // Hide the "Saving progress..." text
    }
}
