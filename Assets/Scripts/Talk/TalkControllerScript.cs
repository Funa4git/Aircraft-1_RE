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
    // gameControllerスクリプトを格納する変数
    private GameControllerScript gameController;
    // 会話パネル
    GameObject talkPanel;
    private TMP_Text nameText;
    private TMP_Text talkText;
    // CSVファイル
    public TextAsset csvFile;
    private List<string> csvData = new List<string>();
    // スコアテキスト
    private Text scoreText;
    // ダイアログデータ
    private List<DialogueData> dialogueDataList = new List<DialogueData>();
    // 現在の会話インデックス
    private int currentIndex = 0;
    // 立ち絵のImageオブジェクト
    public Image CharacterImages_Left;
    public Image CharacterImages_Right;
    // プレハブのCanvasをインスペクタから取得
    public GameObject characterCanvas;  // 立ち絵を表示するCanvas
    public GameObject eventTriggerCanvas;   // イベントトリガーを発火させるパネルを表示するCanvas
    // イベントトリガーを発火させるパネル
    private GameObject talkEventTriggerPanel;

    private void Awake()
    {
        // csvファイルを読み込む
        LoadCsv();
        // csvファイルを解析する
        ParseCsvData();

        // Canvasコンポーネントを取得
        Canvas chaCanvas = characterCanvas.GetComponent<Canvas>();
        Canvas eveCanvas = eventTriggerCanvas.GetComponent<Canvas>();
        // CanvasのRenderModeを設定
        chaCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        eveCanvas.renderMode = RenderMode.ScreenSpaceCamera;
        // メインカメラを取得
        Camera mainCamera = Camera.main;
        // CanvasのRenderCameraに設定
        chaCanvas.worldCamera = mainCamera;
        eveCanvas.worldCamera = mainCamera;
        // CanvasのSorting Layerを設定
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
        for (int i = 1; i < csvData.Count; i++) // 0行目はヘッダーなので無視
        {
            string[] fields = csvData[i].Split(',');    // フィールドごとに分割

            // フィールドが足りない場合は空文字列で補完
            string name = (fields.Length > 0) ? fields[0] : "";
            string mainText = (fields.Length > 1) ? fields[1] : "";
            string voiceFolder = (fields.Length > 2) ? fields[2] : "";
            string voice = (fields.Length > 3) ? fields[3] : "";
            string left = (fields.Length > 4) ? fields[4] : "";
            string leftStatus = (fields.Length > 5) ? fields[5] : "";
            string right = (fields.Length > 6) ? fields[6] : "";
            string rightStatus = (fields.Length > 7) ? fields[7] : "";
            // 8列目以降は無視（8列目がないと7列目が読み込み不良になるバグがあるため。原因不明）

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
        // EventTriggerCanvasのTalkEventTriggerPanelオブジェクトを取得
        talkEventTriggerPanel = eventTriggerCanvas.transform.Find("TalkEventTriggerPanel").gameObject;
        // EventTriggerコンポーネントを取得
        EventTrigger eventTrigger = talkEventTriggerPanel.GetComponent<EventTrigger>();
        // EventTrigger.Entryを作成
        EventTrigger.Entry entry = new EventTrigger.Entry();
        // イベントの種類を設定
        entry.eventID = EventTriggerType.PointerClick;
        // イベントが実行されたときに呼び出される関数を設定
        entry.callback.AddListener((data) => { DisplayDialogue(); });
        // EventTrigger.Entryを追加
        eventTrigger.triggers.Add(entry);

        
        // ScoreTextオブジェクトを取得
        scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        // スコアを非表示
        scoreText.enabled = false;

        // TalkPanelオブジェクトを取得
        GameObject canvasObject = GameObject.Find("TalkCanvas");
        Transform talkPanelTransform = canvasObject.transform.Find("TalkPanel");
        if(talkPanelTransform != null)
        {
            talkPanel = talkPanelTransform.gameObject;
        }
        else
        {
            Debug.Log("TalkPanelオブジェクトが見つかりません");
        }
        talkPanel.SetActive(true);      // TalkPanelを表示
        nameText = GameObject.Find("NameText").GetComponent<TMP_Text>();        // NameTextオブジェクトを取得
        talkText = GameObject.Find("TalkText").GetComponent<TMP_Text>();        // TalkTextオブジェクトを取得

        talkPanel.SetActive(true);      // TalkPanelを表示

        // 会話データを表示
        DisplayDialogue();


        // GameControllerScriptのGameClear()関数を呼び出したい
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>();
    }


    // Update is called once per frame
    void Update()
    {

    }

    // 会話データを表示、音声の再生、立ち絵の表示
    void DisplayDialogue()
    {
        if (currentIndex < dialogueDataList.Count - 1)
        {
            DialogueData currentDialogue = dialogueDataList[currentIndex];
            nameText.text = currentDialogue.Name;
            string loadedText = currentDialogue.MainText;
            talkText.text = loadedText.Replace("\\n", "\n");


            // ボイス再生
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

            // キャラクター画像を表示
            UpdateCharacterImage(CharacterImages_Left, currentDialogue.Left, currentDialogue.LeftStatus);   // 左側の立ち絵を表示
            UpdateCharacterImage(CharacterImages_Right, currentDialogue.Right, currentDialogue.RightStatus);    // 右側の立ち絵を表示

            // 会話が終わったらインデックスを進める
            currentIndex++;
        }
        else
        {
            // 会話が終わった後の処理
            gameController.TalkClear();
            // 会話パネルを非表示
            talkPanel.SetActive(false);
            // スコアを表示
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
                // 立ち絵を表示
                image.sprite = characterSprite;
                image.enabled = true;
                // 画像のサイズを取得
                float width = characterSprite.rect.width;
                float height = characterSprite.rect.height;
                // Imageオブジェクトのサイズを画像のサイズに合わせる
                image.rectTransform.sizeDelta = new Vector2(width / 3, height / 3);
            }
            else
            {
                // 立ち絵が見つからない場合はエラー文と立ち絵の非表示
                Debug.LogWarning($"Character sprite not found for: Character/{characterName}/{characterStatus}");
                image.enabled = false;
            }
        }
        else
        {
            // 指定がない場合は立ち絵を非表示
            Debug.Log("NO CHARACTER PART");
            image.enabled = false;
        }
    }

    private void OnDestroy()
    {

    }
}
