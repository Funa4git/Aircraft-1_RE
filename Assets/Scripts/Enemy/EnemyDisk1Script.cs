using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDisk1Script : MonoBehaviour
{
    // 弾を発射する円盤型機体(enemyDisk1)の制御
    public Transform disksFirePoint;
    public GameObject diskBullet;

    // 横揺れ用変数
    private float phase;
    // 打つときのモーション用
    Animator animator;
    // private float speed;
    public float shotDelay;
    // 累計時間の計測
    private float timer;

    // public GameObject bulletEnemy1;
    // Start is called before the first frame update
    void Start()
    {
        phase = Random.Range(0f, Mathf.PI / 2);
        animator = GetComponent<Animator>();
        StartCoroutine(UpdateCoroutine());
        StartCoroutine("Shoot");
    }

    // Update is called once per frame
    private IEnumerator UpdateCoroutine()
    {
        // 所定位置まで移動
        while (transform.position.y > 1.75f)
        {
            timer += Time.deltaTime;

            // 敵をy軸下向きに移動
            transform.position += new Vector3(
                Mathf.Cos(timer * 1f + phase),
                -0.8f,
                0f
            ) * Time.deltaTime;
            yield return null;
        }

        // x方向にのみ移動
        while (true)
        {
            timer += Time.deltaTime;

            // 敵をy軸下向きに移動
            transform.position += new Vector3(
                Mathf.Cos(timer * 1f + phase),
                0,
                0f
            ) * Time.deltaTime;
            yield return null;
        }
    }
    

    IEnumerator Shoot()
    {
        
        yield return new WaitForSeconds(shotDelay);
        
        while (true)
        {
            Instantiate(diskBullet, disksFirePoint.position, transform.rotation);
            animator.SetTrigger("Attack");
            // audioSource.PlayOneShot(shotSE);
            yield return new WaitForSeconds(shotDelay);
        }
    }
}
