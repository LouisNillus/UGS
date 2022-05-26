using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UGS_Action : ScriptableObject
{
    public virtual void Play(UGS_Grid grid) { }
}
