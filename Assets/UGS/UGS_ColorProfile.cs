using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Color Profile", menuName = "UGS Color Profile")]
public class UGS_ColorProfile : ScriptableObject
{
    [Header("Debug Colors")]
    public Color emptyCellColor;
    public Color fullCellColor;
    public Color corruptedCellColor;

}
