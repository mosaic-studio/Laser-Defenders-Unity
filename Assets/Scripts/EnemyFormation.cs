using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFormation : MonoBehaviour {
    public GameObject laserShoot;
    public float health = 150f;
    public float laserShootSpeed = 10;
    public float firingRate = 0.2f;
    public float shootsPerSeconds = 0.5f;
	public int scoreValue = 150;
	public AudioClip shootSound;
	public AudioClip explosion;

	private ScoreKeeper score;

	private void Start()
	{
		score = GameObject.Find("Score").GetComponent<ScoreKeeper>();
	}

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
        // Vector3 startPosition = transform.position + new Vector3(0, -1, 0);
        GameObject shoot = Instantiate(laserShoot, transform.position, Quaternion.identity) as GameObject;
        shoot.GetComponent<Rigidbody2D>().velocity = new Vector3(0, -laserShootSpeed);
		AudioSource.PlayClipAtPoint(shootSound, transform.position);
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
				score.Score(scoreValue);
				Destroy(gameObject);
				AudioSource.PlayClipAtPoint(explosion, transform.position);
			}
        }
    }

}
