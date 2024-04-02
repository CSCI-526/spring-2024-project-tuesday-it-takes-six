using System.Collections;
using Game;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject playerRB;

    [SerializeField]
    private GameObject rightBoundary;

    private readonly float SMOOTH_MOVING_TIME = 0.2f;
    private readonly Vector3 INIT_POSITION = new(3, 0.5f, 0);
    private const float ZOOM_IN_SIZE = 6f;
    private const float ZOOM_OUT_SIZE = 12f;
    private const float ZOOM_DURATION = .8f;

    private Rigidbody2D rb;
    private Camera cam;

    private float rightmostPosition;
    private Vector3 velocity = Vector3.zero;
    private float prevY = 0;
    private bool zoomInput = false;
    private bool isZooming = false;
    private bool zoomedIn = true; // true -> zoomed in, false -> zoomed out

    private void Start()
    {
        cam = GetComponent<Camera>();

        transform.position = INIT_POSITION;
        rightmostPosition = rightBoundary.transform.position.x - 8;
        rb = playerRB.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!zoomInput && !isZooming) zoomInput = Input.GetButtonDown("Zoom");
    }

    private void FixedUpdate()
    {
        SmoothFollowPlayer();

        if (zoomInput)
        {
            StartCoroutine(Zoom());
            zoomedIn = !zoomedIn;
            zoomInput = false;
        }
    }

    private void SmoothFollowPlayer()
    {
        float x = Mathf.Min(playerRB.transform.position.x + 6, rightmostPosition);
        float y = GetYPos();
        float z = playerRB.transform.position.z - 1;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            new Vector3(x, y, z),
            ref velocity,
            SMOOTH_MOVING_TIME);

        prevY = y;
    }

    private float GetYPos()
    {
        // only move y axis when player is not in the air
        if (!Utils.OnGround(rb)) return prevY;

        float y = playerRB.transform.position.y;

        float t = Mathf.RoundToInt(y / 4f) * 4f;
        return Mathf.Max(t, -10.0f);
    }

    private IEnumerator Zoom()
    {
        isZooming = true;
        float l = zoomedIn ? ZOOM_IN_SIZE : ZOOM_OUT_SIZE;
        float r = zoomedIn ? ZOOM_OUT_SIZE : ZOOM_IN_SIZE;

        float currentTime = 0;
        while (currentTime < ZOOM_DURATION)
        {
            float size = Mathf.Lerp(l, r, currentTime / ZOOM_DURATION);
            cam.orthographicSize = size;
            currentTime += Time.deltaTime;
            yield return null;
        }

        isZooming = false;
    }
}
