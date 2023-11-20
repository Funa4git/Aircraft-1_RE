using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSScript : MonoBehaviour
{
    // どこにアタッチされているスクリプトか調査中…。
    // もしかしたら過去の素材にアタッチされていたスクリプト。

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 2f, 0) * Time.deltaTime;
    }
}
