using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJ_AnimationEvent : MonoBehaviour
{
    public CJ_PlayerMove Player;

    public void OnAttack()
    {
        Player.OnAttack();
    }
    public void OnWalk()
    {
        Player.OnWalk();
    }
    public void OnRun()
    {
        Player.OnRun();
    }
    public void OnIdle()
    {
        Player.OnIdle();
    }
    public void OnJump()
    {
        Player.OnJump();
    }
    public void OnHit()
    {
        Player.OnHit();
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
