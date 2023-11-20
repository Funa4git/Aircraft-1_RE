using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class UIControllScript : MonoBehaviour
{
    public GameControllerScript gameControllerScript;
    // UI制御スクリプト
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject replayButton;
    [SerializeField] private GameObject configButton;
    [SerializeField] private GameObject titleButton;


    [SerializeField] private GameObject configPanel;
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider bgmVolume;
    [SerializeField] private Slider seVolume;
    [SerializeField] private Toggle isOnLegacyControl;
    [SerializeField] private Toggle isOnVibration;
    [SerializeField] private Slider speedVolume;
    [SerializeField] private GameObject applyButton;
    [SerializeField] private GameObject defaultButton;

    [SerializeField] AudioMixer audioMixer;

    [SerializeField] private Sprite pauseButtonSprite;
    [SerializeField] private Sprite resumeButtonSprite;

    [SerializeField] private GameObject fixedJoystick;

    // [SerializeField] private GameObject retryPanel;
    // [SerializeField] private GameObject retryYesButton;
    // [SerializeField] private GameObject retryNoButton;


    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        // retryPanel.SetActive(false);
        // PauseButton
        pauseButton.GetComponent<ButtonController>().onClick.AddListener(Pause);

        // PauseMenu内各種ボタン
        replayButton.GetComponent<ButtonController>().onClick.AddListener(Replay);
        titleButton.GetComponent<ButtonController>().onClick.AddListener(LoadTitle);
        // 設定画面開く
        configButton.GetComponent<ButtonController>().onClick.AddListener(activeConfig);

        // isOnVibrationの入力拒否
        if (SystemInfo.supportsVibration)
        {

        }
        else
        {
            isOnVibration.interactable = false;
        }

        // isOnLegacyControlの入力拒否
#if UNITY_IOS
        // iOSのみで実行したい処理
#elif UNITY_ANDROID
        // Androidのみで実行したい処理
#else
        isOnLegacyControl.interactable = false;
#endif

        InitializeConfigSettings();
    }

    // Config画面の初期化
    void InitializeConfigSettings()
    {
        // 最小値、最高値、現在値の設定
        masterVolume.minValue = 0;
        masterVolume.maxValue = 1;
        masterVolume.value = PlayerPrefs.GetFloat("MasterVolume", AudioListener.volume);

        bgmVolume.minValue = 0;
        bgmVolume.maxValue = 1;
        bgmVolume.value = PlayerPrefs.GetFloat("BGMVolume", 1);

        seVolume.minValue = 0;
        seVolume.maxValue = 1;
        seVolume.value = PlayerPrefs.GetFloat("SEVolume", 1);

        // isOnLegacyControl.isOnをスマホではデフォルト値をfalse、それ以外ではtrueにする
#if UNITY_IOS
        // iOSのみで実行したい処理
        int LegacyControlValue = PlayerPrefs.GetInt("LegacyControl", 0);
#elif UNITY_ANDROID
        // Androidのみで実行したい処理
        int LegacyControlValue = PlayerPrefs.GetInt("LegacyControl", 0);
#else
        int LegacyControlValue = PlayerPrefs.GetInt("LegacyControl", 1);
#endif

        // Int型をBool型に変換
        if (LegacyControlValue == 1)
        {
            isOnLegacyControl.isOn = true;
        }
        else
        {
            isOnLegacyControl.isOn = false;
        }

        // Int型をBool型に変換
        if (PlayerPrefs.GetInt("Vibration", 1) == 1)
        {
            isOnVibration.isOn = true;
        }
        else
        {
            isOnVibration.isOn = false;
        }

        speedVolume.minValue = (float)0.25;
        speedVolume.maxValue = 1;
        speedVolume.value = PlayerPrefs.GetFloat("SpeedVolume", (float)0.5);

        configPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        var keyboard = Keyboard.current;
        // escキーでポーズ
        if (pauseMenu.activeSelf == false)
        {
            if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
            {
                Pause();
            }
        }
        else
        {
            if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
            {
                Resume();
            }
        }
    }


    // Config画面の設定を適用
    public void OnClickApplyButton()
    {
        // PlayerPrefsに保存
        PlayerPrefs.SetFloat("MasterVolume", masterVolume.value);
        PlayerPrefs.Save();
        // AudioListenerに値を渡す
        AudioListener.volume = masterVolume.value;

        PlayerPrefs.SetFloat("BGMVolume", bgmVolume.value);
        PlayerPrefs.Save();
        // 正規化済みの値（0, 1)をデシベル(-80, 0)に変換
        var BGM_dB = Mathf.Clamp(Mathf.Log10(bgmVolume.value) * 20f, -80f, 0f);
        // audioMixerに値を渡す
        audioMixer.SetFloat("BGM", BGM_dB);

        PlayerPrefs.SetFloat("SEVolume", seVolume.value);
        PlayerPrefs.Save();
        // 正規化済みの値（0, 1)をデシベル(-80, 0)に変換
        var SE_dB = Mathf.Clamp(Mathf.Log10(seVolume.value) * 20f, -80f, 0f);
        // audioMixerに値を渡す
        audioMixer.SetFloat("SE", SE_dB);


        // Bool型をInt型に変換してPlayerPrefsに保存
        if (isOnLegacyControl.isOn == true)
        {
            PlayerPrefs.SetInt("LegacyControl", 1);
            PlayerPrefs.Save();

#if UNITY_IOS
            //iOSのみで実行したい処理
            fixedJoystick.SetActive(true);
#elif UNITY_ANDROID
            //Androidのみで実行したい処理
            fixedJoystick.SetActive(true);
#else

#endif

        }
        else
        {
            PlayerPrefs.SetInt("LegacyControl", 0);
            PlayerPrefs.Save();
            fixedJoystick.SetActive(false);
        }


        // Bool型をInt型に変換してPlayerPrefsに保存
        if (isOnVibration.isOn == true)
        {
            PlayerPrefs.SetInt("Vibration", 1);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("Vibration", 0);
            PlayerPrefs.Save();
        }

        PlayerPrefs.SetFloat("SpeedVolume", speedVolume.value);
        PlayerPrefs.Save();

        configPanel.SetActive(false);
    }


    // Config画面の設定をリセット
    public void OnClickDefaultButton()
    {
        masterVolume.value = 1;
        bgmVolume.value = 1;
        seVolume.value = 1;

        // isOnLegacyControl.isOnをスマホではデフォルト値をfalse、それ以外ではtrueにする
#if UNITY_IOS
        // iOSのみで実行したい処理
        isOnLegacyControl.isOn = false;
#elif UNITY_ANDROID
        // Androidのみで実行したい処理
        isOnLegacyControl.isOn = false;
#else
        isOnLegacyControl.isOn = true;
#endif

        isOnVibration.isOn = true;
        speedVolume.value = (float)0.5;
    }



    // ポーズ画面の表示
    private void Pause()
    {
        // Debug.Log ("Pause");
        // 時間停止
        Time.timeScale = 0;

        pauseMenu.SetActive(true);
        // クリックイベントの再設定
        pauseButton.GetComponent<ButtonController>().onClick.RemoveListener(Pause);
        pauseButton.GetComponent<ButtonController>().onClick.AddListener(Resume);

        pauseButton.GetComponent<Image>().sprite = resumeButtonSprite;
    }


    // 中断したゲームを再開
    private void Resume()
    {
        // Debug.Log ("Resumed");
        // 時間再開
        Time.timeScale = 1;

        pauseMenu.SetActive(false);
        
        // クリックイベントの再設定
        pauseButton.GetComponent<ButtonController>().onClick.RemoveListener(Resume);
        pauseButton.GetComponent<ButtonController>().onClick.AddListener(Pause);

        pauseButton.GetComponent<Image>().sprite = pauseButtonSprite;
    }

    // replayダイアログでOKボタンを推したときの処理
    Action replayAction = () => {
        // ダイアログボックス自体を消去する処理はMydialog.Confirm()内
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    };


    // titleダイアログでOKボタンを推したときの処理
    Action titleAction = () => {
        Time.timeScale = 1;
        SceneManager.LoadScene("idleScene");
    };

    // プレイデータを消去してリプレイ
    private void Replay()
    {
        MyDialog.Confirm("リプレイしますか ?", replayAction);
    }

    private void activeConfig()
    {
        Debug.Log("Config");
        // OnClickApplyButtonイベントを設定しConfig画面を表示
        applyButton.GetComponent<ButtonController>().onClick.AddListener(OnClickApplyButton);
        defaultButton.GetComponent<ButtonController>().onClick.AddListener(OnClickDefaultButton);
        configPanel.SetActive(true);
    }

    private void LoadTitle()
    {
        MyDialog.Confirm("タイトルへ移動しますか ?", titleAction);
    }

    // ゲームオーバーしたときにリトライするか聞く
    public void activeRetryPanel()
    {
        MyDialog.Confirm("RETRY ?\n Your " + gameControllerScript.scoreText.GetComponent<Text>().text, replayAction, titleAction, true);
        // retryPanel.SetActive(true);
        // retryYesButton.GetComponent<ButtonController>().onClick.AddListener(OnClickRetryYes);
        // retryNoButton.GetComponent<ButtonController>().onClick.AddListener(OnClickRetryNo);
    }

    // private void OnClickRetryYes()
    // {
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    // }

    // private void OnClickRetryNo()
    // {
    //     SceneManager.LoadScene("idleScene");
    // }
}
