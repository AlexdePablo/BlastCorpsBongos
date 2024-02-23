using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MBState : MonoBehaviour, IState
{
    public virtual void Init()
    {
        enabled = true;
    }

    public virtual void Exit()
    {
        enabled = false;
    }
}
