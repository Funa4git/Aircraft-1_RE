using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy4Script : MonoBehaviour
{
    // ステージ1ボス(Enemy4)
    public Transform firePoint1;
    public Transform firePoint2;
    public GameObject missile;

    public float shotDelay;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("Shoot");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Shoot()
    {
        while(transform.position.y > 1.75f)
        {
            transform.position -= new Vector3(0, 1.5f, 0)*Time.deltaTime;
            yield return null;
        }

        while (GameObject.Find("Player") != null)
        {
            Instantiate(missile, firePoint1.position, transform.rotation);
            Instantiate(missile, firePoint2.position, transform.rotation);
            // audioSource.PlayOneShot(shotSE);
            yield return new WaitForSeconds(shotDelay);
        }
    }
}
