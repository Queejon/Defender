using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private GameObject asteroid;
    [SerializeField]
    private GameObject player;
    [Space(20)]
    [SerializeField]
    private GameObject powSplit;
    [SerializeField]
    private float splitChance;
    [SerializeField]
    private GameObject powShield;
    [SerializeField]
    private float shieldChance;
    [SerializeField]
    private GameObject powLife;
    [SerializeField]
    private float lifeChance;

    private float totalChancePool;
    
    private float spawnTime;
    private float nextTime;
    private float startTime;

    private float nextSweep;
    [SerializeField]
    private float sweepInterval = 2f;
    [SerializeField]
    private int asteroidsPerPow = 5;

    public float spawnerMultiplier
    {
        get
        {
            return (3 / (1 + Mathf.Pow((float)System.Math.E, (-0.5f * (Time.time - startTime)/60 + 4))) + 1) ;
        }
    }
    
    private int _count;

    public int count
    {
        get { return _count; }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        spawnTime = 2f;
        startTime = Time.time;
        nextTime = Time.time + spawnTime / spawnerMultiplier;

        nextSweep = Time.time + sweepInterval;
        
        _count = 0;
        totalChancePool = splitChance + shieldChance + lifeChance;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextTime)
        {
            Gen();
        }

        if(Time.time > nextSweep)
        {
            Sweep();
        }
    }

    private void Gen()
    {
        nextTime = Time.time + spawnTime / spawnerMultiplier;
        Vector3 pos = GenPosition();
        Instantiate(asteroid, pos, Quaternion.identity, transform);
        _count++;
        if(_count % asteroidsPerPow == 0)
        {
            GenPower();
        }
    }

    private Vector3 GenPosition()
    {
        Vector3 result;
        do
        {
            result = new Vector3(Random.Range(player.transform.position.x - 25, player.transform.position.x + 25), Random.Range(player.transform.position.y - 45, player.transform.position.y + 45), 0);
        }
        while (Vector3.Distance(result, player.transform.position) < (.5f*player.GetComponent<PlayerController>().GetSpeed()*Mathf.Pow(1.5f,2)));
        return result;
    }

    private float GenRotation()
    {
        return Random.Range(0f,360f);
    }

    private void GenPower()
    {
        Vector3 pos = GenPositionPower();
        float selector = Random.Range(0, totalChancePool);
        GameObject selectedPow;

        if(selector < lifeChance)
        {
            selectedPow = powLife;
        }
        else if(selector < lifeChance + shieldChance)
        {
            selectedPow = powShield;
        }
        else
        {
            selectedPow = powSplit;
        }

        Instantiate(selectedPow, pos, Quaternion.identity);
    }

    private Vector3 GenPositionPower()
    {
        return new Vector3(Random.Range(-30, 30), Random.Range(-20, 20), 0);
    }

    private void Sweep()
    {
        foreach(Transform trans in transform)
        {
            GameObject obj = trans.gameObject;
            Asteroid ast = obj.GetComponent<Asteroid>();
            Collider2D rb = obj.GetComponent<Collider2D>();
            if (!ast.enabled || !rb.enabled) // Check if asteroid or collider component component are disabled
            {
                ast.enabled = true;
                rb.enabled = true;
            }
        }
    }
}
