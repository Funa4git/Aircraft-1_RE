using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 単純な弾の移動制御コード
public class BeamBulletScript : MonoBehaviour
{
    private float speed = 0.0f; // 弾速
    private float lifeTimeSec = 7.0f; // 弾の生存時間
    private Vector3 targetPosition; // 弾の目標地点
    private Vector3 moveVector; // 移動方向


    // Init
    public void Initialize(Vector3 targetPosition, float speed)
    {
        this.targetPosition = targetPosition;   // 弾の進行方向を設定

        // 移動方向を設定
        moveVector = targetPosition - transform.position;
        moveVector.Normalize(); // 正規化
        // 弾速を設定
        this.speed = speed;

    }

    // Update is called once per frame
    void Update()
    {
        if(lifeTimeSec <= 0)
        {
            Destroy(gameObject); // 弾の削除
        }
        lifeTimeSec -= Time.deltaTime; // 弾の生存時間を減らす


        transform.position += moveVector * speed * 0.1f * Time.deltaTime; // 移動
    }
}
