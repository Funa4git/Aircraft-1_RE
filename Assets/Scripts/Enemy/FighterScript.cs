using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;




// 敵機体(EnemyFighter)のスクリプト
public class FighterScript : MonoBehaviour
{
    // 敵弾の発射位置
    public Transform mainFirePoint;     // メインの発射地点
    public Transform missilePointRight;     // ミサイルの発射地点（右）
    public Transform missilePointLeft;      // ミサイルの発射地点（左）

    // 弾のオブジェクト
    public GameObject missile;      // ミサイルのオブジェクト
    public GameObject beamBullet_1;     // 赤弾のオブジェクト
    public GameObject beamBullet_2;     // 青弾のオブジェクト

    // 攻撃パターンのスクリプト
    public BeamAttackScript beamAttackScript;
    public MissileAttackScript missileAttackScript;

    // プレイヤー位置
    Transform target;

    // HP
    EnemyScript enemyScript;
    int maxHp;




    // Start is called before the first frame update
    void Start()
    {
        // 攻撃パターンのスクリプトを取得
        beamAttackScript = GetComponent<BeamAttackScript>();
        // 攻撃パターンのスクリプトを取得
        missileAttackScript = GetComponent<MissileAttackScript>();

        // HPの取得
        enemyScript = GetComponent<EnemyScript>();
        maxHp = enemyScript.hp;


        if (GameObject.Find("Player") != null )
        {
            target = GameObject.Find("Player").GetComponent<Transform>();
        }

        // Update関数と同等の処理
        StartCoroutine(CPUf());
    }


    void OnDestroy()
    {

    }



    private void beamForPlayer()
    {
        float localSpeed = 35.0f;
        float localAngleWidth = 7.0f;

        if (target != null)
        {
            // 外部スクリプトから攻撃の生成
            beamAttackScript?.targetAttack(beamBullet_1, mainFirePoint.position, target.position, localSpeed);
            // +度
            beamAttackScript?.targetAttack(beamBullet_1, mainFirePoint.position, Quaternion.Euler(0, 0, localAngleWidth) * target.position, localSpeed);
            // -度
            beamAttackScript?.targetAttack(beamBullet_1, mainFirePoint.position, Quaternion.Euler(0, 0, -1 * localAngleWidth) * target.position, localSpeed);
        }
    }

    // 上段へのビーム攻撃
    private void beamHigh()
    {
        if (target != null)
        {
            // 外部スクリプトから攻撃の生成
            beamAttackScript?.beamHigh(beamBullet_2, mainFirePoint.position);
        }
    }

    // 中段へのビーム攻撃
    private void beamMid()
    {
        if (target != null)
        {
            // 外部スクリプトから攻撃の生成
            beamAttackScript?.beamMid(beamBullet_2, mainFirePoint.position);
        }
    }

    // 下段へのビーム攻撃
    private void beamLow()
    {
        if (target != null)
        {
            // 外部スクリプトから攻撃の生成
            beamAttackScript?.beamLow(beamBullet_2, mainFirePoint.position);
        }
    }

    // 全方位へのビーム攻撃１
    private void beamAll1()
    {
        if (target != null)
        {
            // 外部スクリプトから攻撃の生成
            beamAttackScript?.AllAttack_1(beamBullet_1, mainFirePoint.position);
        }
    }

    // 全方位へのビーム攻撃２
    private void beamAll2()
    {
        if (target != null)
        {
            // 外部スクリプトから攻撃の生成
            beamAttackScript?.AllAttack_2(beamBullet_2, mainFirePoint.position);
        }
    }

    // プレイヤーに向かってライン攻撃１
    private void beamLineForPlayer1()
    {
        if (target != null)
        {
            // 外部スクリプトから攻撃の生成
            StartCoroutine(beamAttackScript?.continuousAttack(beamBullet_1, mainFirePoint.position, target.position));
        }
    }

    // プレイヤーに向かってライン攻撃２
    private void beamLineForPlayer2()
    {
        if (target != null)
        {
            // 外部スクリプトから攻撃の生成
            StartCoroutine(beamAttackScript?.continuousAttack(beamBullet_2, mainFirePoint.position, target.position));
        }
    }

    // 2連ビーム攻撃
    private IEnumerator beamDouble()
    {
        if (target != null)
        {
            // プレイヤーの位置を一時保存
            Vector3 localPosition = target.position;

            // 外部スクリプトから攻撃の生成
            beamAttackScript?.targetAttack(beamBullet_1, mainFirePoint.position, localPosition, 40.0f);
            yield return new WaitForSeconds(0.15f);
            beamAttackScript?.targetAttack(beamBullet_1, mainFirePoint.position, localPosition, 40.0f);
        }
    }

    // ミサイル攻撃
    private void missileForPlayer()
    {
        if (target != null)
        {
            // 外部スクリプトから攻撃の生成
            missileAttackScript?.targetAttack(missile, missilePointRight.position);
            missileAttackScript?.targetAttack(missile, missilePointLeft.position);
        }
    }

    // beamのランダム攻撃
    private void beamRandomAttack()
    {
        if (target != null)
        {
            // 外部スクリプトから攻撃の生成
            beamAttackScript?.RandomAttack(beamBullet_1, mainFirePoint.position);
        }
    }


    
    // 移動・攻撃モーション制御
    IEnumerator CPUf()
    {
        // 移動
        while(transform.position.y > 1.5f)
        {
            transform.position -= new Vector3(0, 1, 0)*Time.deltaTime;
            yield return null;
        }

        // Update関数と同等の処理
        while(true)
        {
            if((float)enemyScript.currentHp/maxHp*100 > 75)
            {
                beamForPlayer();
                yield return new WaitForSeconds(1.0f);
            }
            else if ((float)enemyScript.currentHp / maxHp * 100 > 50)
            {
                beamLineForPlayer1();
                yield return new WaitForSeconds(1.0f);
                beamLineForPlayer2();
                yield return new WaitForSeconds(1.0f);
                StartCoroutine(beamDouble());
                yield return new WaitForSeconds(0.8f);
                StartCoroutine(beamDouble());
                yield return new WaitForSeconds(0.8f);
                StartCoroutine(beamDouble());
                yield return new WaitForSeconds(0.8f);
                StartCoroutine(beamDouble());
                yield return new WaitForSeconds(1.5f);
            } else if((float)enemyScript.currentHp / maxHp * 100 > 25)
            {
                beamHigh();
                yield return new WaitForSeconds(1.0f);
                beamMid();
                yield return new WaitForSeconds(1.0f);
                beamLow();
                yield return new WaitForSeconds(1.5f);
                beamAll1();
                yield return new WaitForSeconds(0.8f);
                beamAll2();
                yield return new WaitForSeconds(1.0f);
            } else
            {
                beamRandomAttack();
                yield return new WaitForSeconds(0.1f);
            }
            Debug.Log((float)enemyScript.currentHp / maxHp * 100);
            
            // 1秒待機
            yield return new WaitForSeconds(0.1f);
        }
    }
}
