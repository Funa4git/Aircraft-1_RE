using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MissileAttackScript : MonoBehaviour
{
    // �ڕW�Ɍ������čU���i�e�A���˒n�_�A�^�[�Q�b�g�������Ƃ��Ďw��j
    // �K�����̊֐����Ăяo���čU������
    // �Ăяo���O�ɁAtargetPosition��null�łȂ����m�F���邱��
    public void targetAttack(GameObject missilePrefab, Vector3 firePoint)
    {
        Instantiate(missilePrefab, firePoint, Quaternion.identity);
    }

}
