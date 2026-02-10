using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CubeBase", menuName = "Scriptable Objects/CubeBase")]
public class CubeBase : ScriptableObject
{
    public int score = 2;
    public int maxCubes = 8;
    public int initialCubes = 4;
    public float spawnCooldown = 5.0f;
    public float spawnRadius = 10.0f;
    public float spawnHeight = 10.0f;

    [NonSerialized] public float spawnTimer;
    [NonSerialized] public int cubeCounter;
    //public float spawnTimer;
    //public int cubeCounter;
}
