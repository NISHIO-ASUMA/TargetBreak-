using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//*****************************************
// 破壊可能ブロックに対する制御スクリプト
//*****************************************
public class Breaksystem : MonoBehaviour
{
    //****************************************
    // 使用メンバ変数
    //****************************************
    public int BlockLife = 0;   // 体力
    public GameObject BreakEffectPrefab;    // エフェクトプレハブ
    private AllBlockManager blockManager; // 追加

    // Start is called before the first frame update
    void Start()
    {
        // 取得
        blockManager = FindObjectOfType<AllBlockManager>(); // ステージマネージャ取得
    }

    // ダメージ処理
    public void ReceiveDamage(int damage)
    {
        // 体力を減らす
        BlockLife -= damage;

        // 0以下
        if (BlockLife <= 0)
        {
            // 破壊前に報告（引数に自分を渡す）
            if (blockManager != null)
            {
                blockManager.OnBlockDestroyed(gameObject);
            }

            // 破棄
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(FlashRed());
        }
    }

    // 赤点滅
    private IEnumerator FlashRed()
    {
        if (BreakEffectPrefab != null)
        {
            Vector3 effectPos = transform.position + Vector3.up * 0.5f;
            GameObject effect = Instantiate(BreakEffectPrefab, effectPos, Quaternion.identity);
            Destroy(effect, 0.5f); // エフェクト終了

            yield return new WaitForSeconds(0.1f); // 少し時間を置いて処理終了
        }
        else
        {
            yield return null;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
