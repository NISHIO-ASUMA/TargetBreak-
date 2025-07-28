using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

//**********************************
//ゲーム終了時メニューの制御スクリプト
//**********************************
public class FinishMenuManager : MonoBehaviour
{
    //*************************
    // 使用メンバ変数
    //*************************
    public GameObject FinishMenuPanel; // パネルオブジェクト
    public Button RetryButton;         // ボタンオブジェクト
    public Button QuitButton;          // ボタンオブジェクト
    public GameObject player;

    private TimeManager timeManager;    // タイマースクリプト変数
    private AllBlockManager blockManager; // ブロック管理スクリプト変数

    private Button[] menuButtons;       // ボタンの配列
    private int currentIndex = 0;       // 選択メニュー番号
    private float inputCooldown = 0.2f; // 入力受付までの時間
    private float inputTimer = 0f;      // キー入力時間
    private bool isKeyDown = false;     // キーの押下状態
    private Vector3 playerStartPosition; // プレイヤーの初期位置

    private SceneController SManager;

    void Start()
    {
        // パネル、ボタンを非アクティブ化
        FinishMenuPanel.SetActive(false);
        RetryButton.gameObject.SetActive(false);
        QuitButton.gameObject.SetActive(false);

        // スクリプト取得
        timeManager = FindObjectOfType<TimeManager>();
        blockManager = FindObjectOfType<AllBlockManager>();
        SManager = FindObjectOfType<SceneController>();

        // ボタンイベント登録
        RetryButton.onClick.AddListener(OnRetry);
        QuitButton.onClick.AddListener(OnQuit);

        // ボタン配列
        menuButtons = new Button[] { RetryButton, QuitButton };

        // 初期座標をセット
        playerStartPosition = player.transform.position;
    }

    void Update()
    {
        // 未使用じゃなかったら
        if (!FinishMenuPanel.activeSelf) return;

        // 入力時間を加算
        inputTimer += Time.unscaledDeltaTime;

        // Enterキーが離されたかどうかで検出
        if (!isKeyDown && !Input.GetKey(KeyCode.Return))
        {
            // 入力可能状態になる
            isKeyDown = true;
        }

        // ゲームパッドの縦方向入力を有効化
        float vertical = Input.GetAxisRaw("Vertical");

        // 入力可能時間がクールダウンタイムより大きくなったら
        if (Mathf.Abs(vertical) > 0.5f && inputTimer >= inputCooldown)
        {
            // メニューを選択
            if (vertical < 0)
                currentIndex = (currentIndex + 1) % menuButtons.Length;
            else
                currentIndex = (currentIndex - 1 + menuButtons.Length) % menuButtons.Length;

            // 初期は一番上に設定
            EventSystem.current.SetSelectedGameObject(menuButtons[currentIndex].gameObject);

            // 入力受付を初期化
            inputTimer = 0f;
        }

        // 一度だけ入力を受け付ける
        if (Input.GetKeyDown(KeyCode.Return) && isKeyDown)
        {
            // メニューの処理を実行
            menuButtons[currentIndex].onClick.Invoke();
        }
    }

    //==============================
    // メニュー表示関数
    //==============================
    public void ShowFinishMenu()
    {
        // 時間をリセット
        Time.timeScale = 0f;

        // キーフラグを未使用にする
        isKeyDown = false;

        // パネル、ボタンをアクティブ化
        FinishMenuPanel.SetActive(true);
        RetryButton.gameObject.SetActive(true);
        QuitButton.gameObject.SetActive(true);

        // インデックス,入力受付時間の初期化
        currentIndex = 0;
        inputTimer = 0f;
    }

    //==============================
    // リトライ関数
    //==============================
    void OnRetry()
    {
        // 遷移時間をセット
        Time.timeScale = 1f;

        // リトライ用の処理
        blockManager.RetryStage();
        timeManager.ResetTimer();

        // パネル、ボタンを非アクティブ化
        FinishMenuPanel.SetActive(false);
        RetryButton.gameObject.SetActive(false);
        QuitButton.gameObject.SetActive(false);

        // プレイヤーを初期位置に戻す
        if (player != null)
        {
            // 初期座標を代入
            player.transform.position = playerStartPosition;
        }
    }

    //==============================
    // ゲーム終了関数
    //==============================
    void OnQuit()
    {
        // 遷移時間をセット
        Time.timeScale = 1f;

        // パネル、ボタンを非アクティブ化
        FinishMenuPanel.SetActive(false);
        RetryButton.gameObject.SetActive(false);
        QuitButton.gameObject.SetActive(false);

        // SceneControllerを取得
        SceneController sceneController = FindObjectOfType<SceneController>();

        // nullじゃなかったら
        if (sceneController != null)
        {
            // リザルトに遷移
            sceneController.scenChange(3);
        }
    }
}
