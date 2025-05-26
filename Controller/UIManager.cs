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

    public LevelManager LevelManager {get; private set;}
    public Text timerText; // UnityEngine.UI.Text
    private DatabaseManager _databaseManager;

    // 타이머 구현
    public void UpdateTimerUI(float time)
    {
        if (timerText != null)
            timerText.text = "Time: " + Mathf.CeilToInt(time).ToString();
    }

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

        levelScrollView.Initialize(this, _databaseManager.LastUnlockedLevel);
        resultUI.Initialize(this);
    }
}
