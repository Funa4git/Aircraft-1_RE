using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryScript : MonoBehaviour
{
    // ボーダー外に出たオブジェクト（弾や敵など）を消す
    void OnTriggerExit2D(Collider2D collision)
    {
        Destroy(collision.gameObject);
    }
}
