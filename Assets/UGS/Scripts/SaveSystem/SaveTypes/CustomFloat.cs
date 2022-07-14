using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomFloat : MonoBehaviour
{

    public static float operator + (CustomFloat a, float b)
    {
        return a + b;
    }
}
