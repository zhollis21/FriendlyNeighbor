using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Necromancer : UnitHealth
{

    public GameManager.Team team;
    public bool isBattleMode;
    public GameObject PrefabSkeleton;

    private const int maxHealth = 75;
    private int speed = 2;
    private Rigidbody2D rb2d;
    private bool doneSummoning;
    private Vector2 direction;
    private float timeSinceLastSummon = 0;
    private const float timeBetweenSummons = 5;
    private Vector3 offset1;
    private Vector3 offset2;

    // Use this for initialization
    void Start()
    {
        healthSlider = GetComponentInChildren<Slider>();
        speed = team == GameManager.Team.Player1 ? speed : -speed; // sets the direction based on team
        SetHealth(maxHealth);
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        doneSummoning = true;
        offset1 = new Vector3(.5f, -1, 0);
        offset2 = new Vector3(-.5f, -1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSummon += Time.deltaTime;

        if (timeSinceLastSummon >= timeBetweenSummons)
        {
            doneSummoning = false;
            timeSinceLastSummon = 0;
            anim.SetTrigger("Summoning");
        }
        else if(doneSummoning)
        {
            rb2d.velocity = new Vector2(speed, 0);
        }
    }

    public void Summon()
    {
        if (isBattleMode)
        {
            if (team == GameManager.Team.Player1)
            {
                GameManager.instance.AddToPlayer1Units(Instantiate(PrefabSkeleton, this.transform.position + offset1, this.transform.rotation));
                GameManager.instance.AddToPlayer1Units(Instantiate(PrefabSkeleton, this.transform.position + offset2, this.transform.rotation));
            }
            else
            {
                GameManager.instance.AddToPlayer2Units(Instantiate(PrefabSkeleton, this.transform.position + offset1, this.transform.rotation));
                GameManager.instance.AddToPlayer2Units(Instantiate(PrefabSkeleton, this.transform.position + offset2, this.transform.rotation));
            }
            doneSummoning = true;
        }
        else
        {
            if (team == GameManager.Team.Player1)
            {
                SurvivalGameManager.instance.AddToPlayer1Units(Instantiate(PrefabSkeleton, this.transform.position + offset1, this.transform.rotation));
                SurvivalGameManager.instance.AddToPlayer1Units(Instantiate(PrefabSkeleton, this.transform.position + offset2, this.transform.rotation));
            }
            else
            {
                SurvivalGameManager.instance.AddToPlayer2Units(Instantiate(PrefabSkeleton, this.transform.position + offset1, this.transform.rotation));
                SurvivalGameManager.instance.AddToPlayer2Units(Instantiate(PrefabSkeleton, this.transform.position + offset2, this.transform.rotation));
            }
            doneSummoning = true;
        }
    }

    public void stop()
    {
        rb2d.velocity = Vector2.zero;
    }
}