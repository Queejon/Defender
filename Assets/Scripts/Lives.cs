using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lives : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private TMP_Text text;

    private int lives;

    // Start is called before the first frame update
    void Start()
    {
        lives = player.lives;
    }

    // Update is called once per frame
    void Update()
    {
        lives = player.lives;
        text.text = "" + lives;
    }
}
