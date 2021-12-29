using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Portal : MonoBehaviour
{

    public GameObject receiver;
    public GameObject player;

    private bool playerIsOverlapping = false;

    


    // Update is called once per frame
    void Update()
    {
        if (playerIsOverlapping) {
            Teleport();
            StartCoroutine(TeleportCooldown());
        }
        
    }

    IEnumerator TeleportCooldown() {
        receiver.SetActive(false);
        yield return new WaitForSeconds(5);
        receiver.SetActive(true);
        
    }

    void Teleport() {
        GetComponent<AudioSource>().Play();
        player.transform.position = receiver.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            playerIsOverlapping = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") {
            playerIsOverlapping = false;
        }
    }

}
