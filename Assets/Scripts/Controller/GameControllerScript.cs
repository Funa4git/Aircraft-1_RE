using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System;

// スコアの加算・表示
public class GameControllerScript : MonoBehaviour
{
    public UIControllScript uIControllScript;

    /* プレイ中の文字の表示・非表示用 */
    // スコア関係
    public GameObject scoreText;
    private int score = 0;


    // Scene_objプレハブの取得
    public GameObject[] Scene_obj_Prefab;

    // ステージ番号表示
    public Text stageText;  // ステージ番号のテキスト
    private int currentStageIndex = 0; // ステージ番号（ステージ名ではなく、ボス戦なども含めた連番で0からスタート）
    private int totalStageNum;  // ステージの総数

    // リザルトテキスト
    public GameObject gameClearText;
    public GameObject fixedJoystick;

    // HPバー
    GameObject EnemyHealthBar_Background;
    GameObject EnemyHealthBar_Bar;
    Image EnemyHealth_Image;


    // System Input
    private InputAction nextAction, menuAction;

    private void Awake()
    {
        nextAction = GetComponent<PlayerInput>().currentActionMap["Next"];
        menuAction = GetComponent<PlayerInput>().currentActionMap["Menu"];
    }

    void Start()
    {
        // TalkPanelを非表示
        GameObject.Find("TalkPanel").SetActive(false);

        // 敵HPバーの取得
        EnemyHealthBar_Background = GameObject.Find("EnemyHealthBar_Background");
        EnemyHealthBar_Bar = GameObject.Find("EnemyHealthBar_Bar");
        EnemyHealth_Image = EnemyHealthBar_Bar.GetComponent<Image>();

        // 敵HPバーの非表示
        EnemyHealthBar_Background.SetActive(false);



        totalStageNum = Scene_obj_Prefab.Length;    // ステージの総数を取得

        gameClearText.SetActive(false);
        fixedJoystick.SetActive(false);

#if UNITY_IOS
        //iOSのみで実行したい処理
        if(PlayerPrefs.GetInt("LegacyControl", 0) == 1){
            fixedJoystick.SetActive(true);
        }
#elif UNITY_ANDROID
        //Androidのみで実行したい処理
        if(PlayerPrefs.GetInt("LegacyControl", 0) == 1){
            fixedJoystick.SetActive(true);
        }
#else

#endif

        // PlayerPrefsの呼び出し
        string selectStageName = PlayerPrefs.GetString("Selected_Scene_obj_Prefs", "Stage 1");
        // ステージ名から自動でステージ番号を取得
        for (int i = 0; i < totalStageNum; i++)
        {
            if (Scene_obj_Prefab[i].name == selectStageName)
            {
                currentStageIndex = i;
            }
        }
        // ステージ番号の表示
        UpdateStageText();
        // stageText5秒後非表示
        StartCoroutine("eraseStageText");


        // コルーチンの開始
        StartCoroutine(InputCoroutine());
    }

    // Update is called once per frame
    private IEnumerator InputCoroutine()
    {
        // 初回のインスタンス（Stage1_obj_Prefab）の生成
        Instantiate(Scene_obj_Prefab[currentStageIndex], Vector3.zero, Quaternion.identity);
        InitializeScenePrefabVolume();
        // 敵HPバーの表示判定
        CheckDisplayEnemyHealthBar();

        while (true)
        {
            // gameClear後の処理, gameClearTextがactiveかによって制御
            if (gameClearText.activeSelf == true)
            {
                // Scene_obj_Prefab[currentStageIndex].nameにTalkが含まれているかどうか
                if (Scene_obj_Prefab[currentStageIndex].name.Contains("Talk"))
                {

                } else
                {
                    // クリア後数秒間待つ
                    yield return new WaitForSeconds(4f);
                }

                // 最終ステージならEndrollシーン、それ以外は次のステージ
                if (currentStageIndex < totalStageNum - 1)
                {
                    // 既存のインスタンスの破棄
                    GameObject existingStage = GameObject.FindWithTag("BattleStage");
                    if (existingStage != null)
                    {
                        Destroy(existingStage);
                    }
                    // ステージ番号の更新
                    currentStageIndex++;
                    // 現在のステージ名をPlayerPrefsに保存
                    PlayerPrefs.SetString("Selected_Scene_obj_Prefs", Scene_obj_Prefab[currentStageIndex].name);
                    PlayerPrefs.Save();
                    Debug.Log($"Now Stage Name: {Scene_obj_Prefab[currentStageIndex].name}");

                    // クリア表示の非表示
                    gameClearText.SetActive(false);
                    // ステージ番号の表示
                    UpdateStageText();
                    // stageText4秒後非表示
                    StartCoroutine("eraseStageText");

                    // インスタンスの生成
                    Instantiate(Scene_obj_Prefab[currentStageIndex], new Vector3(0, 0, 0), Quaternion.identity);
                    InitializeScenePrefabVolume();
                    // 敵HPバーの表示判定
                    CheckDisplayEnemyHealthBar();
                }
                else
                {
                    SceneManager.LoadScene("Endroll");
                }
            }

            // 1F待つ
            yield return null;
        }
    }


