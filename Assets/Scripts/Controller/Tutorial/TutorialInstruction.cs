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
    public GameObject player;

    private int instructionIndex = 0;

    // (x position, text)[]
    // if distance == -1, it is not triggered by movement but by event
    private (int, string)[] INSTRUCTIONS = {
        (0, $"Press {KeyMapping.LEFT}/{KeyMapping.RIGHT} to move"),
        (6, $"Press {KeyMapping.JUMP} to jump"),
        (20, $"Press {KeyMapping.TIME_SWITCH} to switch time"),
        (28, "Jump on enemies to kill them"),
        (-1, "Now try to push the corpse of the enemy"),
    };

    void Start()
    {

    }

    void Update()
    {
        if (instructionIndex >= INSTRUCTIONS.Count()) return;

        int pos = INSTRUCTIONS[instructionIndex].Item1;
        if (pos != -1 && player.transform.position.x >= pos)
        {
            ShowNext();
        }
    }

    public void ShowNext()
    {
        if (instructionIndex >= INSTRUCTIONS.Count())
        {
            t.text = "";
            return;
        }

        t.text = INSTRUCTIONS[instructionIndex].Item2;
        instructionIndex ++;
    }
}
