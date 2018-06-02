﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvGenerator : MonoBehaviour, iHalt
{
    public Vector3 shift;
    public float disFromPlayer = 10;
    EnvPooler pool;
    Transform lastTile;

    bool isHalt;

    public TileConfig tc;

    public GameData gameData;

    /// <summary>
    /// t = d/v, d per prefab
    /// </summary>

    private void OnEnable()
    {
        RegisterListeners();
        isHalt = true;
    }

    void Start()
    {
        pool = gameObject.GetComponent<EnvPooler>();
        lastTile = this.transform;
        //StartCoroutine(GenerateCoroutine());
    }

    private void Update()
    {
        if (!isHalt && pool.activeTileCount < 8)
        {
            GenerateTile();
        }
    }

    void GenerateTile()
    {
        var obj = pool.GetObjectFromPool();
        obj.transform.position = lastTile.transform.position + shift;
        lastTile = obj.transform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5F);
        Gizmos.DrawCube(transform.position, new Vector3(10, 10, 1));
    }

    public void RegisterListeners()
    {
        gameData.OnStart.AddListener(Begin);
        gameData.onPause.AddListener(Halt);
        gameData.OnResume.AddListener(Resume);
        gameData.onEnd.AddListener(End);
    }

    public void Begin()
    {
        isHalt = false;
    }

    public void Halt()
    {
        isHalt = true;
    }

    public void Resume()
    {
        isHalt = false;
    }

    public void End()
    {
        isHalt = true;
    }
}
