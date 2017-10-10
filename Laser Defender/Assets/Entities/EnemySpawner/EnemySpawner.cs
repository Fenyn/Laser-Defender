using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyPrefab;
    public float height = 5f;
    public float width = 10f;
    public float speed = 1f;
    public float spawnDelay = 0.5f;
   

    private bool isPos = true;
    private float xmin = -5f;
    private float xmax = 5f;



	// Use this for initialization
	void Start ()
    {
        SpawnUntilFull();
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        xmin = leftmost.x;
        xmax = rightmost.x;
    }

    //private void SpawnEnemies()
    //{
    //    foreach (Transform child in transform)
    //    {
    //        GameObject enemy = Instantiate(enemyPrefab, child.position, Quaternion.identity) as GameObject;
    //        enemy.transform.parent = child;
    //    }        
    //}

    void SpawnUntilFull()
    {
        Transform freePosition = NextFreePosition();
        if (freePosition)
        {
            GameObject enemy = Instantiate(enemyPrefab, freePosition.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePosition;
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height));
    }

    // Update is called once per frame
    void Update () {
       
        if (isPos)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
            BorderCheck();
        } else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
            BorderCheck();
        }

        if (AllMembersDead())
        {
            StartCoroutine("SpawnEnemiesDelayed");
        }

	}

    void BorderCheck()
    {
        double rightEdgeOfFormation = transform.position.x + .5f * width;
        double leftEdgeOfFormation = transform.position.x - .5f * width;

        if (leftEdgeOfFormation <= xmin)
        {
            isPos = true;
        } else if (rightEdgeOfFormation >= xmax)
        {
            isPos = false;
        }
    }

    bool AllMembersDead()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount > 0)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnEnemiesDelayed()
    {
        yield return new WaitForSeconds(5);
        SpawnUntilFull();
        StopCoroutine("SpawnEnemiesDelayed");
    }

    Transform NextFreePosition()
    {
        foreach (Transform childPositionGameObject in transform)
        {
            if (childPositionGameObject.childCount == 0)
            {
                return childPositionGameObject;
            }
        }
        return null;
    }
    
}
