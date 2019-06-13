using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Miner : UnitHealth
{

    public GameManager.Team team;

    private Rigidbody2D rb2d;
    private const int maxHealth = 100;
    private const int player1MineX = 15;
    private const int player2MineX = 55;
    private int speed = 1;
    private float mineDistance = 1.5f;
    private bool added = false;

	// Use this for initialization
	void Start ()
    {
        healthSlider = GetComponentInChildren<Slider>();
        SetHealth(maxHealth);
        speed = team == GameManager.Team.Player1 ? speed : -speed; // sets the direction based on team;
        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!added && team == GameManager.Team.Player1 && transform.position.x > player1MineX - mineDistance && transform.position.x < player1MineX + mineDistance)
        {
            rb2d.velocity = Vector2.zero;
            anim.SetTrigger("Mining");
            GameManager.instance.Player1AddMiner();
            added = true;
        }
        else if (!added && team == GameManager.Team.Player2 && transform.position.x > player2MineX - mineDistance && transform.position.x < player2MineX + mineDistance)
        {
            rb2d.velocity = Vector2.zero;
            anim.SetTrigger("Mining");
            GameManager.instance.Player2AddMiner();
            added = true;
        }
        else if (!added)
            rb2d.velocity = new Vector2(speed, 0);
    }

    public void KillMiner()
    {
        if (added && team == GameManager.Team.Player1)
            GameManager.instance.Player1RemoveMiner();
        else if (added)
            GameManager.instance.Player2RemoveMiner();

        GameObject.Destroy(gameObject);
    }
}
