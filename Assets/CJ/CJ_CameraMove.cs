using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJ_CameraMove : MonoBehaviour
{
    public static CJ_CameraMove instance;
    private void Awake()
    {
        instance = this;
    }
    enum State
    {
        Follow,
        Shake
    }
    State state;
    public GameObject target;

    public float orffsetX;
    public float orffsetY;
    public float orffsetZ;

    public AudioSource mainBGM;

    void Start()
    {
        mainBGM.enabled = true;
        state = State.Follow;
    }

    void Update()
    {

        if (state == State.Follow)
        {
            Vector3 FixedPos = new Vector3(target.transform.position.x + orffsetX,
                target.transform.position.y + orffsetY,
                target.transform.position.z + orffsetZ);

            Vector3 dir = FixedPos - target.transform.position;
            float dist = dir.magnitude;
            Ray ray = new Ray(target.transform.position, dir);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, dist))
            {
                FixedPos = hitInfo.point;
            }
            
            transform.position = Vector3.Lerp(transform.position, FixedPos, Time.deltaTime * 5);
        }
        else if (state == State.Shake)
        {
            shakeTime -= Time.deltaTime;
            transform.position = shakeOrigin + Random.insideUnitSphere * kShakeAdjust;
            if (shakeTime <= 0)
            {
                state = State.Follow;
            }
        }

    }

    public float kShakeAdjust = 0.2f;
    float shakeTime;
    Vector3 shakeOrigin;

    public void DoShake(float shakeTime)
    {
        state = State.Shake;
        this.shakeTime = shakeTime;
        shakeOrigin = transform.position;
    }

}
