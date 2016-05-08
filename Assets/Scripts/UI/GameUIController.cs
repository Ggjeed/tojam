﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUIController : MonoBehaviour {

    public List<GameObject> gameUiComponents;
    public List<GameObject> gameMenuComponents;

    public float gameTimer = 300.0f; // in seconds

	// Use this for initialization
	void Start () {
        EnableGameMenu();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void EnableGameMenu()
    {
        foreach (GameObject ui in gameUiComponents)
        {
            ui.SetActive(false);
        }

        foreach (GameObject ui in gameMenuComponents)
        {
            ui.SetActive(true);
        }
    }

    public void EnableGameUi()
    {
        foreach (GameObject ui in gameUiComponents)
        {
            ui.SetActive(true);
        }

        foreach (GameObject ui in gameMenuComponents)
        {
            ui.SetActive(false);
        }
    }

    public void StartDialogue(int _index)
    {
        foreach(GameObject ui in gameUiComponents)
        {
            uiDialogueComponent dialogueComponent = ui.GetComponent<uiDialogueComponent>();
            if(dialogueComponent)
            {
                dialogueComponent.DisplayDialogue(_index);
            }
        }
    }

    public void StartTimer()
    {
        foreach (GameObject ui in gameUiComponents)
        {
            uiGameTimerController gameTimeController = ui.GetComponent<uiGameTimerController>();
            if (gameTimeController)
            {
                gameTimeController.SetLevelTime(gameTimer);
                gameTimeController.StartTimer();
            }
        }
    }

    public void StopTimer()
    {
        foreach (GameObject ui in gameUiComponents)
        {
            uiGameTimerController gameTimeController = ui.GetComponent<uiGameTimerController>();
            if (gameTimeController)
            {
                gameTimeController.StartTimer(false);
            }
        }
    }

    public void SetPlayerLives(int _currentLives)
    {
        foreach (GameObject ui in gameUiComponents)
        {
            uiLifeDisplayController lifeDisplayController = ui.GetComponent<uiLifeDisplayController>();
            if (lifeDisplayController)
            {
                lifeDisplayController.UpdateLife(_currentLives);
            }
        }
    }
}
