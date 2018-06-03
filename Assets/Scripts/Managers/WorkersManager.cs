﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorkersManager : MonoBehaviour
{
    public GameObject leader;
    public Button myButton;
    public WorkerConfig wc;
    public TileConfig tc;
    public GameData gData;
    public int workerPrice;
    public int wPFactor = 2;

    void Start()
    {
        gData.CoinCount = 0;
        wc.leader = leader;
        wc.onLeaderDeath.AddListener(LeaderDied);
    }

    void Update()
    {
        wc.aheadFollowPoint = wc.workers.Count * (-0.35f);
        workerPrice = wc.workers.Count * wPFactor;

        if (workerPrice > gData.CoinCount)
        {
            myButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            myButton.GetComponent<Button>().interactable = true;
        }
    }

    public void AddWorker()
    {
        GameObject worker = ObjectPooler.instance.GetFromPool(wc.worker);
        float newXPos = Random.Range(leader.transform.position.x - tc.laneWidth, leader.transform.position.x + tc.laneWidth);
        float newZPos = Random.Range(tc.disableSafeDistance + 5, tc.disableSafeDistance + 8);
        worker.transform.position = new Vector3(newXPos, worker.transform.position.y, newZPos);
        wc.workers.Add(worker);
        gData.CoinCount -= workerPrice;
    }

    void LeaderDied()
    {
        if (wc.workers.Count > 0)
        {
            ElectNewLeader();
        }
        else
        {
            gData.gameState = GameState.GameOver;
            gData.onEnd.Invoke();
        }
    }

    public void ElectNewLeader()
    {
        wc.leader = wc.workers[0];
        if (!wc.leader.activeSelf)
        {
            Debug.Log("bad");
        }
        wc.leader.GetComponent<WorkerStrafe>().enabled = true;
        wc.leader.GetComponent<SeekLeaderPosition>().enabled = true;
    }
}
