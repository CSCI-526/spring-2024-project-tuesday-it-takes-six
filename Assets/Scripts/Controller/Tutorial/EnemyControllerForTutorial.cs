using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControllerForTutorial: EnemyController
{
    protected new bool Die() {
        // TODO: trigger next tutorial instruction
        bool status = base.Die();
        if (status)
        {
            GetComponent<TutorialInstruction>()?.ShowNext();
        }

        return status;
    }
}
