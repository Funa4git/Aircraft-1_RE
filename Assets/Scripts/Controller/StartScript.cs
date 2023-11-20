using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using UnityEngine.Rendering;


public class StartScript : MonoBehaviour
{
    // スタート待機画面(idleScene)での制御

    private InputAction nextAction, menuAction;

    [SerializeField] private GameObject gameStartButton;
    [SerializeField] private GameObject stageSelectButton;
    [SerializeField] private GameObject configButton;
    [SerializeField] private GameObject creditButton;
    [SerializeField] private GameObject exitButton;

    [SerializeField] private GameObject inactiveSSButton; // stageSelectPanel非表示用ボタン
    [SerializeField] private GameObject stageSelectPanel;

    [SerializeField] private GameObject[] stageButtons;


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



    private void Awake()
    {
        nextAction = GetComponent<PlayerInput>().currentActionMap["Next"];
        menuAction = GetComponent<PlayerInput>().currentActionMap["Menu"];
    }

    IEnumerator Start(){
        while (!SplashScreen.isFinished)
        {
            yield return null;
        }

        stageSelectPanel.SetActive(true); // ステージ選択ボタン処理の後にfalse こうしないとInitializeInteractable()が上手くいかない
        gameStartButton.GetComponent<ButtonController>().onClick.AddListener(ActiveGameStart);
        stageSelectButton.GetComponent<ButtonController>().onClick.AddListener(ActiveStageSelect);
        configButton.GetComponent<ButtonController>().onClick.AddListener(ActiveConfig);
        creditButton.GetComponent<ButtonController>().onClick.AddListener(ActiveCredit);

#if UNITY_IOS || UNITY_ANDROID
        // iOSのみで実行したい処理
        // 非表示処理の記述
#else
        exitButton.SetActive(true);
        exitButton.GetComponent<ButtonController>().onClick.AddListener(ActiveExit);
#endif

        inactiveSSButton.GetComponent<ButtonController>().onClick.AddListener(InactiveStageSelect);

        // ステージ制限
        int unlockedStageNum = PlayerPrefs.GetInt("unlockedStageNum", 1);   // Stage名の番号と同じ（１からスタート）
        PlayerPrefs.Save();
        for (int i = 0; i < stageButtons.Length; i++)
        {
            if (i < unlockedStageNum){
                stageButtons[i].GetComponent<ButtonController>().interactable = true;
            }else{
                stageButtons[i].GetComponent<ButtonController>().interactable = false;
            }
            stageButtons[i].GetComponent<ButtonController>().InitializeInteractable();
        }
        
        stageSelectPanel.SetActive(false);


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

        if (nextAction.triggered == true)
        {
            // 第一ステージを選択
            PlayerPrefs.SetInt("Selected_Scene_obj_Prefs", 0);
            PlayerPrefs.Save();
            SceneManager.LoadScene("Master");
            Debug.Log("StartScript_Next");
        }
        
        if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
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
        }
        else
        {
            PlayerPrefs.SetInt("LegacyControl", 0);
            PlayerPrefs.Save();
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


    private void ActiveGameStart()
    {
        Debug.Log("Game Start");

        // 第一ステージを選択
        PlayerPrefs.SetString("Selected_Scene_obj_Prefs", "Talk_01_SetUp");
        PlayerPrefs.Save();
        SceneManager.LoadScene("Master");
    }

    private void ActiveStageSelect()
    {
        Debug.Log("Stage Select");
        stageSelectPanel.SetActive(true);
    }
    private void ActiveConfig()
    {
        Debug.Log("Config");
        // OnClickApplyButtonイベントを設定しConfig画面を表示
        applyButton.GetComponent<ButtonController>().onClick.AddListener(OnClickApplyButton);
        defaultButton.GetComponent<ButtonController>().onClick.AddListener(OnClickDefaultButton);
        configPanel.SetActive(true);
    }

    private void ActiveCredit()
    {
        Debug.Log("Add Credit Scene");
        SceneManager.LoadScene("Endroll");
    }
    private void ActiveExit()
    {
        Debug.Log("Add Exit");
        Application.Quit();
    }

    void InactiveStageSelect(){
        stageSelectPanel.SetActive(false);
    }
}
