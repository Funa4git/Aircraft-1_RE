using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFirstScript : MonoBehaviour
{
    // 弾のオブジェクト
    public GameObject bulletM;
    // 敵弾の発射位置
    public Transform mainFirePoint;
    // プレイヤー位置
    Transform target;

    bool isIdleScene;

    // 雑魚敵(Enemy)の制御
    // Start is called before the first frame update
    void Start()
    {
        isIdleScene = (GameObject.Find("StartController") != null);

        if (GameObject.Find("Player") != null)
        {
            target = GameObject.Find("Player").GetComponent<Transform>();
        }

        StartCoroutine(UpdateCoroutine());
    }


    // Update is called once per frame
    private IEnumerator UpdateCoroutine()
    {
        
        if(isIdleScene){
            while (true)
            {
                transform.position += new Vector3(0, -0.8f, 0) * Time.deltaTime;
                yield return null;
            }
        }else{
            while (transform.position.y > 1.75f)
            {
                transform.position += new Vector3(0, -0.8f, 0) * Time.deltaTime;
                yield return null;
            }
            while (true)
            {
                yield return MainShotN(1);
                yield return new WaitForSeconds(2f);
            }
        }
    }

    private IEnumerator MainShotN(int n)
    {
        for (int i = 0; i < n; i++)
        {
            MainShoot();
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void MainShoot()
    {
        if (target == null)
            return;
        Instantiate(bulletM, mainFirePoint.position, transform.rotation);
    }
}
