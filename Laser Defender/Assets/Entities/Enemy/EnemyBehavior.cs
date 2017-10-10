using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {
    public float health = 150;
    public GameObject laser;
    public float projectileSpeed = 1f;
    public float shotsPerSecond = .5f;
    public int scoreValue = 150;
    public AudioClip enemyDeath;

    private ScoreKeeper keeper;

    //private float timeTracker = 0;

    private void Start()
    {
        keeper = GameObject.Find("Score").GetComponent<ScoreKeeper>();
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
                AudioSource.PlayClipAtPoint(enemyDeath, transform.position);
                Destroy(gameObject);
                keeper.Score(scoreValue);                
            }
            Debug.Log("Enemy hit");
        }
    }

    private void Update()
    {
        float probability = shotsPerSecond * Time.deltaTime;
        if(Random.value < probability)
        {
            Fire();
        }        
    }

    void Fire()
    {
        Vector3 startPos = transform.position + new Vector3(0, -1f, 0);
        GameObject beam = Instantiate(laser, startPos, Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);        
    }
}
