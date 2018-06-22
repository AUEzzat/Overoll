﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPowerUp : MonoBehaviour {


    ObjectReturner cReturn;
    GameObject gameobj;
    public WorkerConfig wc;
    public GameData gameData;

    void OnEnable()
    {
      
        cReturn = GetComponent<ObjectReturner>();
        gameobj = GetComponent<GameObject>();
    }
   
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Worker")
        {
            if (tag == "Magnet")
            { 
                wc.gotMagnet.Invoke();

            }
            if(tag=="Shield")
            {
                wc.gotShield.Invoke();
            }
           StartCoroutine(cReturn.ReturnToPool(0));
           
        }
    }
}
