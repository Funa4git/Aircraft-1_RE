using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[System.Serializable]
public class DialogueData
{
    public string Name;
    public string MainText;
    public string VoiceFolder;
    public string Voice;
    public string Left;
    public string LeftStatus;
    public string Right;
    public string RightStatus;
}

public class TalkControllerScript : MonoBehaviour
{
    // gameController�X�N���v�g���i�[����ϐ�
    private GameControllerScript gameController;
    // ��b�p�l��
    GameObject talkPanel;
    private TMP_Text nameText;
    private TMP_Text talkText;
    // CSV�t�@�C��
    public TextAsset csvFile;
    private List<string> csvData = new List<string>();
    // �X�R�A�e�L�X�g
    private Text scoreText;
    // �_�C�A���O�f�[�^
    private List<DialogueData> dialogueDataList = new List<DialogueData>();
    // ���݂̉�b�C���f�b�N�X
    private int currentIndex = 0;
    // �����G��Image�I�u�W�F�N�g
    public Image CharacterImages_Left;
    public Image CharacterImages_Right;
    // �v���n�u��Canvas���C���X�y�N�^����擾
    public GameObject characterCanvas;  // �����G��\������Canvas
    public GameObject eventTriggerCanvas;   // �C�x���g�g���K�[�𔭉΂�����p�l����\������Canvas
    // �C�x���g�g���K�[�𔭉΂�����p�l��
    private GameObject talkEventTriggerPanel;

    private void Awake()
    {
        // csv�t�@�C����ǂݍ���
        LoadCsv();
        // csv�t�@�C������͂���
        ParseCsvData();

        // Canvas�R���|�[�l���g���擾
        Canvas chaCanvas = characterCanvas.GetComponent<Canvas>();
        Canvas eveCanvas = eventTriggerCanvas.GetComponent<Canvas>();
        // Canvas��RenderMode��ݒ�
        chaCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        eveCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        // ���C���J�������擾
        Camera mainCamera = Camera.main;
        // Canvas��RenderCamera�ɐݒ�
        chaCanvas.worldCamera = mainCamera;
        eveCanvas.worldCamera = mainCamera;
        // Canvas��Sorting Layer��ݒ�
        chaCanvas.sortingLayerName = "Character";
        eveCanvas.sortingLayerName = "TalkEventTrigger";
    }

    void LoadCsv()
    {
        string[] data = csvFile.text.Split('\n');
        foreach (string row in data)
        {
            csvData.Add(row);
        }
    }

