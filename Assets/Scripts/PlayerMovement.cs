using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
	Vector2 moveInput;
	Rigidbody2D playerRigidbody;
	Animator playerAnimator;
	bool isRunning = false;
	CapsuleCollider2D playerBodyCollider;
	BoxCollider2D playerFeetCollider;
	PlayerInput playerInput;

	

	bool isAlive = true;

	[SerializeField] float playerSpeed = 4;
	[SerializeField] float jumpSpeed;
	[SerializeField] float climbSpeed;
	[SerializeField] float basePlayerGravityScale = 4;
	[SerializeField] Vector2 deathKick  = new Vector2(10f, 10f);
	[SerializeField] int deathResetSceneNum;

	void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
		playerAnimator = GetComponent<Animator>();
		playerBodyCollider = GetComponent<CapsuleCollider2D>();
		playerFeetCollider = GetComponent<BoxCollider2D>();
		playerInput = GetComponent<PlayerInput>();
	}


    void Update()
	{
		if (!isAlive) { return; }
		Run();
		FlipSprite();
		ClimbLadder();
		Die();
	}

	void OnMove(InputValue value)
	{
		if (!isAlive) { return; }
		moveInput = value.Get<Vector2>();
	}

	void OnJump(InputValue value)
	{
		if (!isAlive) { return; }
		//If Player is grounded
		if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }

		if (value.isPressed)
		{
			//Jump
			playerRigidbody.velocity += new Vector2(0f, jumpSpeed);
		}
	}

	void Run()
	{
		//Running Code
		Vector2 playerVelocity = new Vector2(moveInput.x * playerSpeed, playerRigidbody.velocity.y);
		playerRigidbody.velocity = playerVelocity;

		bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;

		playerAnimator.SetBool("isRunning", playerHasHorizontalSpeed);

		
	}

	void FlipSprite()
	{
		//Flip-Image Movement Code
		bool playerHasHorizontalSpeed = Mathf.Abs(playerRigidbody.velocity.x) > Mathf.Epsilon;

		if (playerHasHorizontalSpeed)
		{
			transform.localScale = new Vector2(Mathf.Sign(playerRigidbody.velocity.x), 1f);
		}
	}

	void ClimbLadder()
	{
		if (!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
		{
			playerRigidbody.gravityScale = basePlayerGravityScale;
			playerAnimator.SetBool("isClimbing", false);
			return; 
		}

		Vector2 climbVelocity = new Vector2(playerRigidbody.velocity.x, moveInput.y * climbSpeed);
		playerRigidbody.velocity = climbVelocity;
		playerRigidbody.gravityScale = 0;

		bool playerHasVerticalSpeed = Mathf.Abs(playerRigidbody.velocity.y) > Mathf.Epsilon;
		playerAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
	}

	void Die()
	{
		if (playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
		{
			playerInput.enabled = false;
			playerAnimator.SetTrigger("Dying");
			playerRigidbody.velocity = deathKick;
			StartCoroutine(DeathReset());
			
		}
	}
	IEnumerator DeathReset()
	{
		yield return new WaitForSeconds(5);
		SceneManager.LoadScene(deathResetSceneNum);
	}
}
