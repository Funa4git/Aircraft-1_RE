using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskBulletScript : MonoBehaviour
{
    // enemyDisk1が発射する弾のスクリプト
    public float DiskBulletSpeed = 2.5f;

    // Update is called once per frame
    void Update()
    {
         transform.position += new Vector3(0, -DiskBulletSpeed, 0) * Time.deltaTime;
    }
}
