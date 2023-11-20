using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 敵機体の共通部分の制御
// プレイヤーの弾に当ったらhp減少。hpがゼロで爆発。プレイヤーに当たったら爆発(Boss以外)。
// プレイヤーに倒されたらポイント加算
public class EnemyScript : MonoBehaviour
{
    public int hp = 1; // ヒットポイント最大値
    public int currentHp; // 現在のヒットポイント
    public int point = 100; // scoreポイント初期値

    // 機体爆発時のモーションオブジェクト格納
    public GameObject explosion;
    // プレイヤーが機体に当たった時のモーションオブジェクト格納
    public GameObject explosionPlayer;

    private Animator animator;

    // GameControllerScriptsクラスのメソッドを呼び出したい
    // AddScore()メソッド・GameOver()メソッド
    private GameControllerScript gameController;

    // 敵HPバー
    GameObject EnemyHealthBar_Background;
    Image EnemyHealth_Image;

    // Start is called before the first frame update
    void Start()
    {
        // currentHpの初期化
        currentHp = hp;

        // Tagでヒエラルキー上のGameContorollerという名前のオブジェクトを取得
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>();
        animator = GetComponent<Animator>();

        // 敵HPバーの取得
        EnemyHealthBar_Background = GameObject.Find("EnemyHealthBar_Background");
        if(EnemyHealthBar_Background != null)
        {
            EnemyHealth_Image = GameObject.Find("EnemyHealthBar_Bar").GetComponent<Image>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(EnemyHealthBar_Background != null)
        {
            // 敵HPバーが表示されている場合はHPを連動させる
            if (EnemyHealthBar_Background.activeSelf)
            {
                EnemyHealth_Image.fillAmount = (float)currentHp / hp;
            }
        }
    }
    
    // 弾やプレイヤーのオブジェクトに衝突した時の処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // プレイヤーの弾と衝突したときの処理
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Bulletコンポーネントを取得
            BulletScript bullet = GameObject.FindWithTag("Bullet").GetComponent<BulletScript>();
            currentHp = currentHp - bullet.power;
            Destroy(collision.gameObject);

            if(currentHp <= 0)
            {
                // 爆発発生, 敵機体・弾の消去, スコア加算
                Instantiate(explosion, transform.position, transform.rotation);
                Destroy(gameObject);
                gameController.AddScore(point);
            }else{
                // 被弾時は赤く表示する
                GetAnimator().SetTrigger("Damage");
            }
        }

        // プレイヤーと衝突したときの処理
        if (collision.gameObject.CompareTag("Player"))
        {
            /*
            Bossタグの有無によって挙動変更
            Bossタグが付いている敵(Enemy4, EnemyFighter, EnemyTransporter)
            */
            // Bossならプレイヤーだけ爆発
            if(this.gameObject.CompareTag("Boss")){
                Instantiate(explosionPlayer, collision.transform.position, collision.transform.rotation);
                Destroy(collision.gameObject);
                gameController.GameOver();
            }
            else // Boss以外なら敵機体も爆発
            {
                // 爆発,敵・弾の消去
                Instantiate(explosion, transform.position, transform.rotation);
                Instantiate(explosionPlayer, collision.transform.position, collision.transform.rotation);
                Destroy(collision.gameObject);
                Destroy(gameObject);
                gameController.GameOver();
            }
        }
    }

    public Animator GetAnimator()
    {
        return animator;
    }


    private void OnDestroy()
    {
        if (EnemyHealthBar_Background != null)
        {
            // 敵HPバーが表示されている場合はHPを連動させる
            if (EnemyHealthBar_Background.activeSelf)
            {
                EnemyHealth_Image.fillAmount = 0.0f;
            }
        }
    }

}
