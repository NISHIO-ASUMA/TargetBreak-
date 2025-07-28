using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//*************************************
// ゲームタイマー
//*************************************
public class TimeManager : MonoBehaviour
{
    //***********************:
    // 使用メンバ変数
    //***********************:
    public TextMeshProUGUI timerText;     // TextMeshPro 表示用
    private float gameTime = 20f;          // タイマー初期時間（カウントダウンの場合）
    public bool countDown = true;         // カウントダウンかカウントアップか

    private float currentTime;            // 現在のタイマー値
    private bool isRunning = true;        // タイマーの動作フラグ

    private FinishMenuManager finishMenu; // 終了メニュー

    // Start is called before the first frame update
    void Start()
    {
        // タイマーセット
        currentTime = gameTime;

        // テキスト更新
        UpdateTimerText();

        // オブジェクト取得
        finishMenu = FindObjectOfType<FinishMenuManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning) return;

        if (countDown)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                isRunning = false;

                // nullチェック
                if (finishMenu != null)
                {
                    // 終了メニュー表示
                    finishMenu.ShowFinishMenu();
                }
            }
        }
        else
        {
            currentTime += Time.deltaTime;
        }

        UpdateTimerText();
    }

    void UpdateTimerText()
    {
        // 1秒ごとに減らしていく
        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);

        timerText.text = $"Time {minutes:00}:{seconds:00}";
    }

    // タイマー停止
    public void StopTimer()
    {
        isRunning = false;
    }

    // タイマースタート
    public void StartTimer()
    {
        isRunning = true;
    }

    // タイマーリセット
    public void ResetTimer()
    {
        currentTime = gameTime;
        isRunning = true;
    }
}
