using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//***************************************
// プレイヤー制御スクリプト
//***************************************
public class PlayerContoroller : MonoBehaviour
{
    //***************************
    // 使用メンバ変数
    //***************************
    private float playermove = 5.0f;
    public float jumpPower = 5f;
    public float gravity = 9.8f;
    public float AttackRange = 3.0f;
    private float verticalSpeed = 0f;
    private bool isGround = true;

    void Start()
    {
        // ナニカを取得する
    }

    void Update()
    {
        Debug.Log($"Y={transform.position.y}, verticalSpeed={verticalSpeed}, isGround={isGround}");

        // キー入力で移動更新
        if (Input.GetKey(KeyCode.A))
        {// Aキー
            transform.position -= transform.right * playermove * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {// Dキー
            transform.position += transform.right * playermove * Time.deltaTime;
        }

        // ゲームパッドで移動
        float horizontal = Input.GetAxis("Horizontal");

        if (Mathf.Abs(horizontal) > 0.1f)
        {
            transform.position += transform.right * horizontal * playermove * Time.deltaTime;
        }

        // ジャンプ処理
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump") )&& isGround)
        {
            verticalSpeed = jumpPower;

            isGround = false;
        }

        // 攻撃処理
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Fire2"))
        {
            // ブロック攻撃
            AttackNearbyBlock();
        }

        // 重力適用時
        if (!isGround)
        {
            verticalSpeed -= gravity * Time.deltaTime;
        }

        // Y軸移動適用
        transform.position += Vector3.up * verticalSpeed * Time.deltaTime;
    }

    // 接地判定
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("BreakBlock"))
        {
            isGround = true;
            verticalSpeed = 0f;
        }
    }

    // 接地判定
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("BreakBlock"))
        {
            isGround = true;
            verticalSpeed = 0f;
        }
    }

    // 接地判定
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("BreakBlock"))
        {
            isGround = false;
        }
    }

    // 距離参照ブロック攻撃
    void AttackNearbyBlock()
    {
#if true
        Breaksystem[] allBlocks = FindObjectsOfType<Breaksystem>();

        foreach (Breaksystem block in allBlocks)
        {
            // 距離計算
            float distance = Vector3.Distance(transform.position, block.transform.position);

            // 範囲内に入ったら
            if (distance < AttackRange)
            {
                if (block.CompareTag("BreakBlock")) // 壊せるブロックをタグで判別
                {
                    block.ReceiveDamage(1); // 1ダメージ与える
                    break; // 1つだけ攻撃して終了
                }
            }
        }
#endif
    }
}
