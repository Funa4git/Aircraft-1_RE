using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// ’Pƒ‚È’e‚ÌˆÚ“®§ŒäƒR[ƒh
public class BeamBulletScript : MonoBehaviour
{
    private float speed = 0.0f; // ’e‘¬
    private float lifeTimeSec = 7.0f; // ’e‚Ì¶‘¶ŠÔ
    private Vector3 targetPosition; // ’e‚Ì–Ú•W’n“_
    private Vector3 moveVector; // ˆÚ“®•ûŒü


    // Init
    public void Initialize(Vector3 targetPosition, float speed)
    {
        this.targetPosition = targetPosition;   // ’e‚Ìis•ûŒü‚ğİ’è

        // ˆÚ“®•ûŒü‚ğİ’è
        moveVector = targetPosition - transform.position;
        moveVector.Normalize(); // ³‹K‰»
        // ’e‘¬‚ğİ’è
        this.speed = speed;

    }

    // Update is called once per frame
    void Update()
    {
        if(lifeTimeSec <= 0)
        {
            Destroy(gameObject); // ’e‚Ìíœ
        }
        lifeTimeSec -= Time.deltaTime; // ’e‚Ì¶‘¶ŠÔ‚ğŒ¸‚ç‚·


        transform.position += moveVector * speed * 0.1f * Time.deltaTime; // ˆÚ“®
    }
}
