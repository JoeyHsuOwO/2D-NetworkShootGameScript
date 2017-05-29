using UnityEngine;
using System.Collections;

//槍枝切換
//更改槍枝產生的特效以及數值
public class Weapon : MonoBehaviour 
{
	public Projectile Projectile;
	public float FireRate;
	public GameObject GunRotationCenter;
	public ParticleSystem GunFlames;
	public ParticleSystem GunShells;	
	public Transform ProjectileFireLocation;
	public AudioClip GunShootFx;

	void Start()
	{
		SetGunFlamesEmission (false);
		SetGunShellsEmission (false);
	}
	
	public void SetGunFlamesEmission(bool state)
	{
		GunFlames.enableEmission=state;	
	}
	
	public void SetGunShellsEmission(bool state)
	{
		GunShells.enableEmission=state;	
	}
}
