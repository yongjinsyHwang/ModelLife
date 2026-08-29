using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimController : MonoBehaviour
{

    public Animator playerAnim;

    public bool Warring1;
    public bool Joke1;
    public bool Joke2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAnim = GetComponent<Animator>();

        Warring1 = false;
        Joke1 = false;
        Joke2 = false;
    }

    // Update is called once per frame
    void Update()
    {
        TestAnimation();
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    playerAnim.SetTrigger("Joke1");
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    playerAnim.SetTrigger("Joke2");
        //}
    }

    public void TestAnimation()
    {
        if (Warring1)
        {
            playerAnim.SetTrigger("Warring1");
            
        }
        else if(!Warring1)
        {
            playerAnim.SetTrigger("Idle");
        }

        if (Joke1)
        {
            playerAnim.SetTrigger("Joke1");

        }
        else if (!Joke1)
        {
            playerAnim.SetTrigger("Idle");
        }
        if (Joke2)
        {
            playerAnim.SetTrigger("Joke2");

        }
        else if (!Joke2)
        {
            playerAnim.SetTrigger("Idle");
        }
    }
}

