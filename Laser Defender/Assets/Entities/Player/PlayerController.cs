using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 1.0f;
    public float padding = 1f;
    public GameObject laser;
    public float projectileSpeed;
    public float firingRate;
    public float health = 250;
 

    private LevelManager levelManager;

    float xmin = -5;
    float xmax = 5;

	// Use this for initialization
	void Start () {
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xmin = leftmost.x + padding;
        xmax = rightmost.x - padding;
    }
	
    void Fire()
    {
        Vector3 startPos = transform.position + new Vector3(0, 1f, 0);
        GameObject beam = Instantiate(laser, startPos, Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, projectileSpeed);
    }

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.A)){
            //this.transform.position -= (new Vector3(1f, 0f,0f) * moveSpeed * Time.deltaTime);
            transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.D))
        {
            //this.transform.position += (new Vector3(1f, 0f, 0f) * moveSpeed * Time.deltaTime);
            transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        } else if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            InvokeRepeating("Fire",0.0000001f, (60/firingRate)/60);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke();
        }

        float newX = Mathf.Clamp(transform.position.x, xmin, xmax);

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Projectile missile = collision.gameObject.GetComponent<Projectile>();
        if (missile)
        {
            health -= missile.GetDamage();
            missile.Hit();
            if (health <= 0)
            {
                Die();
            }
            Debug.Log("Enemy hit");
        }
    }

    private void Die()
    {
        GameObject.Find("LevelManager").GetComponent<LevelManager>().LoadLevel("Win Screen");
        Destroy(gameObject);
    }
}
