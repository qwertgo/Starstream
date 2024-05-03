using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;  
using System.IO;
public class ScoreManager : MonoBehaviour
{
    ScoreData scoreData;
    string dataFilePath;
    [ShowNonSerializedField]int currentScore = 0;
    [ShowNonSerializedField]int lastScore;
    [ShowNonSerializedField]int highscore;
    [SerializeField] private int scoreToWinGame;
    
    [SerializeField]TMP_Text scoreDisplay;
    [SerializeField]TMP_Text lastScoreDisplay;
    [SerializeField]TMP_Text highscoreDisplay;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private GameObject youWonScreen;
    [SerializeField] private RectTransform scoreSlider;
    [SerializeField] TextMeshProUGUI loseScreenText;
    [SerializeField] GameObject youLooseScreen;
    private void Start() 
    {
        dataFilePath = Application.persistentDataPath + "/scoreData.json";
        LoadData();
        UpdateDisplay();
    }

    public void IncreaseScore()
    {
        currentScore++;
        if (currentScore >= scoreToWinGame)
        {
            playerController.Stop();
            youWonScreen.SetActive(true);
            scoreDisplay.enabled = false;
            lastScoreDisplay.enabled = false;
            highscoreDisplay.enabled = false;
        }
    }

    private void UpdateLooseScreenText()
    {
        youLooseScreen.SetActive(true);
        loseScreenText.text = $"Collected {currentScore} out of {scoreToWinGame} crystals";
    }

    [Button]
    public void OnDefeat()
    {
        //CheckScore();
        UpdateDisplay();
        UpdateLooseScreenText();
        //SaveData();
    }

    public void UpdateScoreDisplay() 
    { 
        scoreDisplay.text = string.Format("{0:00}|{1:00}", currentScore, scoreToWinGame);
        float percentage = (float)currentScore / scoreToWinGame;
        scoreSlider.localScale = new Vector3(percentage, 1, 1);
    
    }
    

    void UpdateDisplay()
    {
        UpdateScoreDisplay();
        lastScoreDisplay.text = $"Last Score: {lastScore}";
        highscoreDisplay.text = $"Highscore: {highscore}";
    }
    
    void CheckScore()
    {
        if (currentScore > highscore)
            highscore = currentScore;
        
        lastScore = currentScore;
        currentScore = 0;
    }
    
    public void SaveData()
    {
        scoreData.lastScore = lastScore;
        scoreData.highscore = highscore;
        string jsonData = JsonUtility.ToJson(scoreData);
        File.WriteAllText(dataFilePath, jsonData);
    }
    public void LoadData()
    {
        if (File.Exists(dataFilePath))
        {
            string jsonData = File.ReadAllText(dataFilePath);
            scoreData = JsonUtility.FromJson<ScoreData>(jsonData);

            lastScore = scoreData.lastScore;
            highscore = scoreData.highscore;
        }
        else
            scoreData = new ScoreData();
    }
    [Button]
    void DeleteScoreData() => File.Delete(dataFilePath);
}

public class ScoreData
{
    public int highscore;
    public int lastScore;
}
