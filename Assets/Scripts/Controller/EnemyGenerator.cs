using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    // 敵のスポーン制御スクリプト, クリア条件制御

    // gameControllerスクリプトを格納する変数
    private GameControllerScript gameController;
    // ステージごとに敵ウェーブのprefabである wave を格納する配列
    public GameObject[] waves;
    // 現在のweve数を格納
    private int currentWave;


    IEnumerator Start()
    {
        // GameControllerScriptのGameClear()関数を呼び出したい
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameControllerScript>();

        // 配列が空であれば何もしない(Waveが存在しなければ)
        if (waves.Length == 0)
        {
            yield break;
        }

        while (true)
        {
            // 配列に格納されたプレハブ wave からインスタンスを作成して waveに格納
            GameObject wave = (GameObject)Instantiate(waves[currentWave], transform.position, Quaternion.identity);

            // 敵 wave を enemyGenetator の子要素にして、位置をenemyGenetatorの位置へ
            wave.transform.parent = transform;

            // enemyGeneratorの子要素の敵機体が0になるまで処理を待機
            while (wave.transform.childCount != 0)
            {
                // 削除されるまで待機(レンダリング終了まで)
                yield return new WaitForEndOfFrame(); // WaitForEndOfFrame()はUnityEngineの中
                
            }

            // 現在のwave削除
            Destroy(wave);

            // 最終waveの敵を倒すとゲームクリア、currentWaveを更新
            if (waves.Length <= currentWave + 1)
            {
                gameController.GameClear();
                break;
            } else
            {
                currentWave = currentWave +1;
            }
        }
    }

    // void Start()
    // {
    //     // // 繰り返し関数を実行、("実行する関数", ディレイ, 間隔)
    //     // InvokeRepeating("Spawn", 2f, 0.5f);
    //     // InvokeRepeating("SpawnDisk1", 2f, 2f);
    //     // Invoke("BossSpawn", 4f); /*他のInvokeをキャンセルする*/
    //     // SpawnFighter();

        
    // }

// // 各種敵の生成
//     void Spawn()
//     {
//         Vector3 spawnPosition = new Vector3(
//             Random.Range(-2.5f, 2.5f),
//             transform.position.y,
//             transform.position.z
//         );
//         Instantiate(enemyPrefab, spawnPosition, transform.rotation);
//     }
//     void SpawnDisk1()
//     {
//         Vector3 spawnPosition = new Vector3(
//             Random.Range(-2.5f, 2.5f),
//             transform.position.y,
//             transform.position.z
//         );
//         Instantiate(enemyDisk1, spawnPosition, transform.rotation);
//     }

//     void SpawnTransporter()
//     {
//         Vector3 spawnposition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z );
//         Instantiate(Transporter, spawnposition, transform.rotation);
//     }

//     void SpawnFighter()
//     {
//         Vector3 spawnposition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z );
//         Instantiate(Fighter, spawnposition, transform.rotation);
//     }
//     // Update is called once per frame
//     void Update()
//     {
        
//     }

}