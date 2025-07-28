using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charactor : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform cameraTransform;

    void Start()
    {
        if (!cameraTransform)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    void Update()
    {
        // 入力取得
        float h = Input.GetAxis("Horizontal"); // A/D or 左スティック左右
        float v = Input.GetAxis("Vertical");   // W/S or 左スティック上下

        Vector3 inputDir = new Vector3(h, 0, v);
        if (inputDir.magnitude > 0.1f)
        {
            // カメラの向きに合わせて移動方向を調整
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;

            camForward.Normalize();
            camRight.Normalize();

            Vector3 move = camForward * v + camRight * h;

            // 実際に移動
            transform.position += move * moveSpeed * Time.deltaTime;

            // 向きを移動方向に合わせる
            transform.rotation = Quaternion.LookRotation(move);
        }
    }
}
