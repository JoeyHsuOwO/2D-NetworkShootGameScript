using System;
using UnityEngine;
using System.Collections;


[Serializable]
public class CharacterBehaviorControllerParameters
{
	public Vector2 MaxVelocity = new Vector2(200f, 200f);
	public int MaxSpeed=200;
	[Range(0,90)]
	public float MaximumSlopeAngle = 45;		
	public float Gravity = -15;	
	public float SpeedAccelerationOnGround = 20f;
	public float SpeedAccelerationInAir = 5f;	
}
