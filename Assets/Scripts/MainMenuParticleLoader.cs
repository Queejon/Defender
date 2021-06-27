using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuParticleLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject particle;
    
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            Instantiate(particle);
        }
    }
}
