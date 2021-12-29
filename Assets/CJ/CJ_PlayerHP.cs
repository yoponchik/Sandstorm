using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CJ_PlayerHP : MonoBehaviour
{

    #region Singleton
    public static CJ_PlayerHP instance;
    private void Awake()
    {
        CJ_PlayerHP.instance = this;
    }
    #endregion

    public int hp = 0;
    public int maxHP = 100;
    public Slider sliderHP;
    public Text textHPRate;

    CJ_PlayerMove playerMove;

    public int HP
    {
        get { return hp; }
        set
        {
            if (hp <= maxHP)
            {
                hp = value;
                if (hp > maxHP)
                {
                    hp = maxHP;
                }
            }
            
            sliderHP.value = hp;
            // textHPRate.text = ((float)hp / maxHP * 100).ToString() + "%";
            textHPRate.text = hp + " / 100";


            if (hp <= 0) {
                playerMove.DoDie();
            }
            
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        sliderHP.maxValue = maxHP;
        HP = maxHP;
        playerMove = GetComponent<CJ_PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
