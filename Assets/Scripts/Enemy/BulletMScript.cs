using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMScript : MonoBehaviour
{
    // 追尾弾の移動制御コード
    Transform target; // ターゲット(プレイヤー)の位置

    float speed = 2;
    Vector3 diff; // 弾とプレイヤーの位置
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("Player") != null )
        {
            target = GameObject.Find("Player").GetComponent<Transform>();
        }

        diff = target.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += diff.normalized * speed * Time.deltaTime;
    }
}
