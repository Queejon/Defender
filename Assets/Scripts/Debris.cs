using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour
{
    private float timeUp;
    private float lifetime = 2f;

    // Start is called before the first frame update
    void Start()
    {
        timeUp = Time.time + lifetime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeUp)
            Destroy(gameObject);
    }
}
