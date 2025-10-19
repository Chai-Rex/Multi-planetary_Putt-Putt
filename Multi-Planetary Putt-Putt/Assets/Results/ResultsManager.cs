using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultsManager : MonoBehaviour
{
    public static ResultsManager Instance;

    [SerializeField] private StarData currentLevelStars = new StarData();
    [SerializeField] private GameObject resultsCanvas;
    [SerializeField] private TextMeshProUGUI numberOfPuttsText = new TextMeshProUGUI();
    [SerializeField] private Button retryButton;
    [SerializeField] private Button nextHoleButton;
    private Color starGold;
    private Color starGray;

    private int numberOfPutts = 0;

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
    }

    public void ShowResultsScreen(ELevel resultsForLevel)
    {
        resultsCanvas.SetActive(true);
        if (retryButton != null && nextHoleButton != null)
        {
            retryButton.onClick.AddListener(() => LevelManager.Instance.ReloadCurrentLevel());
            nextHoleButton.onClick.AddListener(() => LevelManager.Instance.PlayLastUnlockedLevel());
        }
        ShowStars(currentLevelStars, LevelManager.Instance.GetLevelStarResults(resultsForLevel));
    }

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
}