    void ParseCsvData()
    {
        for (int i = 1; i < csvData.Count; i++) // 0�s�ڂ̓w�b�_�[�Ȃ̂Ŗ���
        {
            string[] fields = csvData[i].Split(',');    // �t�B�[���h���Ƃɕ���

            // �t�B�[���h������Ȃ��ꍇ�͋󕶎���ŕ⊮
            string name = (fields.Length > 0) ? fields[0] : "";
            string mainText = (fields.Length > 1) ? fields[1] : "";
            string voiceFolder = (fields.Length > 2) ? fields[2] : "";
            string voice = (fields.Length > 3) ? fields[3] : "";
            string left = (fields.Length > 4) ? fields[4] : "";
            string leftStatus = (fields.Length > 5) ? fields[5] : "";
            string right = (fields.Length > 6) ? fields[6] : "";
            string rightStatus = (fields.Length > 7) ? fields[7] : "";
            // 8��ڈȍ~�͖����i8��ڂ��Ȃ���7��ڂ��ǂݍ��ݕs�ǂɂȂ�o�O�����邽�߁B�����s���j

            DialogueData dialogue = new DialogueData
            {
                Name = name,
                MainText = mainText,
                VoiceFolder = voiceFolder,
                Voice = voice,
                Left = left,
                LeftStatus = leftStatus,
                Right = right,
                RightStatus = rightStatus
            };
            dialogueDataList.Add(dialogue);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // EventTriggerCanvas��TalkEventTriggerPanel�I�u�W�F�N�g���擾
        talkEventTriggerPanel = eventTriggerCanvas.transform.Find("TalkEventTriggerPanel").gameObject;
        // EventTrigger�R���|�[�l���g���擾
        EventTrigger eventTrigger = talkEventTriggerPanel.GetComponent<EventTrigger>();
        // EventTrigger.Entry���쐬
        EventTrigger.Entry entry = new EventTrigger.Entry();
        // �C�x���g�̎�ނ�ݒ�
        entry.eventID = EventTriggerType.PointerClick;
        // �C�x���g�����s���ꂽ�Ƃ��ɌĂяo�����֐���ݒ�
        entry.callback.AddListener((data) => { DisplayDialogue(); });
        // EventTrigger.Entry��ǉ�
        eventTrigger.triggers.Add(entry);

        
        // ScoreText�I�u�W�F�N�g���擾
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        // �X�R�A���\��
        scoreText.enabled = false;

        // TalkPanel�I�u�W�F�N�g���擾
        GameObject canvasObject = GameObject.Find("TalkCanvas");
        Transform talkPanelTransform = canvasObject.transform.Find("TalkPanel");
        if(talkPanelTransform != null)
        {
            talkPanel = talkPanelTransform.gameObject;
        }
        else
        {
            Debug.Log("TalkPanel�I�u�W�F�N�g��������܂���");
        }
        talkPanel.SetActive(true);      // TalkPanel��\��
        nameText = GameObject.Find("NameText").GetComponent<TMP_Text>();        // NameText�I�u�W�F�N�g���擾
        talkText = GameObject.Find("TalkText").GetComponent<TMP_Text>();        // TalkText�I�u�W�F�N�g���擾

        talkPanel.SetActive(true);      // TalkPanel��\��

        // ��b�f�[�^��\��
        DisplayDialogue();


        // GameControllerScript��GameClear()�֐����Ăяo������
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>();
    }


    // Update is called once per frame
    void Update()
    {

    }

    // ��b�f�[�^��\���A�����̍Đ��A�����G�̕\��
    void DisplayDialogue()
    {
        if (currentIndex < dialogueDataList.Count - 1)
        {
            DialogueData currentDialogue = dialogueDataList[currentIndex];
            nameText.text = currentDialogue.Name;
            string loadedText = currentDialogue.MainText;
            talkText.text = loadedText.Replace("\\n", "\n");


            // �{�C�X�Đ�
            if(currentDialogue.Voice != "")
            {
                string voicePath = $"VOICE/{currentDialogue.VoiceFolder}/{Path.GetFileNameWithoutExtension(currentDialogue.Voice)}";
                AudioClip clip = Resources.Load<AudioClip>(voicePath);
                if (clip != null)
                {
                    AudioSource audioSource = GetComponent<AudioSource>();
                    audioSource.clip = clip;
                    audioSource.Play();
                }
                else
                {
                    Debug.LogWarning($"Voice clip not found for: {voicePath}");
                }
            } else
            {
                Debug.Log("NO VOICE PART");
            }

            // �L�����N�^�[�摜��\��
            UpdateCharacterImage(CharacterImages_Left, currentDialogue.Left, currentDialogue.LeftStatus);   // �����̗����G��\��
            UpdateCharacterImage(CharacterImages_Right, currentDialogue.Right, currentDialogue.RightStatus);    // �E���̗����G��\��

            // ��b���I�������C���f�b�N�X��i�߂�
            currentIndex++;
        }
        else
        {
            // ��b���I�������̏���
            gameController.TalkClear();
            // ��b�p�l�����\��
            talkPanel.SetActive(false);
            // �X�R�A��\��
            scoreText.enabled = true;
        }
    }

    void UpdateCharacterImage(Image image, string characterName, string characterStatus)
    {
        if (!string.IsNullOrEmpty(characterName) && !string.IsNullOrEmpty(characterStatus))
        {
            Sprite characterSprite = Resources.Load<Sprite>($"Character/{characterName}/{characterStatus}");
            if(characterSprite != null)
            {
                // �����G��\��
                image.sprite = characterSprite;
                image.enabled = true;
                // �摜�̃T�C�Y���擾
                float width = characterSprite.rect.width;
                float height = characterSprite.rect.height;
                // Image�I�u�W�F�N�g�̃T�C�Y���摜�̃T�C�Y�ɍ��킹��
                image.rectTransform.sizeDelta = new Vector2(width / 3, height / 3);
            }
            else
            {
                // �����G��������Ȃ��ꍇ�̓G���[���Ɨ����G�̔�\��
                Debug.LogWarning($"Character sprite not found for: Character/{characterName}/{characterStatus}");
                image.enabled = false;
            }
        }
        else
        {
            // �w�肪�Ȃ��ꍇ�͗����G���\��
            Debug.Log("NO CHARACTER PART");
            image.enabled = false;
        }
    }

    private void OnDestroy()
    {

    }
}
