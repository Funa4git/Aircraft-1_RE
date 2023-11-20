using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour
{
    // 敵性オブジェクトの機雷(Mine)制御スクリプト
    // MineはEnemyScriptから独立中(統合してもいいかもしれない)
    public GameObject explosion;
    public GameObject biggerExplosion;
    private Animator animator;

    public int hp = 1; // ヒットポイント初期値
    public int point = 50; // scoreポイント初期値

    // GameControllerScriptsクラスのメソッドを呼び出したい
    private GameControllerScript gameController;

    // Start is called before the first frame update
    void Start()
    {
        // ヒエラルキー上のGameContorollerという名前のオブジェクトを取得
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, -0.3f, 0) * Time.deltaTime;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            // Bulletコンポーネントを取得
            BulletScript bullet = GameObject.FindWithTag("Bullet").GetComponent<BulletScript>();
            hp = hp - bullet.power;
            Destroy(collision.gameObject);

            if(hp <= 0)
            {
                // 爆発発生, 敵・弾の消去
                Instantiate(explosion, transform.position, transform.rotation);
                Destroy(gameObject);
                gameController.AddScore(point);
            }else{
                GetAnimator().SetTrigger("Damage");
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            // 爆発,敵・弾の消去
            Instantiate(biggerExplosion, transform.position, transform.rotation);
            Instantiate(explosion, collision.transform.position, collision.transform.rotation);
            Destroy(collision.gameObject);
            Destroy(gameObject);
            gameController.GameOver();
        }
    }
    public Animator GetAnimator()
    {
        return animator;
    }

}