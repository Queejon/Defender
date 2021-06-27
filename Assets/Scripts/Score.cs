using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private TMP_Text text;

    private int score;

    // Start is called before the first frame update
    void Start()
    {
        score = player.score;
    }

    // Update is called once per frame
    void Update()
    {
        score = player.score;
        text.text = "" + score;
    }
}