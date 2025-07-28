using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;          // 追加
using UnityEngine.SceneManagement;     // 追加

//******************************************
// シーン制御スクリプト
//******************************************
public class SceneController : MonoBehaviour
{
	// インスペクターからプレハブ化したCanvasを入れる用の変数
	public GameObject fade;

	// 操作するCanvasを設定する
	GameObject FadeCanvas;

	// Start is called before the first frame update
	void Start()
	{
		// FadeManagerからisFadeInstanceを参照する
		if (!FadeManager.isFadeInstance)
		{// isFadeInstanceがfalseだったら
			Instantiate(fade);
		}

		// 起動時用にCanvasの生成を少し待つ (今回の場合は0.02秒後に呼び出す)
		Invoke("findFadeObject", 0.02f);
	}

	// Update is called once per frame
	void Update()
	{
		// 現在のシーン名を取得
		string currentSceneName = SceneManager.GetActiveScene().name;

		// 決定キー かつ ゲームシーンではない
		if (Input.GetKey(KeyCode.Return) && currentSceneName != "GameScene")
		{
			// 現在のシーン番号に1を足した数字を保持する
			int nScnenIdx = SceneManager.GetActiveScene().buildIndex + 1; // 現在のアクティブなシーンを取得,加算

			// 最大シーン番号と比較,超えそうになったら最初のシーンに戻す
			if (nScnenIdx >= SceneManager.sceneCountInBuildSettings)
			{
				nScnenIdx = 0;
			}

			// シーン切り替え
			scenChange(nScnenIdx);
		}

		// Escキーでゲーム終了
		if (Input.GetKey(KeyCode.Escape))
		{
#if UNITY_EDITOR
			// ゲームプレイ終了
			UnityEditor.EditorApplication.isPlaying = false;
#else
			// ゲームプレイ終了
			Application.Quit();
#endif
		}
	}

	// オブジェクト検知用関数
	void findFadeObject()
	{
		// Canvasを見つける (タグを探す)
		FadeCanvas = GameObject.FindGameObjectWithTag("Fade");

		// フラグを立て,フェードイン関数を呼び出す
		FadeCanvas.GetComponent<FadeManager>().FadeIn();
	}

	// シーン切り替え用の関数
	public async void scenChange(int sceneIndex)
	{
		// フラグを立てて,フェードアウト関数を呼び出す
		FadeCanvas.GetComponent<FadeManager>().FadeOut();

		// 暗転するまでここで処理を止める
		await Task.Delay(200);

		// 暗転した後にシーンを切り替える
		SceneManager.LoadScene(sceneIndex);
	}
}
