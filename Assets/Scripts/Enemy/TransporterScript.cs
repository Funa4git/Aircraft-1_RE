using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransporterScript : MonoBehaviour
{
    // 敵機体(EnemyTransporter)の制御スクリプト
    // 6つの発射位置
    public Transform firepoint1;
    public Transform firepoint2;
    public Transform firepoint3;
    public Transform firepoint4;
    public Transform firepoint5;
    public Transform firepoint6;
    // 追尾弾(bulletMiddle)
    public GameObject bulletM;

    Transform target;

    // 機雷(Mine)の生成用
    public GameObject mine;
    public float mineSetPosRange = 5.0f;

    Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        
        if (GameObject.Find("Player") != null )
        {
            target = GameObject.Find("Player").GetComponent<Transform>();
        }
        StartCoroutine(CPU());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 移動、攻撃モーション制御
    IEnumerator CPU()
    {
        while(transform.position.y > 1.5f)
        {
            transform.position -= new Vector3(0, 1, 0)*Time.deltaTime;
            yield return null;
        }
        while (true)
        {
            yield return RandomCor(30);
            yield return new WaitForSeconds(1.5f);
            yield return StepRepeatNWaveM(10, 2);
            yield return new WaitForSeconds(1.5f);
        }
    }

    // 弾をn回,m回ウェーブ打つ
    IEnumerator StepRepeatNWaveM(int n, int m)
    {
        
        for(int i = 0; i < m; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                for(int k = 0; k < n; k++)
                {
                    StepShot(j);
                    yield return new WaitForSeconds(0.15f);
                }
                yield return new WaitForSeconds(0.6f);
            }
            yield return new WaitForSeconds(1.3f);
        }
    }
    // 手前の発射位置から下段, 中段, 上段と弾を生成
    void StepShot(int step)
    {
        if (target == null)
            return;
        if(step == 0)
        {
            Instantiate(bulletM, firepoint1.position, transform.rotation);
            Instantiate(bulletM, firepoint4.position, transform.rotation);
        }else if(step == 1)
        {
            Instantiate(bulletM, firepoint2.position, transform.rotation);
            Instantiate(bulletM, firepoint5.position, transform.rotation);
        }else if(step == 2)
        {
            Instantiate(bulletM, firepoint3.position, transform.rotation);
            Instantiate(bulletM, firepoint6.position, transform.rotation);
        }
    }

    // RandomShotのコルーチン
    IEnumerator RandomCor(int m)
    {
        for(int i = 0; i < m; i++)
        {
            RandomShot();
            yield return new WaitForSeconds(0.3f);
        }
    }

    // ランダムな位置の発射口から弾を生成
    void RandomShot()
    {
        if (target == null)
            return;
        int Value = Random.Range(0, 6);
        if(Value == 0)
        {
            Instantiate(bulletM, firepoint1.position, transform.rotation);
        }
        if(Value == 1)
        {
            Instantiate(bulletM, firepoint2.position, transform.rotation);
        }
        if(Value == 2)
        {
            Instantiate(bulletM, firepoint3.position, transform.rotation);
        }
        if(Value  == 3)
        {
            Instantiate(bulletM, firepoint4.position, transform.rotation);
        }
        if(Value == 4)
        {
            Instantiate(bulletM, firepoint5.position, transform.rotation);
        }
        if(Value == 5)
        {
            Instantiate(bulletM, firepoint6.position, transform.rotation);
        }
        
    }

    //// Mineのn回生成
    //IEnumerator GeneMines(int n)
    //{
    //    for(int i = 0; i < n; i++)
    //    {
    //        PutMine();
    //        yield return new WaitForSeconds(0.8f);
            
    //    }
    //}
    //// Mineの生成
    //void PutMine()
    //{
        
    //    if (0 < mineSetPosRange)
    //    {
    //        var xr = Random.Range(-mineSetPosRange, mineSetPosRange);
    //        position = transform.position + new Vector3(xr, 0, 0);
    //    }
    //     Instantiate(mine, position, transform.rotation);
    //}

    // void MoveHorizontal()
    // {
        // 機体が左右に動く:未実装
    // }
}
