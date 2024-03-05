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
    private GameObject playerRB;

    [SerializeField]
    private GameObject[] objects;
    

    private int instructionIndex = 0;

    // (x position, text, double check by event)[]
    // if `double check by event` is ensured
    //     - if the player haven't reached the x position, it will check the event first
    //     - if player have reached the x position, it will move to next instruction
    private (int, string, bool)[] INSTRUCTIONS = {
        (0, $"Press {KeyMapping.LEFT}/{KeyMapping.RIGHT} to move", false),
        (6, $"Press {KeyMapping.JUMP} to jump", false),
        (20, $"Press {KeyMapping.TIME_SWITCH} to switch time", false),
        (28, "Jump on enemies to kill them", false),
        // idx = 4
        (46, "Push the corpse through the time portal.\nWhen the portal is open, it turns green", true),
        // idx = 5
        (50, "Push the corpse towards the spikes and jump on it to pass the spikes", true),
        (48, "Switch to present, you can see a laser launcher.\nGo close to operate it.", false),
        (58, $"The laser can kill EVERYONE.\nPress {KeyMapping.LASER_ROTATE} near it to rotate the laser.", false),
        (63, "Push the yellow button to open the door", false),
        // idx = 9
        (80, "Pass the green door to reach next level", true)
    };

    void Start()
    {

    }

    void Update()
    {
        if (instructionIndex >= INSTRUCTIONS.Count()) return;

        var (pos, _, triggerByEvent) = INSTRUCTIONS[instructionIndex];

        if (playerRB.transform.position.x >= pos)
        {
            ShowNext();
            return;
        }

        if (triggerByEvent) CheckEvent();
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
                if (!objects[0].GetComponent<EnemyController>().IsAlive()) ShowNext();
                break;
            case 5:
                if (objects[0].GetComponent<EnemyController>().GetTimeTense() == TimeTense.PAST) ShowNext();
                break;
            case 9:
                if (objects[1].GetComponent<ButtonController>().IsPressed()) ShowNext();
                break;
        }
    }
}
