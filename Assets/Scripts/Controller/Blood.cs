using UnityEngine;

public class Blood : MonoBehaviour
{
    private void Update()
    {
        Destroy(gameObject, 2f);
    }
}
