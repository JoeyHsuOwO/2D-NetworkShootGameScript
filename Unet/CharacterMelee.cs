using UnityEngine;
using System.Collections;

//角色劍武器使用
public class CharacterMelee : MonoBehaviour 
{
	public GameObject MeleeCollider;
	public float MeleeAttackDuration=0.3f;
	private CharacterBehavior _characterBehavior;

	void Start () { 

		_characterBehavior = GetComponent<CharacterBehavior>();
		
		if (MeleeCollider!=null)
		{
			MeleeCollider.SetActive(false);
		}
	}

	public void Melee()
	{	
		if (!_characterBehavior.Permissions.MeleeAttackEnabled)
			return;
		if (_characterBehavior.BehaviorState.IsDead)
			return;
		if (!_characterBehavior.BehaviorState.CanMoveFreely)
			return;

		if (_characterBehavior.BehaviorState.CanMelee)
		{	
			_characterBehavior.BehaviorState.MeleeAttacking=true;
			MeleeCollider.SetActive(true);
			StartCoroutine(MeleeEnd());			
		}
	}

	private IEnumerator MeleeEnd()
	{
		yield return new WaitForSeconds(MeleeAttackDuration);
		MeleeCollider.SetActive(false);
		_characterBehavior.BehaviorState.MeleeAttacking=false;
	}
}
