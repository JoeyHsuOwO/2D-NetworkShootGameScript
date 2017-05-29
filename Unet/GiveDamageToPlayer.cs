using UnityEngine;
using System.Collections;

public class GiveDamageToPlayer : MonoBehaviour 
{
	public int DamageToGive = 10;
	
	private Vector2
		_lastPosition,
		_velocity;
	
	public void LateUpdate () 
	{
		_velocity = (_lastPosition - (Vector2)transform.position) /Time.deltaTime;
		_lastPosition = transform.position;
	}
	

}
