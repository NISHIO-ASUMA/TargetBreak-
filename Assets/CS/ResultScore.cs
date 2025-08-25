using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//***************************************
// リザルトに出すクリアステージ数描画
//***************************************
public class ResultScore : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI stageText;

    // 開始関数
    void Start()
    {
        stageText.text = "ClaerStage : " + StageCheckManager.StageCount;
    }

    // Update is called once per frame
    void Update()
    {
     
    }
}
