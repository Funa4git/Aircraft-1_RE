using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    // 爆発モーションオブジェクトの制御
    // SEの設定
    AudioSource audioSource;
    public AudioClip boomSE;

    // Start is called before the first frame update
    void Start()
    {
        // 生成後の処理
        Destroy(gameObject, 0.8f); // 0.8秒後に消失
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(boomSE); // 爆発SE再生

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
