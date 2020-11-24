using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    public enum DoorType
    { left,right,top,bottom};

    public DoorType doorType;
}
