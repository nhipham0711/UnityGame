using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    public static CameraController instance;
    public Room currRoom;
    public float moveSpeedWhenRoomChange;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }
    void UpdatePosition()
    {
        if (currRoom == null) { return; }
   
    Vector3 targetPos = GetCameraPosition();
    transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeedWhenRoomChange);

    }
    Vector3 GetCameraPosition()
    {
        if (currRoom == null) 
        {
            return Vector3.zero;
        }
        Vector3 targetPos = currRoom.GetRoomCenter();
        targetPos.z = transform.position.z;
        return targetPos;
    }

    public bool isSwitchingScene()
    {
        return !transform.position.Equals(GetCameraPosition());
    }
}
