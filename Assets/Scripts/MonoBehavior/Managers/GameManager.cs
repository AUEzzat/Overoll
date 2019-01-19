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
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum GameState
{
    MainMenu,
    Gameplay,
    Pause,
    GameOver
};

/// <summary>
/// Handles In game management and Main menu UI management
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState gameState;

    public IntField difficulty;

    public LanesDatabase lanes;
    
    public Text gamePausedTxt;

    public Sprite pauseSprite;
    public Sprite resumeSprite;

    public Button pauseBtn;
    public Button restartBtn;
    public Button playBtn;
    public Button settingsBtn;
    public Button storeBtn;

    [HideInInspector]
    public UnityEvent OnStart = new UnityEvent();
    [HideInInspector]
    public UnityEvent OnResume;
    [HideInInspector]
    public UnityEvent onPause;
    [HideInInspector]
    public UnityEvent onEnd;

    public Canvas mainMenuCanvas;
    public Canvas inGameCanvas;
    public Canvas endGameCanvas;
   
    private void Awake()
    {
        Debug.Log("gm");
        if (Instance == null)
        {
            Instance = this;
        }
  
        gameState = GameState.MainMenu;
        difficulty.Value= PlayerPrefs.GetInt("PlayedTutorial");

    }

    private void Start()
    {
      
        lanes.ResetLanes();

        inGameCanvas.gameObject.SetActive(false);
        endGameCanvas.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(true);

        gamePausedTxt.gameObject.SetActive(false);

        onEnd.AddListener(EndGame);
    }

    public void PlayBtnEntered()
    {
        mainMenuCanvas.gameObject.SetActive(false);
        inGameCanvas.gameObject.SetActive(true);
        GameStart();
    }

    public void RestartBtnEntered()
    {
        SceneManager.LoadScene("Main");
        endGameCanvas.gameObject.SetActive(false);
    }

    public void PauseBtnEntered()
    {
        switch (gameState)
        {
            case GameState.Gameplay:
                gamePausedTxt.gameObject.SetActive(true);
                pauseBtn.GetComponent<Image>().sprite = resumeSprite;
                GameHalt();
                break;

            case GameState.Pause:
                gamePausedTxt.gameObject.SetActive(false);
                pauseBtn.GetComponent<Image>().sprite = pauseSprite;
                GameResume();
                break;
        }
    }

    public void GameHalt()
    {
        gameState = GameState.Pause;
        onPause.Invoke();
    }

    public void GameResume()
    {
        gameState = GameState.Gameplay;
        OnResume.Invoke();
    }

    void GameStart()
    {
        gameState = GameState.Gameplay;
        OnStart.Invoke();
    }

    /// <summary>
    /// acts as a pause but at the end of the game
    /// </summary>
    void EndGame()
    {
        inGameCanvas.gameObject.SetActive(false);
        mainMenuCanvas.gameObject.SetActive(false);
        endGameCanvas.gameObject.SetActive(true);

        gameState = GameState.GameOver;
       
    }

}
