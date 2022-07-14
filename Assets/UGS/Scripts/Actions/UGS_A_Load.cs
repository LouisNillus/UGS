using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UGS_A_Load : UGS_Action
{
    public override void Play(UGS_Grid grid)
    {
        UGS_M_Library libraryModule = grid.FindModuleOfType<UGS_M_Library>();
        libraryModule.LoadSavedGrid(0);
    }
}
