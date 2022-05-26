using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UGS_Module : MonoBehaviour
{
    [HideInInspector] public UGS_Grid grid;

    public virtual void Initialize() { }
}
