using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	MyDialog の呼び出し例
	注意：ヒエラルキー上に、Canvas と EventSystem が必要です。
	Canvas と EventSystem を作るには、UI - Button などを作成してから削除すると楽です。
*/
public class DialogSample : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		MyDialog.Alert("「OK」を押すと、Confirm ダイアログを表示。", Click_OK);
	}
	
	// Update is called once per frame
	void Update()
	{
	}
	
	void Click_OK() {
		MyDialog.Confirm("次は、どちらのボタンを押しても\nDebug.Log だけ出して終わります。", Click_OK2, Click_Cancel);
	}
	
	void Click_OK2() {
		Debug.Log("OK が押されました");
	}
	
	void Click_Cancel() {
		Debug.Log("Cancel が押されました");
	}
	
}
