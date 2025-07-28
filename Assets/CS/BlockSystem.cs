using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//***********************************
// ブロックオブジェクト制御
//***********************************
public class BlockSystem : MonoBehaviour
{
    public int BreakBlockLife = 2;      // 壊れるブロックの体力

    void Start()
    {
       // 無し
    }

    void Update()
    { 
        // 無し
    }

    // ダメージ処理関数
    public void ReceiveDamage(int damage)
    {
        // 体力を減らす
        BreakBlockLife -= damage;

        // 0以下の時
        if (BreakBlockLife <= 0)
        {
            Destroy(gameObject); // 自分を破壊
        }
    }
}
