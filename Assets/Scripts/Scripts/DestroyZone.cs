using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // ºÎµúÈù ³Ê¸¸ Á×¾î
        

        CJ_GameManager.instance.GameOverUI.SetActive(true);
        CJ_PlayerMove.instance.state = CJ_PlayerMove.State.Die;
        GetComponent<Collider>().enabled = false;
        CJ_PlayerMove.instance.anim.SetTrigger("Die");
        Destroy(other.gameObject,3);
    }
}
