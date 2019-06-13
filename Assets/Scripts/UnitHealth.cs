using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealth : MonoBehaviour
{

    private int health;
    protected Slider healthSlider;
    protected Animator anim;
    
	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void SetHealth(int health)
    {
        this.health = health;
        if (healthSlider != null)
        {
            healthSlider.maxValue = health;
            healthSlider.value = health;
        }        
    }

    public int GetHealth()
    {
        return health;
    }

    public void GiveHealth(int extraHealth)
    {
        health += extraHealth;
        if (health > healthSlider.maxValue)
            health = (int)healthSlider.maxValue;

        if (healthSlider != null)
            healthSlider.value = health;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (healthSlider != null)
            healthSlider.value = health;

        if (health <= 0)
        {
            if (anim != null)
                anim.SetTrigger("Dying");
            else
                Kill();
        }
    }

    public virtual void Kill()
    {
        GameObject.Destroy(gameObject);
    }
}
