using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityEngine.Audio;
using Unity.Mathematics;
using Random = UnityEngine.Random;

public enum ELevel
{
    // ORDER MATTERS HERE
    LevelOne,
    LevelTwo,
    LevelThree,
    LevelFour,
    LevelFive,
    LevelSix,
    LevelSeven,
    LevelEight
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
    [SerializeField] private Button rightArrowButton;
    [SerializeField] private Button leftArrowButton;
    [SerializeField] private int maxLevelsPerList = 8;
    [SerializeField] private int maxExtraLevelsList = 0;
    private int currentLevelsList = 0;

    [SerializeField]
    private List<AudioResource> musicTracks = new List<AudioResource>();
    private AudioSource audioSource;

    private List<string> levelNames = new List<string>
    {
        "Level1",
        "Level2",
        "Level3",
        "Level4",
        "Level5",
        "Level6",
        "Level7",
        "Level8"
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

        audioSource = GetComponentInChildren<AudioSource>();
        if(audioSource)
        {
            if(musicTracks.Count > 0)
            {
                audioSource.resource = musicTracks[Random.Range(0, musicTracks.Count)];
                audioSource.Play();
            }
        }

        CreatePlayerPrefs();
        DisableAllLevelButtons();
        ShowSelectionArrows();
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
            case ELevel.LevelSix:
                UpdateLevelCompletedProgress(6);
                break;
            case ELevel.LevelSeven:
                UpdateLevelCompletedProgress(7);
                break;
            case ELevel.LevelEight:
                UpdateLevelCompletedProgress(8);
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
            case ELevel.LevelSix:
                LoadLevel(5);
                break;
            case ELevel.LevelSeven:
                LoadLevel(6);
                break;
            case ELevel.LevelEight:
                LoadLevel(7);
                break;
        }
    }

    public void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LoadLevel(int levelNumber)
    {
        if (!(levelNumber >= 0 && levelNumber < levelNames.Count)) 
        {
            LoadMainMenu();
        }
        else
        {
            SceneManager.LoadScene(levelNames[levelNumber]);
        }
            
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

        for (availableLevels = currentLevelsList * maxLevelsPerList; availableLevels <
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

    private void DisableAllLevelButtons()
    {
        if (levelButtons.Count == 0) { return; }
        foreach (Button levelButton in levelButtons)
        {
            levelButton.gameObject.SetActive(false);
            levelButton.onClick.RemoveAllListeners();
        }
    }

    public void NextLevelList()
    {
        if (currentLevelsList < maxExtraLevelsList) { currentLevelsList++; }
        DisableAllLevelButtons();
        ShowSelectionArrows();
        AvailableLevels();
        
    }

    public void PrevLevelList()
    {
        if (currentLevelsList > 0) { currentLevelsList--; }
        DisableAllLevelButtons();
        ShowSelectionArrows();
        AvailableLevels();
    }

    public void ShowSelectionArrows()
    {
        if (leftArrowButton == null) { return; }
        if (rightArrowButton == null) { return; }

        if (currentLevelsList > 0)
        {
            leftArrowButton.gameObject.SetActive(true);
        }
        else
        {
            leftArrowButton.gameObject.SetActive(false);
        }

        if (currentLevelsList < maxExtraLevelsList)
        {
            rightArrowButton.gameObject.SetActive(true);
        }
        else
        {
            rightArrowButton.gameObject.SetActive(false);
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

        if (!PlayerPrefs.HasKey("LevelTwoResult"))
        {
            PlayerPrefs.SetInt("LevelTwoResult", 0);
        }

        if (!PlayerPrefs.HasKey("LevelThreeResult"))
        {
            PlayerPrefs.SetInt("LevelThreeResult", 0);
        }

        if (!PlayerPrefs.HasKey("LevelFourResult"))
        {
            PlayerPrefs.SetInt("LevelFourResult", 0);
        }

        if (!PlayerPrefs.HasKey("LevelFiveResult"))
        {
            PlayerPrefs.SetInt("LevelFiveResult", 0);
        }

        if (!PlayerPrefs.HasKey("LevelSixResult"))
        {
            PlayerPrefs.SetInt("LevelSixResult", 0);
        }

        if (!PlayerPrefs.HasKey("LevelSevenResult"))
        {
            PlayerPrefs.SetInt("LevelSevenResult", 0);
        }

        if (!PlayerPrefs.HasKey("LevelEightResult"))
        {
            PlayerPrefs.SetInt("LevelEightResult", 0);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
