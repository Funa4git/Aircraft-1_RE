using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class GameInitializerScript : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;


    // Start is called before the first frame update
    void Start()
    {
        ApplyConfigSettings();

        if (Accelerometer.current != null)
        {
            OnEnableAccelerometer();
        }
        Debug.Log("加速度センサの状態（有効：Accelerometer、無効：Null");
        Debug.Log(Accelerometer.current);
    }



    // Config��ʂ̏�����
    void ApplyConfigSettings()
    {
        // �ŏ��l�A�ō��l�A���ݒl�̐ݒ�
        AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", AudioListener.volume);

        var normalize_bgm = PlayerPrefs.GetFloat("BGMVolume", 1);
        // 0,1���f�V�x��(-80,0)�ɕϊ�
        var BGM_dB = Mathf.Clamp(Mathf.Log10(normalize_bgm) * 20f, -80f, 0f);
        audioMixer.SetFloat("BGM", BGM_dB);

        var normalize_se = PlayerPrefs.GetFloat("SEVolume", 1);
        // 0,1���f�V�x��(-80,0)�ɕϊ�
        var SE_dB = Mathf.Clamp(Mathf.Log10(normalize_se) * 20f, -80f, 0f);
        audioMixer.SetFloat("SE", SE_dB);


        // PlayerPrefsに端末ごとの優先初期値を保存
#if UNITY_IOS
        PlayerPrefs.SetInt("LegacyControl", PlayerPrefs.GetInt("LegacyControl", 0));
        PlayerPrefs.Save();
#elif UNITY_ANDROID
        PlayerPrefs.SetInt("LegacyControl", PlayerPrefs.GetInt("LegacyControl", 0));
        PlayerPrefs.Save();
#else
        PlayerPrefs.SetInt("LegacyControl", PlayerPrefs.GetInt("LegacyControl", 1));
        PlayerPrefs.Save();
#endif

        if (SystemInfo.supportsVibration)
        {
            PlayerPrefs.SetInt("Vibration", PlayerPrefs.GetInt("Vibration", 1));
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("Vibration", PlayerPrefs.GetInt("Vibration", 0));
            PlayerPrefs.Save();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnEnableAccelerometer()
    {
        InputSystem.EnableDevice(Accelerometer.current);
    }

    void OnDisableAccelerometer()
    {
        InputSystem.DisableDevice(Accelerometer.current);
    }
}
