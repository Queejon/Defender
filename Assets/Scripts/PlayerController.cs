using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private LevelLoader levelLoader;
    [SerializeField]
    private GameObject gameover;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private GameObject debris;
    [SerializeField]
    private GameObject belt;

    private PlayerStats stats;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float velocityCap;
    private Vector3 velocity;

    [SerializeField]
    private float fireDelay = .5f;

    [SerializeField]
    private bool hasSplit = false;
    [SerializeField]
    private float splitTime = 7.5f;
    private float nextSplitTime;
    // Spread of projectiles in degrees when multiple are fired
    [SerializeField]
    private float projectileSpread = 4f;
    // Number of projectiles to add when the multishot powerup is active
    [SerializeField]
    private int projectileAdditionalCount = 2;

    [SerializeField]
    private bool hasShield = false;
    [SerializeField]
    private float shieldTime = 7.5f;
    private float nextShieldTime;
    private GameObject shield;

    private float nextTime = 0f;

    void Start()
    {
        // Tracks Score
        stats = new PlayerStats(0, 0);
        // A vector (speed+an angle) applied over time to move the player
        velocity = new Vector3();
        shield = transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // "Acceleration"
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            Vector3 temp = (transform.rotation * (Vector3.up * speed * Time.deltaTime));
            if ((velocity.x + velocity.y + velocity.z + temp.x + temp.y + temp.z) / 2f <= velocityCap)
                velocity += temp;
        }
        // "Backward"
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            if (velocity.x + velocity.y + velocity.z <= velocityCap)
                velocity -= (transform.rotation * (Vector3.up * speed * Time.deltaTime));
        }

        // "Shoot"
        if (Input.GetButton("Fire1") && nextTime < Time.time)
        {
            Fire();
        }
        // "Airbrake"
        if (Input.GetButton("Jump"))
        {
            velocity = velocity - (velocity * .75f * Time.deltaTime);
            if (velocity.x + velocity.y + velocity.z < 10)
            {
                velocity = velocity - (velocity * .875f * Time.deltaTime);
            }
        }
        // REMOVED - Causes some sort of freezing bug, possibly due to the amount of calculations doubling the rate of acceleration induces
        // "Boost"
        /*if(Input.GetButtonDown("Fire2"))
        {
            speed *= 2;
        }
        if(Input.GetButtonUp("Fire2"))
        {
            speed /= 2;
        }*/
    }

    void FixedUpdate()
    {
        Vector3 move = velocity * Time.fixedDeltaTime;
        transform.position += move;
        if (gameObject.transform.position.y > 25)
        {
            gameObject.transform.position += Vector3.down * 50;
        }
        else if (gameObject.transform.position.y < -25)
        {
            gameObject.transform.position += Vector3.up * 50;
        }
        else if (gameObject.transform.position.x > 45)
        {
            gameObject.transform.position += Vector3.left * 90;
        }
        else if (gameObject.transform.position.x < -45)
        {
            gameObject.transform.position += Vector3.right * 90;
        }

        if (hasSplit && Time.time > nextSplitTime)
        {
            hasSplit = false;
        }
        if (hasShield && Time.time > nextShieldTime)
        {
            hasShield = false;
            shield.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag == "Asteroid")
        {
            if (!hasShield)
            {
                stats.lives--;
                if (lives > 0)
                {
                    ClearWorld();
                }
                else
                {
                    gameObject.SetActive(false);
                    Gameover();
                }
            }
            else
            {
                GameObject deb = Instantiate(debris);
                deb.transform.position = obj.transform.position;

                Destroy(obj);
            }
        }
    }

    void Fire()
    {
        nextTime = Time.time + fireDelay;
        int projectileCount = 1;
        if (hasSplit)
        {
            projectileCount += projectileAdditionalCount;
        }

        float degreeDif = -projectileSpread * (projectileCount / 2);

        GameObject obj;

        for (int i = 0; i < projectileCount; i++)
        {
            Quaternion newRot = transform.rotation * Quaternion.Euler(0f, 0f, degreeDif);
            obj = Instantiate(projectile, (transform.position + newRot * Vector3.up), newRot);

            degreeDif += projectileSpread;
        }
    }

    public PlayerController AsteroidHit(Asteroid ast)
    {
        stats.AsteroidHit(ast);
        return this;
    }

    public int score
    {
        get
        {
            return stats.score;
        }
    }

    public int lives
    {
        get
        {
            return stats.lives;
        }
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetVelocityTotal()
    {
        return velocity.x + velocity.y + velocity.z;
    }

    public float GetVelocityHypo()
    {
        return Mathf.Sqrt(Mathf.Pow(velocity.x, 2) + Mathf.Pow(velocity.y, 2));
    }

    public void ActivateSplit()
    {
        hasSplit = true;
        nextSplitTime = Time.time + splitTime;
    }

    public void ActivateShield()
    {
        hasShield = true;
        nextShieldTime = Time.time + shieldTime;
        shield.SetActive(true);
    }

    public void LifeUp()
    {
        stats.lives++;
    }

    // Move to some sort of world/game manager class
    private void ClearWorld()
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        foreach (GameObject go in asteroids)
        {
            Destroy(go);
        }

        transform.position = Vector3.zero;
        velocity = Vector3.zero;
    }

    private GameObject Gameover()
    {
        gameover.SetActive(true);
        GameObject.Find("End Score").transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "" + stats.score;
        Destroy(belt);
        ClearWorld();
        return gameObject;
    }
}
