using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour
{

    public GameManager.Team team;

    private float speed = .25f;
    private int undergroundDistance = 5;
    private bool waiting = false;
    private float timer = 0;
    private const float waitTime = 0.5f;
    private Animator anim;
    private Vector3 oldPosition;
    private const int damage = 45;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        speed = team == GameManager.Team.Player1 ? speed : -speed; // sets the direction based on team
        undergroundDistance = team == GameManager.Team.Player1 ? undergroundDistance : -undergroundDistance;
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting)
        {
            timer += Time.deltaTime;

            if (timer >= waitTime)
            {
                anim.SetTrigger("Attack");
                waiting = false;
                transform.position = oldPosition;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != tag)
            collision.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);            
    }

    public void Seq_1()
    {
        transform.position = new Vector3(transform.position.x, -3.1f, 0);
    }

    public void Seq_2()
    {
        transform.position = new Vector3(transform.position.x + speed, -2.7f, 0);
    }

    public void Seq_3()
    {
        transform.position = new Vector3(transform.position.x + speed, -2.5f, 0);
    }

    public void Seq_4()
    {
        transform.position = new Vector3(transform.position.x + speed, -2.32f, 0);
    }

    public void Seq_5()
    {
        transform.position = new Vector3(transform.position.x + speed, -2.13f, 0);
    }

    public void Seq_6()
    {
        transform.position = new Vector3(transform.position.x + speed, -2.17f, 0);
    }

    public void Seq_7()
    {
        transform.position = new Vector3(transform.position.x + speed, -2.165f, 0);
    }

    public void Seq_8()
    {
        transform.position = new Vector3(transform.position.x + speed, -2.165f, 0);
    }

    public void Seq_9()
    {
        transform.position = new Vector3(transform.position.x + speed, -2.22f, 0);
    }

    public void Seq_10()
    {
        transform.position = new Vector3(transform.position.x + speed, -2.165f, 0);
    }

    public void Seq_11()
    {
        transform.position = new Vector3(transform.position.x + speed, -2.177f, 0);
    }

    public void Seq_12()
    {
        transform.position = new Vector3(transform.position.x + speed, -2.18f, 0);
    }

    public void Seq_13()
    {
        transform.position = new Vector3(transform.position.x + speed, -2.31f, 0);
    }

    public void Seq_14()
    {
        transform.position = new Vector3(transform.position.x + speed, -2.58f, 0);
    }

    public void Seq_15()
    {
        transform.position = new Vector3(transform.position.x + speed, -2.81f, 0);
    }

    public void Seq_16()
    {
        transform.position = new Vector3(transform.position.x + speed, -3.05f, 0);
    }

    public void Seq_17()
    {
        if (transform.position.x < GameManager.leftEdge || transform.position.x > GameManager.rightEdge)
            GameObject.Destroy(gameObject);
        oldPosition = transform.position = new Vector3(transform.position.x + undergroundDistance, -3.1f, 0);
        transform.position = new Vector3(0,-100, 0);
        waiting = true;
        timer = 0;        
    }
}
