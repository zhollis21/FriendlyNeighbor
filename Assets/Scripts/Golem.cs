using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Golem : UnitHealth
{

    public GameManager.Team team;

    private const int maxHealth = 300;
    private int speed = 2;
    private Rigidbody2D rb2d;
    private bool walking;
    private RaycastHit2D[] targets;
    private RaycastHit2D[] lastValidTargets;
    private const int damage = 10;
    private LayerMask enemyMask;
    private Vector2 direction;
    private Vector2 checkFromPosition = new Vector2(0, -2.75f);
    private bool validTarget = false;
    private const int distance = 3;

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
        checkFromPosition.x = transform.position.x;
        targets = Physics2D.RaycastAll(checkFromPosition, direction, distance, enemyMask);

        if (targets.Length > 0)
        {
            lastValidTargets = targets;
            validTarget = true;
            rb2d.velocity = new Vector2(speed*.5f, 0.5f);
            anim.SetTrigger("Attacking");
        }
        else if (!validTarget)
        {
            anim.SetTrigger("Walking");
            rb2d.velocity = new Vector2(speed, 0);
        }
    }

    public void Attack()
    {
        if (lastValidTargets.Length > 0)
            for(int i = 0; i < lastValidTargets.Length; i++)
                if (lastValidTargets[i].collider != null)
                    lastValidTargets[i].collider.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
        lastValidTargets = new RaycastHit2D[0];
        validTarget = false;
    }
}
