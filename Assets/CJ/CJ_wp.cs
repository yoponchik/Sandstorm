using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJ_wp : MonoBehaviour
{
    public static CJ_wp instance;

    private void Awake()
    {
        CJ_wp.instance = this;
    }

    public int attack = 10;

    public int damage
    {
        get
        {
            return attack;
        }
        set
        {
            attack = value;
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
