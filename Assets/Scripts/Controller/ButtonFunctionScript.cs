using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctionScript : MonoBehaviour
{

    public void LoadStageScene(string sceneName){
        // ステージを選択
        PlayerPrefs.SetString("Selected_Scene_obj_Prefs", sceneName);
        PlayerPrefs.Save();
        // sceneNameをデバッグログに表示
        Debug.Log(sceneName);
        SceneManager.LoadScene("Master");
    }
}
