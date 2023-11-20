using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// �P���Ȓe�̈ړ�����R�[�h
public class BeamBulletScript : MonoBehaviour
{
    private float speed = 0.0f; // �e��
    private float lifeTimeSec = 7.0f; // �e�̐�������
    private Vector3 targetPosition; // �e�̖ڕW�n�_
    private Vector3 moveVector; // �ړ�����


    // Init
    public void Initialize(Vector3 targetPosition, float speed)
    {
        this.targetPosition = targetPosition;   // �e�̐i�s������ݒ�

        // �ړ�������ݒ�
        moveVector = targetPosition - transform.position;
        moveVector.Normalize(); // ���K��
        // �e����ݒ�
        this.speed = speed;

    }

    // Update is called once per frame
    void Update()
    {
        if(lifeTimeSec <= 0)
        {
            Destroy(gameObject); // �e�̍폜
        }
        lifeTimeSec -= Time.deltaTime; // �e�̐������Ԃ����炷


        transform.position += moveVector * speed * 0.1f * Time.deltaTime; // �ړ�
    }
}
