using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private GameObject hitParticles;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float halflife;
    
    private float time;
    
    // Start is called before the first frame update
    void Start()
    {
        time = Time.time+halflife;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time < time)
        {
            transform.position = transform.position + speed*Time.deltaTime * transform.up;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if(gameObject.transform.position.y > 25)
        {
            gameObject.transform.position += Vector3.down*50;
        }
        else if(gameObject.transform.position.y < -25)
        {
            gameObject.transform.position += Vector3.up*50;
        }
        else if(gameObject.transform.position.x > 45)
        {
            gameObject.transform.position += Vector3.left*90;
        }
        else if(gameObject.transform.position.x < -45)
        {
            gameObject.transform.position += Vector3.right*90;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if(obj.tag == "Asteroid")
        {
            // Destroy Asteroid
            obj.GetComponent<Asteroid>().Hit();
            // Start Effects
            GameObject deb = Instantiate(hitParticles);
            deb.transform.position = transform.position;
            // Add Points
            GameObject.FindGameObjectsWithTag("Player")[0].GetComponent<PlayerController>().AsteroidHit(obj.GetComponent<Asteroid>());
            // Destroy Self
            Destroy(gameObject);
        }
    }
}
