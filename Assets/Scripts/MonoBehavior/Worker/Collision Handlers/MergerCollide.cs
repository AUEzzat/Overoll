﻿/*Licensed to the Apache Software Foundation (ASF) under one
or more contributor license agreements.  See the NOTICE file
distributed with this work for additional information
regarding copyright ownership.  The ASF licenses this file
to you under the Apache License, Version 2.0 (the
"License"); you may not use this file except in compliance
with the License.  You may obtain a copy of the License at

  http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing,
software distributed under the License is distributed on an
"AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
KIND, either express or implied.  See the License for the
specific language governing permissions and limitations
under the License.*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergerCollide : IWCollide
{
    WorkerConfig wc;
    int mergedCount = 0;

    public MergerCollide(WorkerConfig wc)
    {
        this.wc = wc;
    }

    public WorkerStateTrigger Collide(Collider collider, ref int health)
    {
        // When the master merger recieves a slave merger it returns it to pool,
        // until the number of leveling up is reached then it outputs
        // the trigger for the next state
        if (collider.CompareTag("SlaveMerger"))
        {
            wc.workers.Remove(collider.GetComponent<WorkerFSM>());
            ICollidable slaveMerger = collider.GetComponent<ICollidable>();
            health += slaveMerger.Gethealth();
            slaveMerger.ReactToCollision(0);
            mergedCount++;
            if(mergedCount >= wc.workersPerLevel - 1)
            {
                mergedCount = 0;
                return WorkerStateTrigger.StateEnd;
            }
        }
        return WorkerStateTrigger.Null;
    }

    public void FixedUpdate(float fixedDeltaTime)
    {

    }

    public void ScriptReset()
    {
        mergedCount = 0;
    }
}