using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
<summary>
	気軽に呼び出せるダイアログ
</summary>
<remarks>
注意：このダイアログを使用する Scene のヒエラルキーには、Canvas と EventSystem が必要です。<br/>
Canvas と EventSystem を作るには、UI - Button などを作成してから削除すると楽です。<br/>
また、「Dialog」フォルダ内に複数のプレハブが必要です。詳細は <a href="https://qiita.com/hiroaki_okabe/items/0f2d863cf95ba882c8af">Qitta 記事</a>を参照してください。
</remarks>
*/
public class MyDialog {
	private static MyDialog m_Instance = null;

	Transform CanvasTr;				// ヒエラルキー上にある Canvas の Transform

	GameObject DialogPanel;			// ダイアログ外観
	GameObject BackWall;			// 他のUI押下負荷用


	Dictionary<string, GameObject> LoadedPF;	// Loadした全プレハブ部品

	Action ActionOK = null;			// OKボタンが押された時の処理
	Action ActionCancel = null;		// Cancelボタンが押された時の処理

	/// <summary>
	///	シングルトンの為のインスタンス
	/// </summary>
	private static MyDialog Instance {
		get {
			if (m_Instance == null) {
				m_Instance = new MyDialog();
			}
			return m_Instance;
		}
	}

	/// <summary>
	///	コンストラクタ
	/// </summary>
	private MyDialog() {
		CanvasTr = GameObject.Find("Canvas").transform;		// Canvas だけは、ヒエラルキーから拾う
		LoadedPF = new Dictionary<string, GameObject>();	// プレハブ保持用の連想配列
	}

	/// <summary>
	///	メッセージをダイアログで表示して「OK」ボタンで待つ
	/// </summary>
	/// <param name="mess">表示するメッセージ</param>
	/// <param name="funcOk">「OK」ボタンが押された時の処理</param>
	static public void Alert(string mess, Action funcOk = null) {
		MyDialog dialog = Instance;

		if (dialog.CanvasTr == null) {
			dialog.CanvasTr = GameObject.Find("Canvas").transform;
		}
		
		dialog.BackWall = dialog.Instantiate("BackWall");
		dialog.DialogPanel = dialog.Instantiate("DialogPanel");	// ダイアログ外観

		dialog.AddObj("DialogText").GetComponent<Text>().text = mess;	// メッセージ

		dialog.ActionOK = funcOk;
		dialog.AddObj("OkPanel").transform.Find("OkButton").GetComponent<ButtonController>().onClick.AddListener(dialog.ClickOkButton);
	}

	/// <summary>
	///	OK / Cancel 確認ダイアログを表示する
	/// </summary>
	/// <param name="mess">表示するメッセージ</param>
	/// <param name="funcOk">「OK」ボタンが押された時の処理</param>
	/// <param name="funcCancel">「Cancel」ボタンが押された時の処理</param>
	static public void Confirm(string mess, Action funcOk = null, Action funcCancel = null, bool revPos = false) {
		MyDialog dialog = Instance;

		if (dialog.CanvasTr == null) {
			dialog.CanvasTr = GameObject.Find("Canvas").transform;
		}
		
		dialog.BackWall = dialog.Instantiate("BackWall");
		dialog.DialogPanel = dialog.Instantiate("DialogPanel");

		dialog.AddObj("DialogText").GetComponent<Text>().text = mess;

		dialog.ActionOK = funcOk;
		dialog.ActionCancel = funcCancel;
		GameObject buttonPanel = dialog.AddObj("OkCancelPanel");
		buttonPanel.transform.Find("OkButton").GetComponent<ButtonController>().onClick.AddListener(dialog.ClickOkButton);
		buttonPanel.transform.Find("CancelButton").GetComponent<ButtonController>().onClick.AddListener(dialog.ClickCancelButton);

		if(revPos)
		{
			Vector3 posok = buttonPanel.transform.Find("OkButton").transform.position;
			Vector3 poscan = buttonPanel.transform.Find("CancelButton").transform.position;
			(posok.x, poscan.x) = (poscan.x, posok.x);
			buttonPanel.transform.Find("OkButton").transform.position = posok;
			buttonPanel.transform.Find("CancelButton").transform.position = poscan;
		}
	}

	/// <summary>
	///	OKボタンが押された時の処理
	/// </summary>
	private void ClickOkButton() {
		Close();		// ダイアログを消す
		if (ActionOK != null) {	// コールバック先が登録されていれば
			ActionOK();				// 実行
		}
	}

	/// <summary>
	///	Cancelボタンが押された時の処理
	/// </summary>
	private void ClickCancelButton() {
		Close();		// ダイアログを消す
		if (ActionCancel != null) {
			ActionCancel();
		}
	}

	/// <summary>
	/// 非MonoBehaviour 用の、プレハブ Instantiate(実体化)
	/// </summary>
	/// <param name="prefabName">Instantiate したいプレハブ名</param>
	/// <returns>生成された GameObject</param>
	private GameObject Instantiate(string prefabName) {
		if (!LoadedPF.ContainsKey(prefabName)) {	// まだプレハブが Load されていなければ
			LoadedPF.Add(prefabName, (GameObject)Resources.Load("Dialog/" + prefabName));	// Load
		}
		GameObject obj = UnityEngine.Object.Instantiate(LoadedPF[prefabName]);
		obj.transform.SetParent(CanvasTr, false);
		obj.transform.localScale = Vector3.one;
		obj.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
		// anchoredPosition3D をセットしないと、Pos.Z が不定になる。
		return obj;
	}

	/// <summary>
	/// DialogPanel に GameObject 追加
	/// </summary>
	/// <param name="add">追加する部品名</param>
	/// <returns>追加された GameObject</param>
	private GameObject AddObj(string add) {
		GameObject obj = Instantiate(add);
		obj.transform.SetParent(DialogPanel.transform, false);
		return obj;
	}

	/// <summary>
	///	ダイアログを閉じる
	/// </summary>
	private void Close() {
		foreach (Transform child in DialogPanel.transform) {	// 追加した GameObject を削除
			UnityEngine.Object.Destroy(child.gameObject);
		}
		
		UnityEngine.Object.Destroy(BackWall);
		UnityEngine.Object.Destroy(DialogPanel);	// ダイアログパネルを消す
	}
}
