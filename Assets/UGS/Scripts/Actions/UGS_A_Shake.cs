using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGS_A_Shake : UGS_Action
{
    public float duration;
    public float magnitude;

    public override void Play(UGS_Grid grid)
    {
        UGS_M_Camera cameraModule = grid.FindModuleOfType<UGS_M_Camera>();
        cameraModule.shakeRoutine = cameraModule.StartCoroutine(cameraModule.Shake(duration, magnitude));
    }


}
