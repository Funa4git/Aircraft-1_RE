using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swallow : MonoBehaviour
{
    
    [SerializeField] private float speed = 0.8f;
    // [SerializeField] private float period = 2;
    
    // private float phase;
    [SerializeField] private Vector3 _center = Vector3.zero; // 中心座標
    [SerializeField] private Vector3 _axis = Vector3.back; // 回転軸
    private float _period; // 回転周期

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateCoroutine());
        // 円弧運動の早さをspeedに合わせる
        float r = Mathf.Abs(transform.position.x - _center.x);
        _period = 2 * Mathf.PI * r / speed;
    }

    // Update is called once per frame
    private IEnumerator UpdateCoroutine()
    {
        
        while (transform.position.y > _center.y)
        {
            transform.position += new Vector3(0, -speed, 0) * Time.deltaTime;
            yield return null;
        }
        while(true){
            transform.RotateAround(
                _center,
                _axis,
                360 / _period * Time.deltaTime
            );
            yield return null;
        }
    }
}
