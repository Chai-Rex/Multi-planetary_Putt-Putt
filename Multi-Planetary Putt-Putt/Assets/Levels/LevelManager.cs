using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityEngine.Audio;
using Unity.Mathematics;
using Random = UnityEngine.Random;
using Unity.VisualScripting;

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
    LevelEight,
    LevelNine,
    MainMenu
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
    private Dictionary<int, List<string>> LevelsDictionary = new Dictionary<int, List<string>>();
    private int currentLevelsList = 0;

    [SerializeField]
    private List<AudioResource> musicTracks = new List<AudioResource>();
    private AudioSource audioSource;

    private List<string> levelNames = new List<string>
    {
        "Level1",
        "Level2",
        "Level3",
        "Level 4",
        "Level 5",
        "Level6",
        "Level7",
        "Level8",
        "LevelExtra1"
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
        currentLevelsList = 0;
        for (int i = 0; i <= maxExtraLevelsList; i++)
        {
            for(int j = 0; j < maxLevelsPerList; j++)
            {
                int index = j;
                index += i * maxLevelsPerList;

                

                if (index < levelNames.Count)
                {
                    if (!LevelsDictionary.ContainsKey(i))
                    {
                        LevelsDictionary.Add(i, new List<string>());
                    }

                    LevelsDictionary[i].Add(levelNames[index]);
                }
            }
        }

        CreatePlayerPrefs();
        DisableAllLevelButtons();   
        AvailableLevels();
    }

    private void Start()
    {
        leftArrowButton.onClick.AddListener(() => PrevLevelList());
        rightArrowButton.onClick.AddListener(() => NextLevelList());
        ShowSelectionArrows();
    }

    public void PlayLastUnlockedLevel()
    {
        int lastUnlockedLevel = PlayerPrefs.GetInt("CompletedLevels") + 1;
        if (lastUnlockedLevel > levelNames.Count)
        {
            lastUnlockedLevel = levelNames.Count;
        }
        LoadLevel(lastUnlockedLevel);
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
            case ELevel.LevelNine:
                UpdateLevelCompletedProgress(9);
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
            case ELevel.LevelNine:
                LoadLevel(8);
                break;
            case ELevel.MainMenu:
                LoadMainMenu();
                break;
        }
    }

    public void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void LoadLevel(int levelNumber)
    {

        if (!(levelNumber >=  0 && levelNumber < levelNames.Count)) 
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
        int updatedAvailableIndex = 0;
        List<string> currentLevels = LevelsDictionary[currentLevelsList];

        for (availableLevels = 0; availableLevels < currentLevels.Count; availableLevels++)
        {
            updatedAvailableIndex = availableLevels + (currentLevelsList * maxLevelsPerList);
            if (updatedAvailableIndex < PlayerPrefs.GetInt("CompletedLevels"))
            {
                if (updatedAvailableIndex >= 0 && updatedAvailableIndex < levelButtons.Count)
                {
                    int levelToSelect = updatedAvailableIndex;
                    levelButtons[levelToSelect].gameObject.SetActive(true);
                    ResultsManager.Instance.ShowStars(levelStars[levelToSelect], GetLevelStarResults((ELevel)levelToSelect));
                    levelButtons[levelToSelect].onClick.AddListener(() => LoadLevel(levelToSelect));
                }
            }
        }
    }

    public int GetLevelStarResults(ELevel levelForResult)
    {
        switch(levelForResult)
        {
            case ELevel.LevelOne:
                return PlayerPrefs.GetInt("LevelOneResult");
            case ELevel.LevelTwo:
                return PlayerPrefs.GetInt("LevelTwoResult");
            case ELevel.LevelThree:
                return PlayerPrefs.GetInt("LevelThreeResult");
            case ELevel.LevelFour:
                return PlayerPrefs.GetInt("LevelFourResult");
            case ELevel.LevelFive:
                return PlayerPrefs.GetInt("LevelFiveResult");
            case ELevel.LevelSix:
                return PlayerPrefs.GetInt("LevelSixResult");
            case ELevel.LevelSeven:
                return PlayerPrefs.GetInt("LevelSevenResult");
            case ELevel.LevelEight:
                return PlayerPrefs.GetInt("LevelEightResult");
            case ELevel.LevelNine:
                return PlayerPrefs.GetInt("LevelNineResult");
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
        Debug.Log($"Incremenet {currentLevelsList}");
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

        if (!PlayerPrefs.HasKey("LevelNineResult"))
        {
            PlayerPrefs.SetInt("LevelNineResult", 0);
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
