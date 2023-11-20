using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetrapod : MonoBehaviour
{
    
    [SerializeField] private float averageSpeed = 0.4f;
    [SerializeField] private float period = 2;
    
    private float phase;
    // Start is called before the first frame update
    void Start()
    {
        phase = Random.Range(0f, 360f);
        transform.Rotate(0f, 0f, phase);
        averageSpeed += Random.Range(-0.15f, 0.15f);
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.position += new Vector3(
            0,
            -averageSpeed,
            0f
        ) * Time.deltaTime;

        transform.Rotate(
            0f,
            0f,
            360 / period * Time.deltaTime
        );
    }
}
