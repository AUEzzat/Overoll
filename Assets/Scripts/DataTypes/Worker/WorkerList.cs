﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerList : List<WorkerFSM>
{
    public bool Merging = false;
    List<int> normWorkersHealth;

    int minWorkersToMerge; //workers in each level
    int levels; //levels for merging

    bool shieldOn = false;
    bool magnetOn = false;

    WorkerFSM ascender;
    List<List<WorkerFSM>> workers = new List<List<WorkerFSM>>();

    public WorkerList(int workersPerLevel, int levels)
    {
        normWorkersHealth = new List<int>();

        this.minWorkersToMerge = workersPerLevel;
        this.levels = levels;

        for (int i = 0; i < levels; i++)
        {
            workers.Add(new List<WorkerFSM>());
        }
    }

    public new void Add(WorkerFSM worker)
    {
        //add worker normal health
        normWorkersHealth.Add(worker.health);
        base.Add(worker);

        //if there is a power up apply it to worker
        if (shieldOn)
        {
            worker.health = 1000;
            worker.helmetMaterial.SetFloat("_ExtAmount", 0.0001f);
        }
        else if (magnetOn)
        {
            worker.helmetMaterial.SetFloat("_ColAmount", -0.001f);
        }

        //if the leader is added after succeding
        //add him at the top of the level
        if (worker.currentState == WorkerState.Leader)
            workers[worker.level].Insert(0, worker);
        else
            workers[worker.level].Add(worker);

        //if worker is at last level don't merge
        if (Merging || worker.level == levels - 1)
        {
            return;
        }

        //if the workers in current level match minimum workers to merge
        //start merging
        if (workers[worker.level].Count >= minWorkersToMerge)
        {
            Merging = true;

            int newMasterHealth = 0;

            workers[worker.level][0].ChangeState(WorkerStateTrigger.Merge);
            ascender = workers[worker.level][0];

            for (int i = 1; i < minWorkersToMerge; i++)
            {
                workers[worker.level][i].SetSeekedMaster(ascender.transform);
                workers[worker.level][i].ChangeState(WorkerStateTrigger.SlaveMerge);
                newMasterHealth += workers[worker.level][i].health;
            }
            newMasterHealth = newMasterHealth >= 1000 ? newMasterHealth / 1000 : newMasterHealth;
            normWorkersHealth[IndexOf(worker)] = newMasterHealth;
        }
    }

    public new void Remove(WorkerFSM worker)
    {
        normWorkersHealth.Remove(IndexOf(worker));
        base.Remove(worker);

        worker.helmetMaterial.SetFloat("_ExtAmount", 0);
        worker.helmetMaterial.SetFloat("_ColAmount", 0);

        workers[worker.level].Remove(worker);
    }

    //called after merging is finished
    public void Ascend()
    {
        Merging = false;

        if (ascender.level < levels)
        {
            Remove(ascender);
            ascender.level++;
            Add(ascender);
        }
    }

    public WorkerFSM GetNewLeader()
    {
        for (int i = 0; i < workers.Count; i++)
        {
            if (workers[i].Count > 0)
            {
                workers[i][0].ChangeState(WorkerStateTrigger.Succeed);
                return workers[i][0];
            }
        }
        return null;
    }

    public void StartShieldPowerup()
    {
        shieldOn = true;
        for (int i = 0; i < Count; i++)
        {
            this[i].health = 1000;
            this[i].helmetMaterial.SetFloat("_ExtAmount", 0.0001f);
        }
    }

    public void StartMagnetPowerup()
    {
        magnetOn = true;
        for (int i = 0; i < Count; i++)
        {
            this[i].helmetMaterial.SetFloat("_ColAmount", -0.001f);
        }
    }

    public void EndShieldPowerup()
    {
        shieldOn = false;
        for(int i = 0; i < Count; i++)
        {
            this[i].health = normWorkersHealth[i];
            this[i].helmetMaterial.SetFloat("_ExtAmount", 0);
        }
    }

    public void EndMagnetPowerup()
    {
        magnetOn = false;
        for(int i = 0; i < Count; i++)
        {
            this[i].helmetMaterial.SetFloat("_ColAmount", 0);
        }
    }
}
