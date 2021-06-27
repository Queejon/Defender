using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float lifetime;
    private float nextTime;

    [SerializeField]
    private PowerupType type;

    // Start is called before the first frame update
    void Start()
    {
        nextTime = Time.time + lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextTime)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            switch (type)
            {
                case PowerupType.split:
                    player.ActivateSplit();
                    break;
                case PowerupType.shield:
                    player.ActivateShield();
                    break;
                case PowerupType.life:
                    player.LifeUp();
                    break;
            }

            Destroy(gameObject);
        }
    }
}

enum PowerupType
{
    split,
    shield,
    life
}
