using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : Enemy {

	//Bullets
	[SerializeField] private GameObject E1_Bullet;
	private float E1_BulletForce;
	private Vector3 E1_BulletPos;
	private float E1_FireRate;
	private float E1_NextFire;
	private float E1_WindUpTimer;



	// Use this for initialization
	void Start () {
		E_Start();

		E1_FireRate = 2.0f;
		E1_NextFire = 0.0f;
		E1_BulletForce = 500.0f;
		E1_WindUpTimer = 0.2f;


	}
	
	// Update is called once per frame
	void FixedUpdate () {

		E_RayCast();
		E1_CheckPlayerPosition();

		//Enemy generally moves to the left
		E_RigidBody.velocity = -E_Transform.right *E_Speed;

	}



	private void E1_CheckPlayerPosition()
	{
		if (E_FindPlayer)
			E_PlayerPosition = E_FindPlayer.GetComponent<Transform>().position;

		//If the palyer is 4 units or less away from the enemy, and they're both at roughly the same Y position, the enemy stops moving and starts shooting
		if(Mathf.Abs(E_PlayerPosition.x - E_Transform.position.x)  <= 8.0f && Mathf.Abs(E_PlayerPosition.y - E_Transform.position.y) <= 1.0f)
		{
			E_Speed = 0.0f;

			//The enemy flips to face the player
			if((E_PlayerPosition.x - E_Transform.position.x <=0) && E_FacingRight == true)
			{		
				E_Flip();
			}
			else if ((E_PlayerPosition.x - E_Transform.position.x > 0) && E_FacingRight==false)
			{
				E_Flip();
			}

			if (E1_WindUpTimer <= 0)
			{
				if (Time.time > E1_NextFire)
				{
					E1_NextFire = Time.time + E1_FireRate;
					E1_Fire();
				}
			}
			else
			{
				E1_WindUpTimer -= Time.deltaTime;
			}

		}
		else
		{
			E_Speed = 2.0f;
			E1_WindUpTimer = 0.2f;
		}

	}
	
	private void E1_Fire()
	{
		if (E1_FireRate > 0)
		{
			// Instantiate bullets
			E1_BulletPos = new Vector3(this.transform.position.x, this.transform.position.y, 0f);
			GameObject E1_Bullet = (GameObject)Instantiate(this.E1_Bullet, E1_BulletPos, transform.rotation);

			//Determine the direction of the bullets
			if (!E_FacingRight)
			{
				E1_Bullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(-E1_BulletForce, 0f));
			}
			else
			{
				E1_Bullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(E1_BulletForce, 0f));
			}
		}
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
