using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//與Projectile連結,在protected override class編輯class
public class SimpleProjectile : Projectile, CanTakeDamage
{

	public int Damage;
    public float Change;
	public GameObject DestroyedEffect;
	public int PointsToGiveToPlayer;	
	public float TimeToLive;
    public GameObject UpUpcollider;


	public void Update () 
	{

		if ((TimeToLive -= Time.deltaTime) <= 0)
		{
			DestroyProjectile();
			return;
		}

		transform.Translate(Direction * ((Mathf.Abs (InitialVelocity.x)+Speed) * Time.deltaTime),Space.World);

	}	

    [Command]
	public void CmdTakeDamage(int damage, GameObject instigator)
	{
        RpcTakeDamage(damage , instigator);
	}
    [ClientRpc]
    public void RpcTakeDamage(int damage, GameObject instigator)
    {
        DestroyProjectile();
    }

	
	protected override void OnCollideOther(Collider2D collider)
	{
        DestroyProjectile();
    }



    protected override void OnCollideTakeDamage(Collider2D collider, CanTakeDamage takeDamage)
	{

            takeDamage.CmdTakeDamage(Damage, gameObject);
            DestroyProjectile();
        
	}

    protected override void OnCollideThorns(Collider2D OwnerCollider, CanTakeDamage GetHurt)
    {
        
        GetHurt.CmdTakeDamage(Damage, gameObject);
        DestroyProjectile();
    }

    protected override void OnCollideUpUp()
    {
        if(Change == 0)
        {
            Change = Damage * 0.75f;
            Damage = (int)Change;
        }       
    }


    private void DestroyProjectile()
	{
		if (DestroyedEffect!=null)
		{
			Instantiate(DestroyedEffect,transform.position,transform.rotation);
		}
		
		Destroy(gameObject);
	}
}
