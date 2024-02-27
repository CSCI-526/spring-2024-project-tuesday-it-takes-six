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

    // (distance, text)[]
    private (int, string)[] INSTRUCTIONS = {
        (0, $"Press {KeyMapping.LEFT}/{KeyMapping.RIGHT} to move"),
        (6, $"Press {KeyMapping.JUMP} to jump"),
        (20, $"Press {KeyMapping.TIME_SWITCH} to switch time"),
        (45, "Jump on enemies to kill them"),
        (50, "Now try to push the corpse of the enemy"),
    };

    void Start()
    {

    }

    void Update()
    {
        if (instructionIndex < INSTRUCTIONS.Count() && player.transform.position.x > INSTRUCTIONS[instructionIndex].Item1)
        {
            t.text = INSTRUCTIONS[instructionIndex].Item2;
            instructionIndex ++;
        }
    }
}
