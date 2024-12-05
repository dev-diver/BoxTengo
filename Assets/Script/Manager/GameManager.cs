using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static event Action<float> scoreChange;
    public static event Action<float> bestScoreChange;
    public static event Action<float> timeChange;
    public static event Action timeBonus;

    public static event Action BeforeStart; //처음 들어감.
    public static event Action GameSet; //새 게임 세팅
    public static event Action PreCount; //카운트 다운
    public static event Action GameStart; //게임 시작
    public static event Action GamePause; //일시 정지
    public static event Action GameOver; //완전 끝남

    [Header("Button")]

    [SerializeField] Button restartButton;

    [Header("Board")]
    [SerializeField] GameObject bestScoreMessage;
    [SerializeField] preCountDowner preCountDownScreen;

    [Header("Popup")]
    [SerializeField] GameObject PopupAskContinue;

    float _score;
    float score
    {

        get { return _score; }
        set
        {
            _score = value;
            scoreChange(_score);
        }
    }

    float _bestScore;
    float bestScore
    {
        get
        {
            _bestScore = PlayerPrefs.GetFloat("BestScore", 0.0f);
            return _bestScore;
        }
        set
        {
            _bestScore = value;
            PlayerPrefs.SetFloat("BestScore", _bestScore);
            bestScoreChange(_score);
        }
    }
    float _lastTime;
    float lastTime
    {
        get
        {
            return _lastTime;
        }
        set
        {
            _lastTime = value;
            timeChange(_lastTime);
        }
    }

    int resur = 0;
    int stage = 0;

    private Coroutine countdown;
    private Coroutine comboCount;
    private Coroutine gameStartCo;
    private Coroutine gameoverCo;

    [Header("Objects")]
    public DrawSelection drawSelection;
    public AppleManager appleManager;
    public ComboManager comboManager;
    public PointManager pointManager;
    public ClearManager clearManager;

    public bool bonusMode = false;

    [Tooltip("Choose setting")]
    [SerializeField] GameConfig GameSettings;
    void Start()
    {
        if (DebugManager.Instance.debugMode == true)
        {
            bonusMode = true;
            GameSettings.totalTime = 5.0f;
        }
    }
    public void CalScore(HashSet<Apple> selectedApples)
    {
        float minX = 7, minY = 7, maxX = 0, maxY = 0;

        float addTime = comboManager.comboAddTime();
        Debug.Log("Combo add Time: " + addTime);
        addLastTime(addTime);
        if (comboManager.Combo >= 1)
        {
            comboManager.ShowComboText(comboManager.Combo);
            Debug.Log(comboManager.Combo + "Combo!!");
            if (comboCount != null)
            {
                StopCoroutine(comboCount);
            }
        }

        foreach (var apple in selectedApples)
        {
            if (apple.coord.x <= minX)
            {
                minX = apple.coord.x;
            }
            if (apple.coord.y <= minY)
            {
                minY = apple.coord.y;
            }
            if (apple.coord.x >= maxX)
            {
                maxX = apple.coord.x;
            }
            if (apple.coord.y >= maxY)
            {
                maxY = apple.coord.y;
            }
        }

        //20 이 10보다 더 많음
        float distance = (maxX - minX + 1) * (maxY - minY + 1);
        float point = (comboManager.Combo + 2) * distance * selectedApples.Count;

        if (selectedApples.Count > 0)
        {
            var firstApple = selectedApples.FirstOrDefault();
            if (firstApple != null)
            {
                addPointToScore(point, firstApple.transform.position);
            }
        }

        comboManager.Combo += 1;
        comboCount = StartCoroutine(comboManager.ComboCountDownRoutine());
    }

    public void RecordScore()
    {
        if (GPGSManager.Instance.isLogin())
        {
            GPGSRankingManager.Instance.AddLeaderboard(bestScore);
        }
    }

    public void ShowRanking()
    {
        RecordScore();
        GPGSRankingManager.Instance.ShowLeaderboardUI_Ranking();
    }

    void addPointToScore(float point, Vector3 position)
    {
        pointManager.ShowPointText(point, position);
        score += point;
        //Score Text Animation 추가
    }
    void addLastTime(float time)
    {
        lastTime += time;
        timeBonus?.Invoke();
    }
    IEnumerator preCountDown(float time)
    {

        float CountDownTime = 3;
        PreCount?.Invoke();
        preCountDownScreen.turnOn();
        while (CountDownTime > 0)
        {
            preCountDownScreen.setTime(CountDownTime);
            SFXManager.Instance.PlaySound("PRECOUNT");
            yield return new WaitForSeconds(0.5f);
            CountDownTime -= 1;
        }
        preCountDownScreen.turnOff();
        BGMManager.Instance.BGMChange("GAME");
        SFXManager.Instance.PlaySound("GAMESTART");
        countdown = StartCoroutine(InGameCountDownRoutine(GameSettings.totalTime));
        yield break;
    }
    IEnumerator InGameCountDownRoutine(float time)
    {
        GameStart?.Invoke();
        lastTime = time;
        while (lastTime > 0)
        {
            lastTime -= Time.deltaTime;
            yield return null;
        }
        lastTime = 0;
        continueOrEnd();
        yield break;
    }
    IEnumerator GameOverAdStandby()
    {
        restartButton.interactable = false;
        yield return new WaitForSeconds(0.3f);
        PopupAskContinue.SetActive(true);
        restartButton.interactable = true;
        AdManager.Instance.showEndGameAd();
    }

    public void coinGameStart()
    {
        AdManager.Instance.GameCount = 0;
        PlayerPrefs.SetInt("GameCount", AdManager.Instance.GameCount);
        RewardManager.Instance.addCoin(-1);
        newGame();
    }

    void stopPreCountDown()
    {
        if (gameStartCo != null)
        {
            StopCoroutine(gameStartCo);
        }
        preCountDownScreen.turnOff();
    }

    void stopInGameCountDown()
    {
        if (countdown != null)
        {
            StopCoroutine(countdown);
        }
    }
    public void newGame()
    {
        GamePause?.Invoke();
        resetGame();
        newBlocks();
        gameStartCo = StartCoroutine(preCountDown(GameSettings.totalTime));
    }
    public void resetGame()
    {
        stopInGameCountDown();
        stopPreCountDown();
        comboManager.Combo = 0;
        lastTime = 0;
        resur = 0;
        stage = 0;
        score = 0;
        GameSet?.Invoke();
    }
    public void newBlocks()
    {
        //appleReset
        appleManager.ClearApples();
        appleManager.CreateApples();
        appleManager.ShuffleApples();
    }
    public void nextStage()
    {
        stage++;
        addPointToScore(stage * GameSettings.allClearPoint, Vector3.zero);
        float addTime = GameSettings.secondTime - stage * GameSettings.subRetryTime;
        addLastTime(addTime >= GameSettings.minRetryTime ? addTime : GameSettings.minRetryTime);
        newBlocks();
        clearManager.showClearText();
    }
    public void ContinueGame()
    {
        SFXManager.Instance.PlaySound("GAMECONTINUE");
        resur += 1;
        RewardManager.Instance.addCoin(-1);
        gameStartCo = StartCoroutine(preCountDown(GameSettings.retryTime));
    }

    void continueOrEnd()
    {
        if (score > bestScore)
        {
            GamePause?.Invoke();
            newRecord();
        }
        else
        {
            GameOver?.Invoke();
            if (resur < 1)
            {
                gameoverCo = StartCoroutine(GameOverAdStandby());
            }
        }
    }
    void newRecord()
    {
        SFXManager.Instance.PlaySound("HIGHSCORE");
        bestScore = score;
        bestScoreMessage.SetActive(true);
        RewardManager.Instance.addCoin(1);
        RecordScore();
    }

}
