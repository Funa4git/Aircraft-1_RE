using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class EndrollScript : MonoBehaviour
{
    // Endrollシーンの制御
    // テキストのスクロールスピード
    float textScrollSpeed;
    // テキストの制限位置
    float limitPosition;
    // エンドロールが終了したかどうか
    private bool isStopEndRoll;
    // シーン移動用コルーチン
    private Coroutine endRollCoroutine;

    // スタート画面への遷移をうながす表示
    public GameObject goBackStartText;

    private InputAction nextAction, menuAction;

    [SerializeField] private GameObject menuButton;
    [SerializeField] private AudioSource AudioSource;


    private float BottomSize;

    private void Awake()
    {
        nextAction = GetComponent<PlayerInput>().currentActionMap["Next"];
        menuAction = GetComponent<PlayerInput>().currentActionMap["Menu"];
    }

    void Start()
    {
        // エンドロールの長さを取得
        BottomSize = this.GetComponent<RectTransform>().sizeDelta.y;
        Debug.Log("エンドロールの縦の長さ：" + BottomSize);
        // 制限位置の設定
        limitPosition = BottomSize;
        Debug.Log("制限位置：" + limitPosition);
        // スクロールスピードの決定
        textScrollSpeed = BottomSize / 150f;
        Debug.Log("スクロールスピード：" + textScrollSpeed);

        // goBackStartTextの非表示
        goBackStartText.SetActive(false);

        // OnClickApplyButtonイベントを設定しConfig画面を表示
        menuButton.GetComponent<ButtonController>().onClick.AddListener(OnClickMenuButton);

        StartCoroutine(InputCoroutine());
    }

    // Update is called once per frame
    private IEnumerator InputCoroutine()
    {
        while (true)
        {
            // endRoll遷移
            if (isStopEndRoll)
            {
                endRollCoroutine = StartCoroutine(GoToNextScene());
            }
            else
            {
                if (this.GetComponent<RectTransform>().offsetMax.y <= limitPosition)
                {
                    transform.position += new Vector3(0, textScrollSpeed * 0.1f * Time.deltaTime, 0);
                }
                else
                {
                    isStopEndRoll = true;
                    Debug.Log("エンドロール終わり！");
                }
            }

            // 1F待つ
            yield return null;
        }
    }

    void titleAction(){
        Time.timeScale = 1;
        SceneManager.LoadScene("idleScene");
    }
    void cancelAction(){
        Time.timeScale = 1;
        AudioSource.Play();
    }

    public void OnClickMenuButton()
    {
        // スタート待機画面へ
        Time.timeScale = 0;
        AudioSource.Pause();
        MyDialog.Confirm("タイトルへ移動しますか ?", titleAction, cancelAction);
        
    }

    IEnumerator GoToNextScene()
    {
        // endRoll遷移後数秒間待つ
        yield return new WaitForSeconds(2f);
        // goBackStartTextの表示
        goBackStartText.SetActive(true);

        // 数秒間待つ
        yield return new WaitForSeconds(4f);
        // スタート待機画面へ
        StopCoroutine(endRollCoroutine);
        SceneManager.LoadScene("idleScene");
    }
}
