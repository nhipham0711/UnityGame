using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DungeonCrawler : MonoBehaviour
{
    public Vector2Int Position { get; set; }
    // Start is called before the first frame update
    public DungeonCrawler(Vector2Int startPos) 
    { Position = startPos; }
    // de Crawler di chuyen
    public Vector2Int Move(Dictionary<Direction, Vector2Int> directionMovementMap) 
    {
        Direction toMove = (Direction)UnityEngine.Random.Range(0, directionMovementMap.Count);
        Position += directionMovementMap[toMove];
        return Position;
    }
}
