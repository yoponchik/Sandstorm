using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    public GameObject spiders;

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
        if (other.gameObject.name.Contains("Player"))
        {
            CJ_CameraMove.instance.mainBGM.enabled = false;
            GetComponent<AudioSource>().Play();
            GetComponent<BoxCollider>().enabled = false;
            spiders.SetActive(true);
        }
    }

    
    
}
