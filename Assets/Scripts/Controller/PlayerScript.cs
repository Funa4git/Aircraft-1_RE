using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScript : MonoBehaviour
{
    // Input System
    private GameInputs _gameInputs;
    private Vector2 _moveInputValue;

    // プレイヤー機体の制御


    // 球の発射速度
    public float fireSpeed = 0.2f;
    // 爆発モーション
    public GameObject explosion;

    private GameControllerScript gameController;

    public Transform firePoint; //球を発射する位置
    public GameObject bulletPrefab; //球のプレハブ

    // 弾の発射音SE
    AudioSource audioSource;
    public AudioClip shotSE;

    // Canvasをインスペクタから取得
    private GameObject canvas;
    private GameObject talkPanel;


    // Start()より先に処理される
    private void Awake()
    {
        // Input Actionインスタンス生成
        _gameInputs = new GameInputs();

        // Actionイベント登録
        _gameInputs.Player.Move.started += OnMove;
        _gameInputs.Player.Move.performed += OnMove;
        _gameInputs.Player.Move.canceled += OnMove;

        // Input Actionを機能させるためには、有効化する必要がある
        _gameInputs.Enable();

        // Canvasを取得
        canvas = GameObject.Find("TalkCanvas");
        // Canvas配下のTalkPanelを取得
        talkPanel = canvas.transform.Find("TalkPanel").gameObject;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Moveアクションの入力取得
        _moveInputValue = context.ReadValue<Vector2>();
    }


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // 音源のコンポーネントを取得
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>();
        StartCoroutine("Shoot"); // 弾の発射コルーチン
    }

    // Update is called once per frame
    void Update()
    {
        float x = 0;
        float y = 0;

        // 加速度センサによる操作
        if (Accelerometer.current!=null && PlayerPrefs.GetInt("LegacyControl", 0)==0)
        {
            var vectorAccelerometer = Accelerometer.current.acceleration.ReadValue();
            x = vectorAccelerometer.x * 2.0f;
            y = vectorAccelerometer.y * 2.0f;
        }
        else
        {
            // プレイヤー移動・球の発射
            x = _moveInputValue.x;
            y = _moveInputValue.y;
        }

        Vector3 nextPosition = transform.position + new Vector3(x, y, 0) * Time.deltaTime * 8f * PlayerPrefs.GetFloat("SpeedVolume", (float)0.5);
        // x(-1.9, 2.2), y(-2.95, 2.95) 移動範囲制御
        nextPosition = new Vector3(
            Mathf.Clamp(nextPosition.x, -4f, 4f),
            Mathf.Clamp(nextPosition.y, -1.9f, 2.2f),
            nextPosition.z
        );

        if(!talkPanel.activeSelf)
        {
            transform.position = nextPosition;
        }

    }

    
    IEnumerator Shoot()
    {
        while (true)
        {
            // 弾の発射間隔
            yield return new WaitForSeconds(fireSpeed);
            // TalkPanelが非表示なら弾の発射
            if (!talkPanel.activeSelf)
            {
                Instantiate(bulletPrefab, firePoint.position, transform.rotation);
                audioSource.PlayOneShot(shotSE);
            }
        }
    }

     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            // 爆発,敵・弾の消去
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(collision.gameObject);
            Destroy(gameObject);
            gameController.GameOver();
        }
    }
}
