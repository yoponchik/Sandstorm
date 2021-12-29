using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{


    public float proximityRadius = 4f;
    public Transform player;

    public Vector3 doorOffset = new Vector3(0, 6, 0);

    public float moveDuration = 5f;
    private float elapsedTime;
    private Vector3 startPosition;



    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        

        float distance = Vector3.Distance(player.position, transform.position);
        // 거리가 되면
        if(distance <= proximityRadius) {
            if (CJ_PlayerMove.instance.yellowKey == true && CJ_PlayerMove.instance.greenKey == true)
            {
                GetComponent<AudioSource>().Play();
                MoveDoor();
            }

        }

    }


    void  MoveDoor() {
        elapsedTime += Time.deltaTime;
        float percentageComplete = elapsedTime / moveDuration;
        Vector3 endPosition = startPosition + doorOffset;
        transform.position = Vector3.Lerp(startPosition, endPosition, percentageComplete);
    }



   
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, proximityRadius);
    }


}