    // 敵HPゲージを表示するかしないかを判断
    public void CheckDisplayEnemyHealthBar()
    {
        // Scene_obj_Prefab[currentStageIndex].nameにBossが含まれているかどうか
        if (Scene_obj_Prefab[currentStageIndex].name.Contains("Boss") && !Scene_obj_Prefab[currentStageIndex].name.Contains("Talk"))
        {
            // 敵HPバーの表示
            EnemyHealthBar_Background.SetActive(true);
            // 敵HPバーの初期化
            EnemyHealth_Image.fillAmount = 1.0f;
        }
        else
        {
            // 敵HPバーの非表示
            EnemyHealthBar_Background.SetActive(false);
        }
    }


    // Sceneプレハブの音量の初期化（CRI Atom）
    public void InitializeScenePrefabVolume()
    {
        // 音量を取得
        AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", AudioListener.volume);
        var normalize_bgm = PlayerPrefs.GetFloat("BGMVolume", 1);
    }

    // score変数の更新メソッド
    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UpdateScoreText();
    }

    // スコアテキストの更新メソッド
    void UpdateScoreText()
    {
        scoreText.GetComponent<Text>().text = "Score:" + score;
    }

    // ステージテキストの更新メソッド
    void UpdateStageText()
    {
        // Scene_obj_Prefab[currentStageIndex].nameにTalkが含まれているかどうか
        if (Scene_obj_Prefab[currentStageIndex].name.Contains("Talk"))
        {
            stageText.text = "";
        } else
        {
            stageText.text = Scene_obj_Prefab[currentStageIndex].name;
        }
    }

    public void GameOver()
    {
        StartVibrate();

        // 最終スコアの表示、gameOverTextの表示、scoreTextの非表示
        stageText.text = "";
        scoreText.SetActive(false);

        uIControllScript.activeRetryPanel();
    }

    public void GameClear()
    {
        // 敵の弾をすべて削除する
        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        foreach (GameObject DestroyObject in enemyBullets)
        {
            Destroy(DestroyObject);
        }

        // gameClearText,unlockedStageNumの更新 ステージ3のみendRoll表記
        if (currentStageIndex != totalStageNum - 1){
            gameClearText.GetComponent<Text>().text = "STAGE CLEAR !\nScore: " + score + "\n\nWait a few seconds...";

            //if(currentStageIndex >= PlayerPrefs.GetInt("unlockedStageNum", 1))
            //{
            //    PlayerPrefs.SetInt("unlockedStageNum", currentStageIndex + 1);
            //    PlayerPrefs.Save();
            //}

            if (Scene_obj_Prefab[currentStageIndex].name == "Stage 1")
            {
                PlayerPrefs.SetInt("unlockedStageNum", 2);
                PlayerPrefs.Save();
            }
            else if (Scene_obj_Prefab[currentStageIndex].name == "Stage 2 Boss")
            {
                PlayerPrefs.SetInt("unlockedStageNum", 3);
                PlayerPrefs.Save();
            }
            else if (Scene_obj_Prefab[currentStageIndex].name == "Stage 3")
            {
                //PlayerPrefs.SetInt("unlockedStageNum", 4);
                //PlayerPrefs.Save();
            }
            else if (Scene_obj_Prefab[currentStageIndex].name == "Stage 4")
            {
                //PlayerPrefs.SetInt("unlockedStageNum", 5);
                //PlayerPrefs.Save();
            }
            else
            {
                Debug.Log("No Unlock Stage");
            }
        } else {
            gameClearText.GetComponent<Text>().text = "GAME CLEAR !\nScore: " + score + "\n\nWait a few seconds...";
        }
        // gameClearTextの表示, scoreTextの非表示
        gameClearText.SetActive(true);
    }

    public void TalkClear()
    {
        gameClearText.GetComponent<Text>().text = "";
        // gameClearTextの表示, scoreTextの非表示
        gameClearText.SetActive(true);
    }

    // stageTextを5秒で非表示
    IEnumerator eraseStageText()
    {
        yield return new WaitForSeconds(4.0f);

         stageText.text = "";
    }


    // スマートフォンを振動させる（バイブレーション）
    public void StartVibrate()
    {
#if UNITY_IOS || UNITY_ANDROID
        if (SystemInfo.supportsVibration && PlayerPrefs.GetInt("Vibration", 1) == 1)
        {
            Handheld.Vibrate();
            Debug.Log("振動しました．");
        }
        else
        {
            print("振動に非対応の機種です．");
        }
#endif
    }
}
