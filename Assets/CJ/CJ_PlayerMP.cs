using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CJ_PlayerMP : MonoBehaviour
{
    public int mp = 0;
    public int maxMP = 100;
    public Slider sliderMP;
    public Text textMPRate;

    public int MP
    {
        get { return mp; }
        set
        {
            if (mp <= maxMP)
            {
                mp = value;
                if (mp > maxMP)
                {
                    mp = maxMP;
                }
            }
            sliderMP.value = mp;
            textMPRate.text =  mp + " / 100";
            if (mp <= 0)
            {
                mp = 0;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sliderMP.maxValue = maxMP;
        MP = maxMP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
