using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2 : Enemy {


	//Jumping
	private Vector2 E2_JumpForce;
	private float E2_JumpRate;
	private float E2_NextJump;
	private float E2_WindUpTimer;




	// Use this for initialization
	void Start()
	{
		E_Start();

		E2_JumpRate = 1.0f;
		E2_WindUpTimer = 0.3f;
		E2_NextJump = 0.0f;
		E2_JumpForce = new Vector2(1500,1500);
		E_StateChangeTimer = 3.0f;


	}

	// Update is called once per frame
	void FixedUpdate()
	{
		if (E_State == 1)
		{
			E_RayCast();
		}
		E2_CheckPlayerPosition();


		if (E_State == 1)
		{
			E_Speed = 2.0f;
		}
		else if (E_State == 2)
		{
			E_Speed = 0.0f;
			if (E2_WindUpTimer < -0)
			{
				E2_ApplyForceDown();
			}
		}

		//Enemy generally moves to the left
		E_RigidBody.velocity = -E_Transform.right * E_Speed;


	}



	private void E2_CheckPlayerPosition()
	{
		if(E_FindPlayer)
		E_PlayerPosition = E_FindPlayer.GetComponent<Transform>().position;

		//Debug.Log("faaaaaaaaaaaaaaaaaaaaaalse");

		//If the palyer is 8 units or less away from the enemy, and they're both at roughly the same Y position, the enemy stops moving and starts shooting
		if (Mathf.Abs(E_PlayerPosition.x - E_Transform.position.x) <= 8.0f && Mathf.Abs(E_PlayerPosition.y - E_Transform.position.y) <= 1.0f)
		{
			E_State = 2;
			E_StateChangeTimer = 1.0f;
			//Debug.Log("NAAANNIII");

			//The enemy flips to face the player
			if ((E_PlayerPosition.x - E_Transform.position.x <= 0) && Mathf.Abs(E_PlayerPosition.y - E_Transform.position.y) <= 1.0f && E_FacingRight == true)
			{

					//Debug.Log("Flipxxx");
					E_Flip();
				
			}
			else if ((E_PlayerPosition.x - E_Transform.position.x > 0) && Mathf.Abs(E_PlayerPosition.y - E_Transform.position.y) <= 1.0f && E_FacingRight == false)
			{
				
					//Debug.Log("Flipzzz");
					E_Flip();
				
			}

			if(E2_WindUpTimer>=0.0F)
			{
				E2_WindUpTimer -= Time.deltaTime;
			}

			if (E2_WindUpTimer <= 0.0F)
			{
				E2_Jump();
			}

		}
		else if (Mathf.Abs(E_PlayerPosition.x - E_Transform.position.x) >= 8.0f && Mathf.Abs(E_PlayerPosition.y - E_Transform.position.y) >= 1.0f)
		{
			E_State = 1;
			E2_WindUpTimer = 0.3f;

		}
		else
		{
			//Debug.Log("ElSSSEEEE");
			E_StateChangeTimer -= Time.deltaTime;
			if(E_StateChangeTimer<=0)
			{
				E_StateChangeTimer = 3.0f;
				E2_WindUpTimer = 0.3f;
				E_State = 1;
			}
		}

	}

	private void E2_Jump()
	{

		bool E2_IsGrounded = Physics2D.Linecast(E_LineCastPos, E_LineCastPos + Vector3.down * 0.5f, E_LayerMask);
		bool E2_IsBlocked = Physics2D.Linecast(E_LineCastPos, E_LineCastPos - E_Transform.right * 2.0f, E_LayerMask);

		if (!E2_IsGrounded || E2_IsBlocked)
		{
			//Debug.Log("No Ground");

			E2_ApplyForceDown();
		}

		if (Time.time > E2_NextJump)
		{
			E2_NextJump = Time.time + E2_JumpRate;

			//Debug.Log("Grounded " + E2_IsGrounded);
			//Debug.Log("Blocked " + E2_IsBlocked);
			//Debug.Log("Attempting to Jump");

			if (E2_JumpRate > 0)
			{

				//Debug.Log("Blocked " + E_IsBlocked);

				if (E_FacingRight && E2_IsGrounded && !E2_IsBlocked)
				{
					//Debug.Log("Jumping");
					E_RigidBody.AddForce(E2_JumpForce);
				}
				else if (!E_FacingRight && E2_IsGrounded && !E2_IsBlocked)
				{
					E_RigidBody.AddForce(new Vector2(-E2_JumpForce.x, E2_JumpForce.y));
				}


			}
		}


	}

	private void E2_ApplyForceDown()
	{
		if (!E_FacingRight)
		{

			E_RigidBody.AddRelativeForce(new Vector2(-E2_JumpForce.x * 0.1f, -E2_JumpForce.y * 0.1f));
		}
		else
		{
			E_RigidBody.AddRelativeForce(new Vector2(E2_JumpForce.x * 0.1f, -E2_JumpForce.y * 0.1f));
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
