using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float maxSpeed = 5.0f;
    public float padding = 1f;
    public float laserShootSpeed;
    public float firingRate = 0.2f;
    public float health = 300f;
	public AudioClip shootSound;

	public GameObject laserShoot;

    float xmin;
    float xmax;

    // Use this for initialization
    void Start () {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xmin = leftMost.x + padding;
        xmax = rightMost.x - padding;
    }
	
	// Update is called once per frame
	void Update () {
        Move();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Fire", 0.000001f, firingRate);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Fire");
        }
	}

    void Move()
    {
        Vector3 velocity = new Vector3(Input.GetAxis("Horizontal") * maxSpeed * Time.deltaTime, 0, 0);
        transform.position += velocity;

        float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    void Fire()
    {
        Vector3 startPosition = transform.position + new Vector3(0, 1, 0);
        GameObject shoot = Instantiate(laserShoot, startPosition, Quaternion.identity) as GameObject;
        shoot.GetComponent<Rigidbody2D>().velocity = new Vector3(0, laserShootSpeed);
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
				Die();
            }
        }
    }

	private void Die()
	{
		Destroy(gameObject);
		LevelManager man = GameObject.Find("LevelManager").GetComponent<LevelManager>();
		man.LoadLevel("Win Screen");
	}
}
