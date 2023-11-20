using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class EnemyMegalodonScript : MonoBehaviour
{
    // ステージ6ボス(Megalodon)
    public Transform fireRight;
    public Transform fireLeft;
    public GameObject megaloMissile;

    public float shotDelay;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Shoot");

    }

    // Update is called once per frame
    IEnumerator Shoot()
    {
        while (transform.position.y > 3.0f)
        {
            transform.position -= new Vector3(0, 3.0f, 0) * Time.deltaTime;
            yield return null;
        }

        while (GameObject.Find("Player") != null)
        {
            Instantiate(megaloMissile, fireRight.position, transform.rotation);
            Instantiate(megaloMissile, fireLeft.position, transform.rotation);
            yield return new WaitForSeconds(0.5f);
            Instantiate(megaloMissile, fireRight.position, transform.rotation);
            Instantiate(megaloMissile, fireLeft.position, transform.rotation);
            yield return new WaitForSeconds(0.5f);
            Instantiate(megaloMissile, fireRight.position, transform.rotation);
            Instantiate(megaloMissile, fireLeft.position, transform.rotation);
            yield return new WaitForSeconds(0.5f);
            // audioSource.PlayOneShot(shotSE);
            yield return new WaitForSeconds(shotDelay);
        }

    }
}
