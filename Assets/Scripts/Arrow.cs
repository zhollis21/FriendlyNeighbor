using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public GameManager.Team team;

    private const int damage = 10;
    private const float hangTime = 0.5f;
    private float time = 0;
    private bool hit = false;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (hit)
            time += Time.deltaTime;

        if (time >= hangTime)
            GameObject.Destroy(gameObject);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hit)
        {
            collision.collider.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            hit = true;
        }
    }
}
