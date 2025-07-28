using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//*************************************
// ターゲットブロック表示
//*************************************
public class TargetBlockManager : MonoBehaviour
{
    //***********************:
    // 使用メンバ変数
    //***********************:
    public TextMeshProUGUI Target;     // TextMeshPro 表示用
    public GameObject BreakBlock;      // 破壊可能ブロックの取得

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {// 個数減ったら表示数を減らす

        // タグでオブジェクト検索して表示
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("BreakBlock");

        // 長さカウント
        int count = blocks.Length;

        // テキスト表示
        Target.text = "Target : " + count.ToString();

    }
}
