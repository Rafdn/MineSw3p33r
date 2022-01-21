using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [HideInInspector] public int X;
    [HideInInspector] public int Y;
    [HideInInspector] public int Z;
    [HideInInspector] public int mineCount;
    [HideInInspector] public bool opened = false;
    [HideInInspector] public bool flagged = false;
    [HideInInspector] public bool isMine = false;
}