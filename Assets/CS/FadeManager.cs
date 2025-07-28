using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // UI使うので追加

public class FadeManager : MonoBehaviour
{// フェードシステムスクリプト

    // フェードの管理フラグ変数
    public static bool isFadeInstance = false;

    bool isFadeIn = false;      // フェードインするフラグ
    bool isFadeOut = false;     // フェードアウトするフラグ

    public float alpha = 0.0f;      // フェードの透過率
    public float fadeSpeed = 0.2f;  // フェードのスピード

    // Start is called before the first frame update
    void Start()
    {
        // 起動時
        if (!isFadeInstance)
        {// falseなら

            DontDestroyOnLoad(this);
            isFadeInstance = true;      // フラグを有効化する
        }
        else
        {// 起動時以外は重複しないようにする
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadeIn)
        {// フェードが有効

            // 透過率を計算する
            alpha -= Time.deltaTime / fadeSpeed;

            if (alpha <= 0.0f)
            {// 0.0fより下回ったら
                // フェードインフラグをfalseにする
                isFadeIn = false;

                // 透明度を0.0fに固定
                alpha = 0.0f;
            }

            // カラーを調整
            this.GetComponentInChildren<Image>().color = new Color(0.0f, 0.0f, 0.0f, alpha);
        }
        else if (isFadeOut)
        {// フェードアウトが有効

            // 透過率を計算する
            alpha += Time.deltaTime / fadeSpeed;

            if (alpha >= 1.0f)
            {//1.0fより上回ったら
                // フェードインフラグをfalseにする
                isFadeOut = false;

                // 透明度を1.0fに固定
                alpha = 1.0f;
            }

            // カラーを調整
            this.GetComponentInChildren<Image>().color = new Color(0.0f, 0.0f, 0.0f, alpha);
        }
    }

    // フェードイン用の関数
    public void FadeIn()
    {
        isFadeIn = true;    // Inをtrue
        isFadeOut = false;  // Outをfalse
    }

    // フェードアウト用の関数
    public void FadeOut()
    {
        isFadeIn = false;   // Inをfalse
        isFadeOut = true;   // Outをtrue
    }

}
