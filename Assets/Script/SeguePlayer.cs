using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguePlayer : MonoBehaviour
{
    public Movimentacao player;
    public GameObject segue;
    public float camVel = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Movimentacao>();
        segue = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = segue.transform.position + new Vector3(0, 0, -15);
    }
}
