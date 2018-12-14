using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    //Raycasting
    //Layermask used to make the enemy ignore himself, bullets, and other enemie when raycasting
    [SerializeField] protected LayerMask E_LayerMask;

    protected Vector3 E_LineCastPos;
	protected bool E_IsGrounded;
	protected bool E_IsBlocked;
    protected bool E_IsBlockedByEnemy;
	protected SpriteRenderer E_SpriteRenderer;
	protected float E_Width;
	protected float E_Height;


	protected Rigidbody2D E_RigidBody;
	protected Transform E_Transform;

	//Movement
	protected bool E_FacingRight;
	protected float E_Speed;
	protected Vector3 E_Rotation;

	protected GameObject E_FindPlayer;
	protected Vector3 E_PlayerPosition;

	protected int E_State;
	protected float E_StateChangeTimer;
	[Range(0.0f, 15.0f)]
	[SerializeField] protected float E_RaycastGround;
	[Range(0.0f, 5.0f)]
	[SerializeField] protected float E_RaycastBlocked;


	protected void E_RayCast()
	{
		E_LineCastPos = E_Transform.position - E_Transform.right * E_Width + E_Transform.up * E_Height;

		// Returns true when the line collides with a collider.
		// The layer mask at the end makes sure the line does not return true if it collides with the object's own collider.
		if (E_State == 1)
		{
			bool E_IsGrounded = Physics2D.Linecast(E_LineCastPos, E_LineCastPos + Vector3.down * E_RaycastGround, E_LayerMask);
			bool E_IsBlocked = Physics2D.Linecast(E_LineCastPos, E_LineCastPos - E_Transform.right * E_RaycastBlocked, E_LayerMask);
            /*
            if (E_FacingRight)
            {
                E_IsBlockedByEnemy = Physics2D.Linecast(transform.position + (Vector3.right/3.0f), transform.position + Vector3.right, E_LayerMask2);
                Debug.DrawLine(transform.position + (Vector3.right / 3.0f), transform.position + Vector3.right);
            }
            else
            {
                E_IsBlockedByEnemy = Physics2D.Linecast(transform.position - (Vector3.right / 3.0f), transform.position - Vector3.right, E_LayerMask2);
                Debug.DrawLine(transform.position - (Vector3.right / 3.0f), transform.position - Vector3.right);
            }

            */
            Debug.DrawLine(E_LineCastPos, E_LineCastPos - E_Transform.right * E_RaycastBlocked);
			Debug.DrawLine(E_LineCastPos, E_LineCastPos + Vector3.down * E_RaycastGround);


            if (!E_IsGrounded == true || E_IsBlocked == true)
			{

				E_Flip();

			}
		}

	}


	protected void E_Flip()
	{
		//Rotate the enemy around the Y axis by 180 degrees
		E_FacingRight = !E_FacingRight;
		E_Rotation.y += 180;
		E_Transform.eulerAngles = E_Rotation;
	}

	protected void E_Start()
	{

		E_RigidBody = gameObject.GetComponent<Rigidbody2D>();
		E_Transform = gameObject.GetComponent<Transform>();
		E_SpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		E_Width = E_SpriteRenderer.bounds.extents.x;
		E_Height = E_SpriteRenderer.bounds.extents.y;
		E_State = 1;
		E_StateChangeTimer = 0.0f;
        if (E_RaycastGround == 0.0f)
        {
            E_RaycastGround = 1.5f;
        }
        if(E_RaycastBlocked == 0.0f)
        {
            E_RaycastBlocked = 2.0f;
        }


		//Enemy starts looking left
		E_FacingRight = false;
		E_IsBlocked = false;
		E_IsGrounded = true;
        E_IsBlockedByEnemy = false;


        if (E_FindPlayer = GameObject.Find("Player"))
		{
			E_PlayerPosition = E_FindPlayer.GetComponent<Transform>().position;
		}
		else
		{
			Debug.Log("Player was not found");
		}
		//Raycast position is from the enemy's position to the left of it by the amount of its width and as high as its height
		E_LineCastPos = E_Transform.position - E_Transform.right * E_Width + E_Transform.up * E_Height;


		E_Rotation = E_Transform.eulerAngles;

		E_Speed = 2.0f;

	}
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag.Equals("Enemy_1") || collision.gameObject.tag.Equals("Enemy_2") || collision.gameObject.tag.Equals("Enemy_3"))
        {
            //Debug.Log("I flipped");
            E_Flip();
        }
    }


}
