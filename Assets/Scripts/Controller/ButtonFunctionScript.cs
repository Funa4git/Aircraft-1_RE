using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctionScript : MonoBehaviour
{

    public void LoadStageScene(string sceneName){
        // �X�e�[�W��I��
        PlayerPrefs.SetString("Selected_Scene_obj_Prefs", sceneName);
        PlayerPrefs.Save();
        // sceneName���f�o�b�O���O�ɕ\��
        Debug.Log(sceneName);
        SceneManager.LoadScene("Master");
    }
}
