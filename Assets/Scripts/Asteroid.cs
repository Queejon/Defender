using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject asteroidPrefab;
    
    private float velocity;

    private bool started = false;

    [SerializeField]
    private AsteroidSize _size;

    public AsteroidSize size
    {
        get { return _size; }
        set { _size = value; UpdateSize(); }
    }

    void Awake()
    {
    
    }
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        velocity = GenVelocity();
        Vector3 euler = transform.eulerAngles;
        euler.z = Random.Range(0f, 360f);
        transform.eulerAngles = euler;
        started = true;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += transform.up*velocity*Time.deltaTime;
    }

    private void FixedUpdate()
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

    private float GenVelocity()
    {
        return Random.Range(1,5);
    }

    private void UpdateSize()
    {
        switch (_size)
        {
            case AsteroidSize.small:
                transform.localScale = new Vector3(2f, 2f, 2f);
                break;
            case AsteroidSize.medium:
                transform.localScale = new Vector3(3f, 3f, 3f);
                break;
            default:
                transform.localScale = new Vector3(5f, 5f, 5f);
                break;
        }
    }

    public void Hit()
    {
        if (!started)
            return;
        GameObject go;
        Asteroid ast;
        switch (_size)
        {
            case AsteroidSize.large:
                // First Asteroid Instantiation
                go = Instantiate(asteroidPrefab, transform.parent);
                ast = go.GetComponent<Asteroid>();
                ast.size = AsteroidSize.medium;
                // Second Asteroid Instantiation
                go = Instantiate(asteroidPrefab, transform.parent);
                ast = go.GetComponent<Asteroid>();
                ast.size = AsteroidSize.medium;

                // Old Asteroid Removal
                Destroy(gameObject);
                break;
            case AsteroidSize.medium:
                // First Asteroid Instantiation
                go = Instantiate(asteroidPrefab, transform.parent);
                ast = go.GetComponent<Asteroid>();
                ast.size = AsteroidSize.small;
                // Second Asteroid Instantiation
                go = Instantiate(asteroidPrefab, transform.parent);
                ast = go.GetComponent<Asteroid>();
                ast.size = AsteroidSize.small;

                // Old Asteroid Removal
                Destroy(gameObject);
                break;
            default:
                Destroy(gameObject);
                break;
        }
    }
    // Removed due to an odd behavior of asteroids sticking together after splitting until one of them collides with a new asteroid
    /*
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if(obj.tag == "Asteroid" && started)
        {
            velocity *= -1;
        }
    }
    */
}

public enum AsteroidSize
{
    large,
    medium,
    small
}
