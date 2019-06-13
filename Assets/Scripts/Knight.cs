using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Knight : UnitHealth
{

    public GameManager.Team team;

    private const int maxHealth = 100;
    private int speed = 3;
    private Rigidbody2D rb2d;
    private RaycastHit2D target;
    private RaycastHit2D lastValidTarget;
    private bool validTarget = false;
    private bool walking = false;
    private const int damage = 20;
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
        enemyMask = team == GameManager.Team.Player1 ? LayerMask.GetMask("Player2") : LayerMask.GetMask("Player1");
        direction = team == GameManager.Team.Player1 ? Vector2.right : Vector2.left;
    }

    // Update is called once per frame
    void Update()
    {
        target = Physics2D.Raycast(transform.position, direction, 2, enemyMask);

        if (target.collider != null)
        {
            lastValidTarget = target;
            validTarget = true;
            rb2d.velocity = Vector2.zero;
            anim.SetTrigger("Attacking");
        }
        else if (!validTarget)
        {
            if (!walking)
                anim.SetTrigger("Walking");
            rb2d.velocity = new Vector2(speed, 0);
            walking = true;
        }
    }

    public void Attack()
    {
        if (lastValidTarget.collider != null)
            lastValidTarget.collider.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        lastValidTarget = new RaycastHit2D();
        validTarget = false;
        walking = false;
    }
}
