using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    // 敵弾オブジェクト(Missile)を制御するスクリプト
    // 発射音
    AudioSource audioSource;
    public AudioClip missileShootSE;

    // currentVelocity
    Vector3 velocity;
    // ターゲット位置格納
    Transform target;

    public float period = 2; // 当たるまでの時間
    public float maxAcceleration = 100;

    // 発射時ランダムに加える力
    public float randomPower;
    public float randomPeriod;

    private float timer = 0f;
    public GameObject explosion;

    void Start() {
        
        if (GameObject.Find("Player") != null )
        {
            target = GameObject.Find("Player").GetComponent<Transform>();
        }
        // 発射SE再生
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(missileShootSE);

        StartCoroutine("UpdateCoroutine");
    }
    IEnumerator UpdateCoroutine()
    {
        while (timer <= 6.0f)
        {
            if (target == null)
                break;

            // 加速度の更新
            var acceleration = new Vector3(0, -1f, 0);
            // ターゲットとの変位を変数diffに格納
            var diff = target.position - transform.position;
            // 位置関係によって加速度を追加
            acceleration += (diff - velocity * period) * 2f / (period * period);

            // ランダムに加える力の追加
            if (0 < randomPeriod)
            {
                var xr = Random.Range(-randomPower, randomPower);
                var yr = Random.Range(-randomPower, randomPower);
                var zr = Random.Range(-randomPower, randomPower);
                acceleration += new Vector3(xr, yr, zr);
            }

            // 超過加速度の天引き
            if (acceleration.magnitude > maxAcceleration)
            {
                acceleration = acceleration.normalized * maxAcceleration;
            }

            // 当たるまでの時間をフレームレートごとに減らしていく。
            period -= Time.deltaTime;
            randomPeriod -= Time.deltaTime;
            if (period < 0f)
            {
                transform.position += velocity * Time.deltaTime;
            }
            else
            {
                velocity += acceleration * Time.deltaTime;
                transform.position += velocity * Time.deltaTime;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, -diff);
            }

            timer += Time.deltaTime;
            yield return null;
        }

        // 爆発発生, 敵・弾の消去
        Instantiate(explosion, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }
}
