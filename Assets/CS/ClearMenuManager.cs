using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

//**********************************
// ステージクリア時の制御スクリプト
//**********************************
public class ClearMenuManager : MonoBehaviour
{
    //*************************
    // 使用メンバ変数
    //*************************
    public GameObject clearMenuPanel;   // パネル
    public Button nextButton;           // 次に進むボタン
    public Button quitButton;           // 辞めるボタン

    private AllBlockManager blockManager; // ブロックマネージャーの変数
    private TimeManager timeManager; // タイマー変数

    private Button[] menuButtons;
    private int currentIndex = 0;
    private float inputCooldown = 0.2f;
    private float inputTimer = 0f;
    private float blockInputTimer = 0f;   // 決定キー入力をブロックするタイマー

    // 色指定
    private Color normalColor = Color.white;
    private Color selectedColor = Color.grey;

    void Start()
    {
        clearMenuPanel.SetActive(false); // パネル非表示
        nextButton.gameObject.SetActive(false); // ボタン非表示
        quitButton.gameObject.SetActive(false); // ボタン非表示

        // マネージャー取得
        blockManager = FindObjectOfType<AllBlockManager>();

        // タイムマネージャー取得
        timeManager = FindObjectOfType<TimeManager>();

        // 値を設定
        nextButton.onClick.AddListener(OnNextStage);
        quitButton.onClick.AddListener(OnQuitGame);

        // 配列生成
        menuButtons = new Button[] { nextButton, quitButton };
    }

    // メニュー出現
    public void ShowClearMenu()
    {
        clearMenuPanel.SetActive(true);         // パネル
        nextButton.gameObject.SetActive(true);  // ボタン表示
        quitButton.gameObject.SetActive(true);  // ボタン表示
        Time.timeScale = 0f; // ポーズ

        currentIndex = 0;

        // 選択カラー更新
        UpdateButtonHighlight();

        blockInputTimer = 1.5f; // メニュー表示後0.2秒間は入力無効
    }

    // 次のステージに進む
    void OnNextStage()
    {
        // 1f待つ
        Time.timeScale = 1f;

        clearMenuPanel.SetActive(false);        // パネル非表示
        nextButton.gameObject.SetActive(false); // ボタン非表示
        quitButton.gameObject.SetActive(false); // ボタン非表示

        // 次のステージロード
        blockManager.LoadNextStage();
    }

    // 終了選択関数
    void OnQuitGame()
    {
        // 1f待つ
        Time.timeScale = 1f;

        clearMenuPanel.SetActive(false);        // パネル非表示
        nextButton.gameObject.SetActive(false); // ボタン非表示
        quitButton.gameObject.SetActive(false); // ボタン非表示

        // StageCheckManagerのカウントを初期化 ( クリア数を消す )
        StageCheckManager.StageCount = 0;

        // SceneControllerを取得
        SceneController sceneController = FindObjectOfType<SceneController>();

        // nullじゃなかったら
        if (sceneController != null)
        {
            // タイトルシーン遷移
            sceneController.scenChange(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 例外
        if (!clearMenuPanel.activeSelf) return;

        // 無効化タイマー進める
        if (blockInputTimer > 0f)
        {
            blockInputTimer -= Time.unscaledDeltaTime;
            return; // この間は入力処理をスキップ
        }

        // 加算
        inputTimer += Time.unscaledDeltaTime;

        float vertical = Input.GetAxisRaw("Vertical"); // パッド選択

        if (Mathf.Abs(vertical) > 0.5f && inputTimer >= inputCooldown)
        {
            // 上下に応じてインデックス変更
            if (vertical < 0)
                currentIndex = (currentIndex + 1) % menuButtons.Length;
            else if (vertical > 0)
                currentIndex = (currentIndex - 1 + menuButtons.Length) % menuButtons.Length;

            // 選択カラー更新
            UpdateButtonHighlight();

            // 初期値設定
            inputTimer = 0f;
        }

        // 決定
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Submit"))
        {
            menuButtons[currentIndex].onClick.Invoke();
        }
    }

    // ハイライト更新処理
    private void UpdateButtonHighlight()
    {
        for (int i = 0; i < menuButtons.Length; i++)
        {
            var colors = menuButtons[i].colors;
            if (i == currentIndex)
            {
                colors.normalColor = selectedColor;
                colors.highlightedColor = selectedColor;
            }
            else
            {
                colors.normalColor = normalColor;
                colors.highlightedColor = normalColor;
            }

            menuButtons[i].colors = colors;
        }
    }
}
