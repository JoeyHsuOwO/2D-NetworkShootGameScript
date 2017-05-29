using System;
using UnityEngine;
using System.Collections;

//整合角色數值
[Serializable]
public class CharacterBehaviorParameters 
{
	[Header("Jump")]
	public float JumpHeight = 3.025f;
	public float JumpMinimumAirTime = 0.1f;
	public int NumberOfJumps=3;
	public enum JumpBehavior
	{
		CanJumpOnGround,
		CanJumpAnywhere,
		CantJump,
		CanJumpAnywhereAnyNumberOfTimes
	}

	public JumpBehavior JumpRestrictions;

	public bool JumpIsProportionalToThePressTime=true;
	
	[Space(10)]	
	[Header("Speed")]
	public float MovementSpeed = 8f;
	public float CrouchSpeed = 4f;
	public float WalkSpeed = 8f;
	public float RunSpeed = 16f;
	public float LadderSpeed = 2f;
    public float TankMoveSpeed = 4f;
	
	[Space(10)]	
	[Header("Health")]
	/// the maximum health of the character
	public int MaxHealth = 100;
    public int TankMaxHealth = 300;	
}
