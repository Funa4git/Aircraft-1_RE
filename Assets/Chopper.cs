using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chopper : MonoBehaviour
{
    /*
    x軸からのなす角moveDirection方向に移動
    mineInterval秒おきにmineを落とす
    */
    [SerializeField] private float speed = 1f;
    [SerializeField] private float moveDirection = 160;

    [SerializeField] private GameObject mine;
    [SerializeField] float mineInterval = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(UpdateCoroutine());
    }

    // Update is called once per frame
    private IEnumerator UpdateCoroutine()
    {
        
        while (transform.position.y > 2.8)
        {
            transform.position += new Vector3(0, -speed, 0) * Time.deltaTime;
            yield return null;
        }
        transform.Rotate(
            0f,
            0f,
            moveDirection + 90
        );
        StartCoroutine(geneMine());
        while(true){
            transform.position += new Vector3(
                speed * Mathf.Cos(moveDirection / 180 * Mathf.PI),
                speed * Mathf.Sin(moveDirection / 180 * Mathf.PI),
                0f
            ) * Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator geneMine()
    {
        for(int i = 0; i < 3; i++){
            yield return new WaitForSeconds(mineInterval);
            Instantiate(mine, transform.position, Quaternion.identity);
        }
        yield return null;
    }
}
