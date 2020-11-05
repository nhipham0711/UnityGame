using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomInfo 
{
    public string name;
    //vi tri tuong ung o trong Scene
    public int X;
    public int Y;
}
public class RoomController : MonoBehaviour
{
    // Start is called before the first frame update
    public static RoomController instance;//RoomController la Singleton
    string currentWorldName = "Rooms";
    RoomInfo currentLoadRoomData;
    Room currRoom;
    Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
    public List<Room> loadedRooms = new List<Room>();
    bool isLoadingRoom = false;
    void Awake() 
    {
        instance = this;
    }
    void Start()
    {
       

    }
    void Update() 
    {
        UpdateRoomQueue();
    }
    public void UpdateRoomQueue() 
    { 
        if (isLoadingRoom) { return; }
        if (loadRoomQueue.Count == 0) { return; }
        currentLoadRoomData = loadRoomQueue.Dequeue();
        isLoadingRoom = true;
        StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
    }
    public void LoadRoom(string name, int x, int y)
    {
        if (DoesRoomExist(x, y)) 
        {
            return;
        }
        RoomInfo newRoomData = new RoomInfo();
        newRoomData.name = name;
        newRoomData.X = x;
        newRoomData.Y = y;
        //them thong tin phong moi vao day Queue
        loadRoomQueue.Enqueue(newRoomData);
    }
    //scene can thoi gian nhat dinh de load het,tranh load scene khac truoc
    IEnumerator LoadRoomRoutine(RoomInfo info)
    {
        string roomName = currentWorldName + info.name;
        UnityEngine.AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);
        while (loadRoom.isDone == false)
        {
            yield return null;
        }
    }
    //set room voi vi tri 
    public void RegisterRoom(Room room) 
    {
        if (!DoesRoomExist(currentLoadRoomData.X, currentLoadRoomData.Y))
        {
            room.transform.position = new Vector3(
                currentLoadRoomData.X * room.Width,
                currentLoadRoomData.Y * room.Height,
                0
                );
            room.X = currentLoadRoomData.X;
            room.Y = currentLoadRoomData.Y;
            room.name = currentWorldName + "-" + currentLoadRoomData.name + " " + room.X + "," + room.Y;
            room.transform.parent = transform;
            isLoadingRoom = false;
            if (loadedRooms.Count == 0) { CameraController.instance.currRoom = room; }
            loadedRooms.Add(room);
            room.RemoveUnconnectedDoors();
        }
        else 
        {
            Destroy(room.gameObject);
            isLoadingRoom = false;
        }
    }
    public bool DoesRoomExist(int x, int y) {
        return (loadedRooms.Find(item => item.X == x && item.Y == y)) != null; }
    public void OnPlayerEnterRoom(Room room) 
    {
        CameraController.instance.currRoom = room;
        currRoom = room;
    }
    public Room FindRoom(int x, int y)
    {
        return (loadedRooms.Find(item => item.X == x && item.Y == y));
    }
}
