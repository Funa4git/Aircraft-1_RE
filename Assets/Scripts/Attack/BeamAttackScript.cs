using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class BeamAttackScript : MonoBehaviour
{
    // �ڕW�Ɍ������čU���i�e�A���˒n�_�A�^�[�Q�b�g�������Ƃ��Ďw��j
    // �K�����̊֐����Ăяo���čU������
    // �Ăяo���O�ɁAtargetPosition��null�łȂ����m�F���邱��
    public void targetAttack(GameObject bulletPrefab, Vector3 firePoint, Vector3 targetPosition, float speed = 20.0f)
    {
        if (targetPosition != null)
        {
            // �e�̐���
            GameObject bulletInstance = Instantiate(bulletPrefab, firePoint, Quaternion.identity);
            // �X�N���v�g�̎擾
            BeamBulletScript bullet = bulletInstance.GetComponent<BeamBulletScript>();
            // ������
            bullet.Initialize(targetPosition, speed);
        } else
        {
            return;
        }
    }





    // �A���Ńr�[���̂悤�ɍU��
    public IEnumerator continuousAttack(GameObject bulletPrefab, Vector3 firePoint, Vector3 targetPosition)
    {
        for (float i = 0; i < 12; i++)
        {
            // targetAttack�֐����Ăяo���čU��
            targetAttack(bulletPrefab, firePoint, targetPosition, 45.0f);
            // ��莞�ԑҋ@
            yield return new WaitForSeconds(0.05f);
        }
    }

    // �U�����L�q
    // ���ォ�牺�֔g��U��
    public IEnumerator LeftAttack(GameObject bulletPrefab, Vector3 firePoint)
    {
        for(float angle = 270; angle <= 360; angle += 10)
        {
            // �p�x����x�N�g�����v�Z
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack�֐����Ăяo���čU��
            targetAttack(bulletPrefab, firePoint, targetPosition);
            // ��莞�ԑҋ@
            yield return new WaitForSeconds(0.25f);
        }
    }

    // �E�ォ�牺�֔g��U��
    public IEnumerator RightAttack(GameObject bulletPrefab, Vector3 firePoint)
    {
        for (float angle = 90; angle >= 0; angle -= 10)
        {
            // �p�x����x�N�g�����v�Z
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack�֐����Ăяo���čU��
            targetAttack(bulletPrefab, firePoint, targetPosition);
            // ��莞�ԑҋ@
            yield return new WaitForSeconds(0.25f);
        }
    }




    private float beamHML_AngleWidth = 6.0f; // �p�x�̂��ꕝ
    private float beamHML_Speed = 30.0f; // �e�̑��x

    // ��i�ւ̍U��
    public void beamHigh(GameObject bulletPrefab, Vector3 firePoint)
    {
        for (float angle = 300; angle >= 270; angle -= beamHML_AngleWidth)
        {
            // �p�x����x�N�g�����v�Z
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack�֐����Ăяo���čU��
            targetAttack(bulletPrefab, firePoint, targetPosition, beamHML_Speed);
        }
        for (float angle = 60; angle <= 90; angle += beamHML_AngleWidth)
        {
            // �p�x����x�N�g�����v�Z
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack�֐����Ăяo���čU��
            targetAttack(bulletPrefab, firePoint, targetPosition, beamHML_Speed);
        }
    }

    // ���i�ւ̍U��
    public void beamMid(GameObject bulletPrefab, Vector3 firePoint)
    {
        for (float angle = 330; angle >= 300; angle -= beamHML_AngleWidth)
        {
            // �p�x����x�N�g�����v�Z
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack�֐����Ăяo���čU��
            targetAttack(bulletPrefab, firePoint, targetPosition, beamHML_Speed);
        }
        for (float angle = 30; angle <= 60; angle += beamHML_AngleWidth)
        {
            // �p�x����x�N�g�����v�Z
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack�֐����Ăяo���čU��
            targetAttack(bulletPrefab, firePoint, targetPosition, beamHML_Speed);
        }
    }

    // ���i�ւ̍U��
    public void beamLow(GameObject bulletPrefab, Vector3 firePoint)
    {
        for (float angle = 360; angle >= 330; angle -= beamHML_AngleWidth)
        {
            // �p�x����x�N�g�����v�Z
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack�֐����Ăяo���čU��
            targetAttack(bulletPrefab, firePoint, targetPosition, beamHML_Speed);
        }
        for (float angle = 0; angle <= 30; angle += beamHML_AngleWidth)
        {
            // �p�x����x�N�g�����v�Z
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack�֐����Ăяo���čU��
            targetAttack(bulletPrefab, firePoint, targetPosition, beamHML_Speed);
        }
    }



    private float allAttack_AngleWidth = 90.0f / 4f; // �p�x�̂��ꕝ

    // �S�̍U���P
    public void AllAttack_1(GameObject bulletPrefab, Vector3 firePoint)
    {
        for (float angle = 270 + (allAttack_AngleWidth * 1 / 4); angle <= 360; angle += allAttack_AngleWidth)
        {
            // �p�x����x�N�g�����v�Z
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack�֐����Ăяo���čU��
            targetAttack(bulletPrefab, firePoint, targetPosition);
        }
        for (float angle = 90 + (allAttack_AngleWidth * 1 / 4); angle >= 0; angle -= allAttack_AngleWidth)
        {
            // �p�x����x�N�g�����v�Z
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack�֐����Ăяo���čU��
            targetAttack(bulletPrefab, firePoint, targetPosition);
        }
    }

    // �S�̍U���Q
    public void AllAttack_2(GameObject bulletPrefab, Vector3 firePoint)
    {

        for (float angle = 270 + (allAttack_AngleWidth * -1 / 4); angle <= 360; angle += allAttack_AngleWidth)
        {
            // �p�x����x�N�g�����v�Z
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack�֐����Ăяo���čU��
            targetAttack(bulletPrefab, firePoint, targetPosition);
        }
        for (float angle = 90 + (allAttack_AngleWidth * -1 / 4); angle >= 0; angle -= allAttack_AngleWidth)
        {
            // �p�x����x�N�g�����v�Z
            Quaternion rotation = Quaternion.Euler(0, 0, angle);
            Vector3 directionFromOrigin = rotation * Vector3.down;
            Vector3 targetPosition = firePoint + directionFromOrigin;
            // targetAttack�֐����Ăяo���čU��
            targetAttack(bulletPrefab, firePoint, targetPosition);
        }
    }


    // �����_���U��
    public void RandomAttack(GameObject bulletPrefab, Vector3 firePoint)
    {
        float angle = Random.Range(0.0f, 180.0f);
        // 0�`90�x�A270�`360�x�̊Ԃɂ���
        if (angle > 90.0f)
        {
            angle += 180.0f;
        }
        // �p�x����x�N�g�����v�Z
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        Vector3 directionFromOrigin = rotation * Vector3.down;
        Vector3 targetPosition = firePoint + directionFromOrigin;

        targetAttack(bulletPrefab, firePoint, targetPosition, 20.0f);
    }

}
