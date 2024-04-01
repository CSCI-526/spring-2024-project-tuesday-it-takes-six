using Game;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private GameObject playerRB;

    [SerializeField]
    private GameObject rightBoundary;

    private readonly float SMOOTH_MOVING_TIME = 0.2f;
    private readonly Vector3 INIT_POSITION = new(3, 0.5f, 0);

    private Vector3 velocity = Vector3.zero;

    private float rightmostPosition;
    private float prevY = 0;
    private Rigidbody2D rb;

    void Start()
    {
        transform.position = INIT_POSITION;
        rightmostPosition = rightBoundary.transform.position.x - 8;
        rb = playerRB.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        float x = Mathf.Min(playerRB.transform.position.x + 2, rightmostPosition);
        float y = GetYPos();
        float z = playerRB.transform.position.z - 1;

        SmoothMove(new Vector3(x, y, z));

        prevY = y;
    }

    void SmoothMove(Vector3 targetPosition)
    {
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            SMOOTH_MOVING_TIME);
    }

    private float GetYPos()
    {
        // only move y axis when player is not in the air
        if (!Utils.OnGround(rb)) return prevY;

        float y = playerRB.transform.position.y;

        if (y >= 3.5 && y < 8) return 6;

        float t = Mathf.RoundToInt(y / 3f) * 3f + 0.5f;
        return Mathf.Max(t, -10.0f);
    }
}
