using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//���⦺�`.�d�I����
public class CheckPoint : MonoBehaviour 
{
	private List<IPlayerRespawnListener> _listeners;

	public void Awake () 
	{
		_listeners = new List<IPlayerRespawnListener>();
	}
	

	public void SpawnPlayer(CharacterBehavior player)
	{
		player.RespawnAt(transform);
		
		foreach(var listener in _listeners)
		{
			listener.onPlayerRespawnInThisCheckpoint(this,player);
		}
	}
	
	public void AssignObjectToCheckPoint (IPlayerRespawnListener listener) 
	{
		_listeners.Add(listener);
	}
}
