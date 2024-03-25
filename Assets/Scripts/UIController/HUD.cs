using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    [SerializeField]
    private TMP_Text level;

    // Start is called before the first frame update
    private void Start()
    {
        level.text = SceneManager.GetActiveScene().name;
    }
}
