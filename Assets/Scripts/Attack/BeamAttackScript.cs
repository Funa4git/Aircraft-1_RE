using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class BeamAttackScript : MonoBehaviour
{
    // 目標に向かって攻撃（弾、発射地点、ターゲットを引数として指定）
    // 必ずこの関数を呼び出して攻撃する
    // 呼び出し前に、targetPositionがnullでないか確認すること
    public void targetAttack(GameObject bulletPrefab, Vector3 firePoint, Vector3 targetPosition, float speed = 20.0f)
    {
        if (targetPosition != null)
        {
            // 弾の生成
            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint, Quaternion.identity);
            // スクリプトの取得
            BeamBulletScript bullet = bulletInstance.GetComponent<BeamBulletScript>();
            // 初期化
            bullet.Initialize(targetPosition, speed);
        } else
        {
            return;
        }
    }





    // 連撃でビームのように攻撃
    public IEnumerator continuousAttack(GameObject bulletPrefab, Vector3 firePoint, Vector3 targetPosition)
    {
        for (float i = 0; i < 12; i++)
        {
            // targetAttack関数を呼び出して攻撃
            targetAttack(bulletPrefab, firePoint, targetPosition, 45.0f);
            // 一定時間待機
            yield return new WaitForSeconds(0.05f);
        }
    }

    // 攻撃を記述
    // 左上から下へ波状攻撃
    public IEnumerator LeftAttack(GameObject bulletPrefab, Vector3 firePoint)
    {
        for(float angle = 270; angle <= 360; angle += 10)
        {
            // 角度からベクトルを計算
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack関数を呼び出して攻撃
            targetAttack(bulletPrefab, firePoint, targetPosition);
            // 一定時間待機
            yield return new WaitForSeconds(0.25f);
        }
    }

    // 右上から下へ波状攻撃
    public IEnumerator RightAttack(GameObject bulletPrefab, Vector3 firePoint)
    {
        for (float angle = 90; angle >= 0; angle -= 10)
        {
            // 角度からベクトルを計算
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack関数を呼び出して攻撃
            targetAttack(bulletPrefab, firePoint, targetPosition);
            // 一定時間待機
            yield return new WaitForSeconds(0.25f);
        }
    }




    private float beamHML_AngleWidth = 6.0f; // 角度のずれ幅
    private float beamHML_Speed = 30.0f; // 弾の速度

    // 上段への攻撃
    public void beamHigh(GameObject bulletPrefab, Vector3 firePoint)
    {
        for (float angle = 300; angle >= 270; angle -= beamHML_AngleWidth)
        {
            // 角度からベクトルを計算
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack関数を呼び出して攻撃
            targetAttack(bulletPrefab, firePoint, targetPosition, beamHML_Speed);
        }
        for (float angle = 60; angle <= 90; angle += beamHML_AngleWidth)
        {
            // 角度からベクトルを計算
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack関数を呼び出して攻撃
            targetAttack(bulletPrefab, firePoint, targetPosition, beamHML_Speed);
        }
    }

    // 中段への攻撃
    public void beamMid(GameObject bulletPrefab, Vector3 firePoint)
    {
        for (float angle = 330; angle >= 300; angle -= beamHML_AngleWidth)
        {
            // 角度からベクトルを計算
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack関数を呼び出して攻撃
            targetAttack(bulletPrefab, firePoint, targetPosition, beamHML_Speed);
        }
        for (float angle = 30; angle <= 60; angle += beamHML_AngleWidth)
        {
            // 角度からベクトルを計算
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack関数を呼び出して攻撃
            targetAttack(bulletPrefab, firePoint, targetPosition, beamHML_Speed);
        }
    }

    // 下段への攻撃
    public void beamLow(GameObject bulletPrefab, Vector3 firePoint)
    {
        for (float angle = 360; angle >= 330; angle -= beamHML_AngleWidth)
        {
            // 角度からベクトルを計算
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack関数を呼び出して攻撃
            targetAttack(bulletPrefab, firePoint, targetPosition, beamHML_Speed);
        }
        for (float angle = 0; angle <= 30; angle += beamHML_AngleWidth)
        {
            // 角度からベクトルを計算
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack関数を呼び出して攻撃
            targetAttack(bulletPrefab, firePoint, targetPosition, beamHML_Speed);
        }
    }



    private float allAttack_AngleWidth = 90.0f / 4f; // 角度のずれ幅

    // 全体攻撃１
    public void AllAttack_1(GameObject bulletPrefab, Vector3 firePoint)
    {
        for (float angle = 270 + (allAttack_AngleWidth * 1 / 4); angle <= 360; angle += allAttack_AngleWidth)
        {
            // 角度からベクトルを計算
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack関数を呼び出して攻撃
            targetAttack(bulletPrefab, firePoint, targetPosition);
        }
        for (float angle = 90 + (allAttack_AngleWidth * 1 / 4); angle >= 0; angle -= allAttack_AngleWidth)
        {
            // 角度からベクトルを計算
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack関数を呼び出して攻撃
            targetAttack(bulletPrefab, firePoint, targetPosition);
        }
    }

    // 全体攻撃２
    public void AllAttack_2(GameObject bulletPrefab, Vector3 firePoint)
    {

        for (float angle = 270 + (allAttack_AngleWidth * -1 / 4); angle <= 360; angle += allAttack_AngleWidth)
        {
            // 角度からベクトルを計算
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack関数を呼び出して攻撃
            targetAttack(bulletPrefab, firePoint, targetPosition);
        }
        for (float angle = 90 + (allAttack_AngleWidth * -1 / 4); angle >= 0; angle -= allAttack_AngleWidth)
        {
            // 角度からベクトルを計算
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack関数を呼び出して攻撃
            targetAttack(bulletPrefab, firePoint, targetPosition);
        }
    }


    // ランダム攻撃
    public void RandomAttack(GameObject bulletPrefab, Vector3 firePoint)
    {
        float angle = Random.Range(0.0f, 180.0f);
        // 0～90度、270～360度の間にする
        if (angle > 90.0f)
        {
            angle += 180.0f;
        }
        // 角度からベクトルを計算
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector3 directionFromOrigin = rotation * Vector3.down;
        Vector3 targetPosition = firePoint + directionFromOrigin;

        targetAttack(bulletPrefab, firePoint, targetPosition, 20.0f);
    }

}
