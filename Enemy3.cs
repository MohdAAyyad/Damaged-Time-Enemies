using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : Enemy {


	//Jumping
	private float E3_ChargeRate;
	private float E3_NextCharge;
	private float E3_WindUpTimer;
	private float E3_ChargeTime;
	private float E3_ChargeDistance;




	// Use this for initialization
	void Start()
	{
		E_Start();

		E3_ChargeRate = 1.0f;
		E3_WindUpTimer = 0.1f;
		E3_NextCharge = 0.0f;
		E3_ChargeDistance = 7.5f;
		E3_ChargeTime = 0.0f;
		E_StateChangeTimer = 0.2f;



	}

	// Update is called once per frame
	void FixedUpdate()
	{
		E_RayCast();
		E3_CheckPlayerPosition();

		//Enemy generally moves to the left
		E_RigidBody.velocity = -E_Transform.right * E_Speed;

		//Debug.Log(E_Speed);



	}



	private void E3_CheckPlayerPosition()
	{
		if(E_FindPlayer)
		E_PlayerPosition = E_FindPlayer.GetComponent<Transform>().position;

		//Debug.Log("faaaaaaaaaaaaaaaaaaaaaalse");

		//If the palyer is 8 units or less away from the enemy, and they're both at roughly the same Y position, the enemy stops moving and starts shooting
		if (Mathf.Abs(E_PlayerPosition.x - E_Transform.position.x) <= 8.0f && Mathf.Abs(E_PlayerPosition.y - E_Transform.position.y) <= 1.0f)
		{
			E_State = 2;
			E_StateChangeTimer = 0.2f;
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

			if (E3_WindUpTimer >= 0.0f)
			{
				E3_WindUpTimer -= Time.deltaTime;
			}

			else if (E3_WindUpTimer <= 0.0f)
			{
				E3_Charge();
			}

		}
		else if (Mathf.Abs(E_PlayerPosition.x - E_Transform.position.x) >= 8.0f && Mathf.Abs(E_PlayerPosition.y - E_Transform.position.y) >= 1.0f)
		{
			E_State = 1;
			E3_WindUpTimer = 0.1f;

		}
		else
		{
			//Debug.Log("ElSSSEEEE");
			E_StateChangeTimer -= Time.deltaTime;
			if (E_StateChangeTimer <= 0)
			{
				E_StateChangeTimer = 0.2f;
				E3_WindUpTimer = 0.1f;
				E_Speed = 2.0f;
				E3_ChargeTime = 0.0f;
				E_State = 1;
			}
		}

	}

	private void E3_Charge()
	{

		if (Time.time > E3_NextCharge)
		{
			E3_NextCharge = Time.time + E3_ChargeRate;

			//Debug.Log("Grounded " + E2_IsGrounded);
			//Debug.Log("Blocked " + E2_IsBlocked);
			//Debug.Log("Attempting to Jump");

			if (E3_ChargeRate > 0)
			{
				E_Speed = 10.0f;
			}

		}

		if (E_Speed == 10 && (E_Speed * E3_ChargeTime >= E3_ChargeDistance))
		{
			E_Speed = 2.0f;
			E3_ChargeTime = 0.0f;
		}
		else if (E_Speed == 10 && (E_Speed * E3_ChargeTime <= E3_ChargeDistance))
		{
			E3_ChargeTime += Time.deltaTime;
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