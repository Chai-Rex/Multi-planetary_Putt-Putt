using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public enum ELevel
{
    // ORDER MATTERS HERE
    LevelOne,
    LevelTwo,
    LevelThree,
    LevelFour,
    LevelFive
}

[Serializable]
public struct StarData
{
    public ELevel level;
    public List<Image> stars;
}

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    [SerializeField] private List<Button> levelButtons = new List<Button>();
    [SerializeField] private List<StarData> levelStars = new List<StarData>();
    private List<string> levelNames = new List<string>
    {
        "SampleScene"
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        CreatePlayerPrefs();
        AvailableLevels();
    }

    public void PlayLastUnlockedLevel()
    {
        LoadLevel(PlayerPrefs.GetInt("CompletedLevels"));
    }

    public void CompletedLevel(ELevel levelCompleted)
    {
        switch(levelCompleted)
        {
            case ELevel.LevelOne:
                UpdateLevelCompletedProgress(1);
                break;
            case ELevel.LevelTwo:
                UpdateLevelCompletedProgress(2);
                break;
            case ELevel.LevelThree:
                UpdateLevelCompletedProgress(3);
                break;
            case ELevel.LevelFour:
                UpdateLevelCompletedProgress(4);
                break;
            case ELevel.LevelFive:
                UpdateLevelCompletedProgress(5);
                break;
        }
    }


    public void OpenLevel(ELevel selectedLevel)
    {
        switch(selectedLevel)
        {
            case ELevel.LevelOne:
                LoadLevel(0);
                break;
            case ELevel.LevelTwo:
                LoadLevel(1);
                break;
            case ELevel.LevelThree:
                LoadLevel(2);
                break;
            case ELevel.LevelFour:
                LoadLevel(3);
                break;
            case ELevel.LevelFive:
                LoadLevel(4);
                break;
        }
    }

    public void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LoadLevel(int levelNumber)
    {
        if (!(levelNumber >= 0 && levelNumber < levelNames.Count)) { return; }
        SceneManager.LoadScene(levelNames[levelNumber]);
    }

    private void UpdateLevelCompletedProgress(int levelCompleted)
    {
        if (levelCompleted > PlayerPrefs.GetInt("CompletedLevels"))
        {
            PlayerPrefs.SetInt("CompletedLevels", levelCompleted);
        } 
    }


    private void AvailableLevels()
    {
        if (levelButtons.Count == 0) { return; }

        int availableLevels;
        int unAvailableLevels;

        for (availableLevels = 0; availableLevels <
            PlayerPrefs.GetInt("CompletedLevels"); availableLevels++)
        {
            if (availableLevels >= 0 && availableLevels < levelButtons.Count)
            {
                levelButtons[availableLevels].gameObject.SetActive(true);
                ResultsManager.Instance.ShowStars(levelStars[availableLevels], GetLevelStarResults((ELevel)availableLevels));
                levelButtons[availableLevels].onClick.AddListener(() => LoadLevel(availableLevels));
            }
        }

        for (unAvailableLevels = availableLevels; unAvailableLevels < levelButtons.Count; unAvailableLevels++)
        {
            if (unAvailableLevels >= 0 && unAvailableLevels < levelButtons.Count)
            {
                levelButtons[unAvailableLevels].gameObject.SetActive(false);
                ResultsManager.Instance.ShowStars(levelStars[unAvailableLevels], GetLevelStarResults((ELevel)unAvailableLevels));
            }
        }
    }

    public int GetLevelStarResults(ELevel levelForResult)
    {
        switch(levelForResult)
        {
            case ELevel.LevelOne:
                return PlayerPrefs.GetInt("LevelOneResult");
            default:
                return -1;
        }
    }

    private void CreatePlayerPrefs()
    {
        // COMPLETED LEVELS
        if (!PlayerPrefs.HasKey("CompletedLevels"))
        {
            PlayerPrefs.SetInt("CompletedLevels", 0);
        }

        // LEVEL RESULTS
        if (!PlayerPrefs.HasKey("LevelOneResult"))
        {
            PlayerPrefs.SetInt("LevelOneResult", 0);
        }
    }
}
