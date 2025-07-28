using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ppsystem : MonoBehaviour
{
    public Transform target;          // カメラが追いかける対象
    public float distance = 5.0f;     // プレイヤーとの距離
    public float xSpeed = 120.0f;     // 水平回転速度
    public float ySpeed = 80.0f;      // 垂直回転速度

    public float yMinLimit = -20f;    // 上下回転の下限
    public float yMaxLimit = 80f;     // 上下回転の上限

    private float x = 0.0f;           // 現在のX軸角度（水平）
    private float y = 0.0f;           // 現在のY軸角度（垂直）

    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // カーソルを非表示にする（必要に応じて）
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (target)
        {
            // マウス移動取得
            x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
            y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;

            // 垂直角度制限
            y = Mathf.Clamp(y, yMinLimit, yMaxLimit);

            // 回転情報からカメラの位置を計算
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            Vector3 position = rotation * new Vector3(0.0f, 0.0f, -distance) + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }
}
