using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MissileAttackScript : MonoBehaviour
{
    // 目標に向かって攻撃（弾、発射地点、ターゲットを引数として指定）
    // 必ずこの関数を呼び出して攻撃する
    // 呼び出し前に、targetPositionがnullでないか確認すること
    public void targetAttack(GameObject missilePrefab, Vector3 firePoint)
    {
        Instantiate(missilePrefab, firePoint, Quaternion.identity);
    }

}
