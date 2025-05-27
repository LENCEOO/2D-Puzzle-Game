using IG.Controller;
using IG.UI;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Managers communication between all UI scripts and manager scripts
/// </summary>
public class UIManager : MonoBehaviour
{
    public GameUI gameUI;
    public LevelScrollView levelScrollView;
    public ResultUI resultUI;

    public LevelManager LevelManager { get; private set; }
    public Text timerText;
    private DatabaseManager _databaseManager;

    // 총점 및 완료 텍스트
    public TextMeshProUGUI totalScoreText;
    public TextMeshProUGUI completionText;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void Initialize(LevelManager levelManager, DatabaseManager databaseManager)
    {
        LevelManager = levelManager;
        _databaseManager = databaseManager;

        levelScrollView?.Initialize(this, _databaseManager.LastUnlockedLevel);
        resultUI?.Initialize(this);
    }
    // totalscore 표시
    public void ShowTotalScore()
    {
        Debug.Log($"🔥 ShowTotalScore 실행됨");
        if (totalScoreText != null && DatabaseManager.Instance != null)
        {
            int totalScore = DatabaseManager.Instance.GetTotalScore();
            Debug.Log($"✅ 총점 계산됨: {totalScore}");
            totalScoreText.text = "Total_Score: " + totalScore;
            totalScoreText.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning("⚠️ totalScoreText 또는 DatabaseManager.Instance가 null입니다.");
        }
    }

    public void ShowCompletionMessage(string message)
    {
        if (completionText != null)
        {
            completionText.gameObject.SetActive(true);
            completionText.text = message;
        }
        else
        {
            Debug.LogWarning("[UIManager] completionText가 설정되지 않았습니다.");
        }
    }

    public void UpdateTimerUI(float time)
    {
        if (timerText != null)
            timerText.text = "Time: " + Mathf.CeilToInt(time).ToString();
    }
}