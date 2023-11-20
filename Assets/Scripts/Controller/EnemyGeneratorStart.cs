using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneratorStart : MonoBehaviour
{
    // スタート待機画面(idleScene)内の敵のスポーンを制御
    private GameControllerScript gameController;

    // 敵機のprefab waveStart を格納する配列
    public GameObject[] wavesStart;
    // 現在のweveStart数を格納
    private int currentWaveStart;

    IEnumerator Start()
    {
        // 配列 waveStart が空であれば何もしない
        if (wavesStart.Length == 0)
        {
            yield break;
        }

        while (true)
        {
            
            // 配列に格納されたプレハブ waveStart からインスタンスを作成して waveに格納
            GameObject wave = (GameObject)Instantiate(wavesStart[currentWaveStart], transform.position, Quaternion.identity);
            // 敵機 wave を GameController の子要素にして、位置をGameControllerの位置へ
            wave.transform.parent = transform;
            // GameControllerの子要素の敵機 waveの数が0でなければ
            while (wave.transform.childCount != 0)
            {
                // 削除されるまで待機
                yield return new WaitForEndOfFrame();
                
            }

            Destroy(wave);

            if (wavesStart.Length <= ++currentWaveStart)
            {
                currentWaveStart = 0;
            }
        }
    }
}