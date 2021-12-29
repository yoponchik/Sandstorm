using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    #region Singleton
    public static Player instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion Singleton

    public float speed = 5;

    enum Items
    {
        hp,
        mp,
        yellowKey,
        greenKey,

    }

    int hp, mp, yellowKey, greenKey;
    public Text textHP, textMP, textYellowKey, textGreenKey;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Item_HP"))
        {
            AddItem(Items.hp);
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("Item_MP"))
        {
            AddItem(Items.mp);
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("Item_YellowKey"))
        {
            AddItem(Items.yellowKey);
            Destroy(other.gameObject);
        }

        else if (other.gameObject.CompareTag("Item_GreenKey"))
        {
            AddItem(Items.greenKey);
            Destroy(other.gameObject);
        }
    }

    void AddItem(Items item)
    {
        if (item == Items.hp)
        {
            hp++;
            textHP.text = hp.ToString();
        }

        if (item == Items.mp)
        {
            mp++;
            textMP.text = mp.ToString();
        }

        if (item == Items.yellowKey)
        {
            yellowKey++;
            textYellowKey.text = yellowKey.ToString();
        }

        if (item == Items.greenKey)
        {
            greenKey++;
            textGreenKey.text = greenKey.ToString();
        }
    }

    void CheckKey() //check key
    {
        if (gameObject.CompareTag("Door_Perm"))
        {
            if (yellowKey > 0)
            {
                yellowKey--;
                print("open door");
                //textYellowKey.text = yellowKey.ToString();
            }
            else if (yellowKey == 0)
            {
                print("No Key");
            }
            else if (yellowKey < 0) {
                print("Can't have negative number of keys...");
            }
        }

        else if (gameObject.CompareTag("Door_NotPerm"))
        {
            if (greenKey > 0)
            {
                greenKey--;
                print("open door");
                //textYellowKey.text = yellowKey.ToString();
            }
            else if (greenKey == 0)
            {
                print("No Key");
            }
            else if (greenKey < 0)
            {
                print("Can't have negative number of keys...");
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float hori = Input.GetAxis("Horizontal");
        float verti = Input.GetAxis("Vertical");

        Vector3 dir = Vector3.right * hori + Vector3.forward * verti;

        dir.Normalize();

        transform.position += dir *  speed * Time.deltaTime;
    }
}
