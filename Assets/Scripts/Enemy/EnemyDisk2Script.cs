using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDisk2Script : MonoBehaviour
{
    // 弾を発射しない円盤型機体(enemyDisk2)の制御
    private float phase;
    // 累計時間の計測
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        phase = Random.Range(0f, Mathf.PI / 2);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        transform.position += new Vector3(
            Mathf.Cos(timer * 1f + phase),
            -0.4f,
            0f
        ) * Time.deltaTime;
    }
}
