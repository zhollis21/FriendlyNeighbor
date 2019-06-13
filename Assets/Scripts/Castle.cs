using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Castle : UnitHealth
{

    public GameManager.Team team;
    public bool IsBattleMode;
    public Slider slider;

    private const int startHealth = 1000;

    // Use this for initialization
	void Start ()
    {
        SetHealth(startHealth);
        healthSlider = slider;
        healthSlider.maxValue = startHealth;
        healthSlider.value = startHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
        healthSlider.value = GetHealth();
	}

    public override void Kill()
    {
        if (IsBattleMode)
            GameManager.instance.GameOver(team);
        else
            SurvivalGameManager.instance.GameOver();
        base.Kill();
    }
}
