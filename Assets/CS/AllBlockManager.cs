using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//**************************************
// ブロック全体を管理するスクリプト
//**************************************
public class AllBlockManager : MonoBehaviour
{
    //*************************
    // 使用メンバ変数
    //*************************
    [System.Serializable]
    
    // ステージにセットする情報
    public class StageBlockSet
    {
        public string stageName;
        public List<GameObject> blockPrefabs;
        public List<Vector3> spawnPositions;
    }

    [Header("ステージリスト（インデックス順）")]
    public List<StageBlockSet> stages = new List<StageBlockSet>();
    private List<GameObject> spawnedBlocks = new List<GameObject>();
    private int currentStageIndex = 0;      // ステージのインデックス番号

    private int remainingBlocks = 0; // 残りブロック数
    private ClearMenuManager clearMenuManager;　 // メニュー表示
    private FinishMenuManager finishMenuManager;

    void Start()
    {
        clearMenuManager = FindObjectOfType<ClearMenuManager>(); //マネージャー取得
        finishMenuManager = FindObjectOfType<FinishMenuManager>(); // 取得

        LoadStage(currentStageIndex); // 初期ステージを読み込み
    }

    /// <summary>
    /// インデックスでステージ読み込み
    /// </summary>
    public void LoadStage(int index)
    {
        // ステージ状態をクリア
        ClearStage();

        if (index < 0 || index >= stages.Count)
        {
            Debug.LogWarning("ステージインデックスが範囲外です。");
            return;
        }

        StageBlockSet selectedStage = stages[index];

        for (int i = 0; i < selectedStage.blockPrefabs.Count; i++)
        {
            Vector3 pos = selectedStage.spawnPositions[i];
            GameObject block = Instantiate(selectedStage.blockPrefabs[i], pos, Quaternion.identity);
            spawnedBlocks.Add(block);
        }

        // BreakBlockタグ付きだけをカウント
        remainingBlocks = GameObject.FindGameObjectsWithTag("BreakBlock").Length;

        Debug.Log($"ステージ[{index}]：{selectedStage.stageName} を読み込みました。残りブロック: {remainingBlocks}");

    }

    // 破壊通知を受け取る関数
    public void OnBlockDestroyed(GameObject block)
    {
        if (block.CompareTag("BreakBlock"))
        {
            // 数減らす
            remainingBlocks--;

            Debug.Log("破壊された BreakBlock：残り " + remainingBlocks);

            if (remainingBlocks <= 0)
            {
                Debug.Log("ステージクリア！");

                // 最後のステージかどうかチェック
                if (currentStageIndex +1  >= stages.Count)
                {
                    Debug.Log("最後のステージをクリアしました。FinishMenuを表示します。");

                    // 取得できていたら
                    if (finishMenuManager != null)
                    {
                        // 終了メニュー表示
                        finishMenuManager.ShowFinishMenu();
                    }
                }
                else
                {
                    // 通常メニュー表示
                    clearMenuManager.ShowClearMenu();
                }
            }
        }
    }

    /// <summary>
    /// 次のステージに進む
    /// </summary>
    public void LoadNextStage()
    {
        currentStageIndex++;

        if (currentStageIndex >= stages.Count)
        {
            Debug.Log("全ステージをクリアしました！");

            if (finishMenuManager != null)
            {
                finishMenuManager.ShowFinishMenu(); // 終了メニューを表示
            }

            return;
        }

        LoadStage(currentStageIndex);
    }

    /// <summary>
    /// さいしょのステージを読み込み
    /// </summary>
    public void RetryStage()
    {
        // インデックス番号を初期化
        currentStageIndex = 0;

        // 初期ステージを読み込む
        LoadStage(0);
    }

    /// <summary>
    /// ステージのブロックをすべて削除
    /// </summary>
    private void ClearStage()
    {
        foreach (var block in spawnedBlocks)
        {
            if (block != null)
                Destroy(block);
        }
        spawnedBlocks.Clear();
    }

    /// <summary>
    /// 現在のステージ番号を取得
    /// </summary>
    public int GetCurrentStageIndex()
    {
        return currentStageIndex;
    }
}
