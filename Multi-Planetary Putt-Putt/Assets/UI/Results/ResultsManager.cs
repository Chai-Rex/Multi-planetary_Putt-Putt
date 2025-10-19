using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections.Generic;

public class ResultsManager : MonoBehaviour
{
    public static ResultsManager Instance;

    [SerializeField] private ELevel CURRENTLEVEL; 
    [SerializeField] private StarData currentLevelStars = new StarData();
    [SerializeField] private GameObject resultsCanvas;
    [SerializeField] private TextMeshProUGUI numberOfPuttsText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button nextHoleButton;
    [SerializeField] private int twoStarPutts = 6;
    [SerializeField] private int threeStarPutts = 3;

    private Color starGold;
    private Color starGray;

    private int numberOfPutts = 0;
    private bool isInResults = false;

    private AudioSource audioSource;

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

        if (resultsCanvas != null)
        {
            resultsCanvas.SetActive(false);
        }

        ColorUtility.TryParseHtmlString("#949400", out starGold);
        ColorUtility.TryParseHtmlString("#797979", out starGray);

        audioSource = GetComponentInChildren<AudioSource>();
    }

    private void Start()
    {
        if (retryButton == null) { return; }
        if (nextHoleButton == null) { return; }

        retryButton.onClick.AddListener(() => LevelManager.Instance.ReloadCurrentLevel());
        nextHoleButton.onClick.AddListener(() => LevelManager.Instance.PlayLastUnlockedLevel());
    }

    public void ShowResultsScreen(ELevel resultsForLevel)
    {
        /*if (audioSource)
        {
            audioSource.Play();
        }*/

        if (UIManager.Instance.GetIsInSettings())
        {
            UIManager.Instance.SetSettings();
        }
        isInResults = true;
        resultsCanvas.SetActive(true);
        if (retryButton != null && nextHoleButton != null)
        {
            retryButton.onClick.AddListener(() => LevelManager.Instance.ReloadCurrentLevel());
            nextHoleButton.onClick.AddListener(() => LevelManager.Instance.PlayLastUnlockedLevel());
        }

        LevelManager.Instance.CompletedLevel(CURRENTLEVEL);
        ShowStars(currentLevelStars, CheckLevelResults());
    }

    // CALL Before Show Results Screen
    public void SetNumberOfPutts(int _numberOfPutts)
    {
        numberOfPutts = _numberOfPutts;
        numberOfPuttsText.text = numberOfPutts.ToString();
    }

    public void ShowStars(StarData starData, int starsAchieved)
    {
        for (int star = 0; star < starsAchieved; star++)
        {
            starData.stars[star].gameObject.SetActive(true);
            starData.stars[star].color = starGold;
        }
    }

    public bool GetIsInResults()
    {
        return isInResults;
    }

    public void SetIsInResults(bool _isInResults)
    {
        isInResults = _isInResults;
    }

    private int CheckLevelResults()
    {
        if (numberOfPutts <= threeStarPutts)
        {
            SetLevelResult(CURRENTLEVEL, 3);
            return 3;
        }
        else if (numberOfPutts <= twoStarPutts)
        {
            SetLevelResult(CURRENTLEVEL, 2);
            return 2;
        }
        else
        {
            SetLevelResult(CURRENTLEVEL, 1);
            return 1;
        }
    }

    public void SetLevelResult(ELevel currentLevel, int levelResult)
    {
        switch (currentLevel)
        {
            case ELevel.LevelOne:
                if (PlayerPrefs.GetInt("LevelOneResult") < levelResult)
                {
                    PlayerPrefs.SetInt("LevelOneResult", levelResult);
                }
                break;
            case ELevel.LevelTwo:
                if (PlayerPrefs.GetInt("LevelTwoResult") < levelResult)
                {
                    PlayerPrefs.SetInt("LevelTwoResult", levelResult);
                }
                break;
            case ELevel.LevelThree:
                if (PlayerPrefs.GetInt("LevelThreeResult") < levelResult)
                {
                    PlayerPrefs.SetInt("LevelThreeResult", levelResult);
                }
                break;
            case ELevel.LevelFour:
                if (PlayerPrefs.GetInt("LevelFourResult") < levelResult)
                {
                    PlayerPrefs.SetInt("LevelFourResult", levelResult);
                }
                break;
            case ELevel.LevelFive:
                if (PlayerPrefs.GetInt("LevelFiveResult") < levelResult)
                {
                    PlayerPrefs.SetInt("LevelFiveResult", levelResult);
                }
                break;
            case ELevel.LevelSix:
                if (PlayerPrefs.GetInt("LevelSixResult") < levelResult)
                {
                    PlayerPrefs.SetInt("LevelSixResult", levelResult);
                }
                break;
            case ELevel.LevelSeven:
                if (PlayerPrefs.GetInt("LevelSevenResult") < levelResult)
                {
                    PlayerPrefs.SetInt("LevelSevenResult", levelResult);
                }
                break;
            case ELevel.LevelEight:
                if (PlayerPrefs.GetInt("LevelEightResult") < levelResult)
                {
                    PlayerPrefs.SetInt("LevelEightResult", levelResult);
                }
                break;
            case ELevel.LevelNine:
                if (PlayerPrefs.GetInt("LevelNineResult") < levelResult)
                {
                    PlayerPrefs.SetInt("LevelNineResult", levelResult);
                }
                break;

        }
    }

    public ELevel GetCurrentLevel()
    {
        return CURRENTLEVEL;
    }
}
