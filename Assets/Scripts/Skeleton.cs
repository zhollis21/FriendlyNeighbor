using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skeleton : UnitHealth
{

    public GameManager.Team team;

    private const int maxHealth = 15;
    private int speed = 3;
    private Rigidbody2D rb2d;
    private bool walking;
    private RaycastHit2D target;
    private const int damage = 5;
    private LayerMask enemyMask;
    private Vector2 direction;

    // Use this for initialization
    void Start()
    {
        healthSlider = GetComponentInChildren<Slider>();
        speed = team == GameManager.Team.Player1 ? speed : -speed; // sets the direction based on team
        SetHealth(maxHealth);
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetTrigger("Walking");
        walking = true;
        enemyMask = team == GameManager.Team.Player1 ? LayerMask.GetMask("Player2") : LayerMask.GetMask("Player1");
        direction = team == GameManager.Team.Player1 ? Vector2.right : Vector2.left;
    }

    // Update is called once per frame
    void Update ()
    {
        target = Physics2D.Raycast(transform.position, direction, 1.5f, enemyMask);

        if (target.collider != null)
        {
            rb2d.velocity = Vector2.zero;
            walking = false;
            anim.SetTrigger("Attacking");
        }
        else
        {
            anim.SetTrigger("Walking");
            if (walking)
                rb2d.velocity = new Vector2(speed, 0);
        }
    }

    public void Walk()
    {
        walking = true;
    }

    public void Attack()
    {
        if (target.collider != null)
            target.collider.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
    }
}