using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCanv : MonoBehaviour
{
    void Awake()
    {
        if(GameManager.Instance.canvas ==  null)
        {
            //GameManager.Instance.canvas = this;
            DontDestroyOnLoad(gameObject);
        }
        else if(GameManager.Instance.canvas != this)
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
