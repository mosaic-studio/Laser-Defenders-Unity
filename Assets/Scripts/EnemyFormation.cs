using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFormation : MonoBehaviour {
    public float health = 150f;
    public float laserShootSpeed = 10;
    public float firingRate = 0.2f;
    public GameObject laserShoot;
    public float shootsPerSeconds = 0.5f;

    void Update()
    {
        float probability = Time.deltaTime * shootsPerSeconds;
        if(Random.value < probability)
        {
            Fire();
        }
    }

    void Fire()
    {
        Vector3 startPosition = transform.position + new Vector3(0, -1, 0);
        GameObject shoot = Instantiate(laserShoot, startPosition, Quaternion.identity) as GameObject;
        shoot.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -laserShootSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LaserShoot laser = collision.gameObject.GetComponent<LaserShoot>();
        if (laser)
        {
            health -= laser.getDamage();
            laser.Hit();
            if (health <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

}
