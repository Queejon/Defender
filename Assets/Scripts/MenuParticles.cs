using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuParticles : MonoBehaviour
{
    [SerializeField]
    private float speed;

    void Start()
    {
        Quaternion newRotation = Random.rotation;
        newRotation.Set(0, 0, newRotation.z, 1);

        Vector3 newPosition = new Vector3(Random.Range(-5, 5), Random.Range(-9, 9), 1);

        transform.rotation = newRotation;
        transform.position = newPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + transform.rotation * (speed * Time.deltaTime * Vector3.up);
    }

    private void FixedUpdate()
    {
        if(gameObject.transform.position.y > 5)
        {
            gameObject.transform.position += Vector3.down * 10;
        }
        else if(gameObject.transform.position.y < -5)
        {
            gameObject.transform.position += Vector3.up * 10;
        }
        else if(gameObject.transform.position.x > 9)
        {
            gameObject.transform.position += Vector3.left * 18;
        }
        else if(gameObject.transform.position.x < -9)
        {
            gameObject.transform.position += Vector3.right * 18;
        }
    }
}