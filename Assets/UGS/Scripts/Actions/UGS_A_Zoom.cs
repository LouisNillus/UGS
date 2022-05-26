using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGS_A_Zoom : UGS_Action
{
    public ZoomType zoomType;

    public bool zoomToPointer;
    public bool unzoomToCenter;
    public int step;

    public override void Play(UGS_Grid grid)
    {
        grid.FindModuleOfType<UGS_M_Camera>()?.Zoom(zoomType, step, zoomToPointer);
    }
}
