using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using Game;

public class TutorialInstruction : MonoBehaviour
{
    [SerializeField]
    private TMP_Text t;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private GameObject[] objects;
    

    private int instructionIndex = 0;

    // (x position, text)[]
    // if distance == -1, it is not triggered by movement but by event
    private (int, string)[] INSTRUCTIONS = {
        (0, $"Press {KeyMapping.LEFT}/{KeyMapping.RIGHT} to move"),
        (6, $"Press {KeyMapping.JUMP} to jump"),
        (20, $"Press {KeyMapping.TIME_SWITCH} to switch time"),
        (28, "Jump on enemies to kill them"),
        // idx = 4
        (-1, "Now try to push the corpse of the enemy through the time portal"),
        // idx = 5
        (-1, "Now try to push the corpse of the enemy"),
    };

    // private Dictionary<int, int> EVENT_MAPPING = {}

    void Start()
    {

    }

    void Update()
    {
        if (instructionIndex >= INSTRUCTIONS.Count()) return;

        int pos = INSTRUCTIONS[instructionIndex].Item1;
        if (pos == -1)
        {
            CheckEvent();
        }
        else if (player.transform.position.x >= pos)
        {
            ShowNext();
        }
    }

    private void ShowNext()
    {
        if (instructionIndex >= INSTRUCTIONS.Count())
        {
            t.text = "";
            return;
        }

        t.text = INSTRUCTIONS[instructionIndex].Item2;
        instructionIndex ++;
    }

    private void CheckEvent()
    {
        switch (instructionIndex)
        {
            case 4:
                if (!objects[0].GetComponent<EnemyController>().IsAlive())
                {
                    ShowNext();
                }
                break;
        }
    }
}
