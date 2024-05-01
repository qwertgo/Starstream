using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;

    [SerializeField] private InputFetcher inputFetcher;
    [SerializeField] private PlayableDirector gameStartDirector;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Canvas scoreCanvas;

    private void Start()
    {
        inputFetcher.enabled = false;
        scoreCanvas.enabled = false;
        gameStartDirector.enabled = false;
    }


    public void Play()
    {
        playerController.SelectDifficulty(dropdown.value);
        inputFetcher.enabled = true;
        scoreCanvas.enabled = true;
        gameStartDirector.enabled = true;
        gameObject.SetActive(false);
    }
}
