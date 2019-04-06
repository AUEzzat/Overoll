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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object used with the pooler, generated from scriptable object
/// </summary>
[Serializable]
public class PoolableObj
{
    public PoolableType type;
    public int count;
    public GameObject prefab;

    public string Name
    {
        get
        {
            if (type == null)
                return null;
            return type.name;
        }
    }

    public PoolableObj(PoolableType _type, int _count, GameObject _prefab)
    {
        type = _type;
        count = _count;
        prefab = _prefab;
    }
}
