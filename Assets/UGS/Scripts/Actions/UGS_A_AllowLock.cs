using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGS_A_AllowLock : UGS_Action
{
    public bool allow;

    public override void Play(UGS_Grid grid)
    {
        grid.allowLock = allow;
    }
}
