using Game;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    [SerializeField]
    private GameObject levels;
    private readonly Color INACTIVE_COLOR = new(255, 255, 255, 0.3f);

    private void Start()
    {
        InitLevels();
    }

    private void InitLevels()
    {
        int maxLevelReached =  GlobalData.LevelData.GetMaxLevelReached();
        // if the player finished the first two level, the player can start from any level
        int maxEnabledLevels = maxLevelReached <= 2
            ? maxLevelReached
            : GlobalData.LevelData.LEVEL_COUNT;

        for (int i = 0; i < GlobalData.LevelData.LEVEL_COUNT; i ++)
        {
            var level = levels.transform.GetChild(i).gameObject;
            var img = level.GetComponent<Image>();
            var btn = level.GetComponent<Button>();
            var text = level.transform.GetChild(0).GetComponent<Text>();

            if (i >= maxEnabledLevels)
            {
                img.color = INACTIVE_COLOR;
                text.color = INACTIVE_COLOR;
                btn.enabled = false;
            }
            else
            {
                img.color = Color.white;
                text.color = Color.white;
                btn.enabled = true;
            }
        }
    }


    public void StartLevel(int x)
    {
        GlobalData.Init();
        GlobalData.CheckPointData.ResetCheckPoint();
        var sceneName = GlobalData.LevelData.StartLevel(x);
        GlobalData.CheckPointData.SetCurrentSceneName(sceneName);
    }
}
