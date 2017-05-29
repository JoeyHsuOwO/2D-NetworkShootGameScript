using UnityEngine;
using System.Collections;

//Ãä¬É°»´ú
public class PlayerBounds : MonoBehaviour 
{
	public enum BoundsBehavior 
	{
		Nothing,
		Constrain,
		Kill
	}
	public BoundsBehavior Above;
	public BoundsBehavior Below;
	public BoundsBehavior Left;
	public BoundsBehavior Right;
	
	private BoxCollider2D _bounds;
	private CharacterBehavior _player;
	private BoxCollider2D _boxCollider;
	
	public void Start () 
	{
		_player=GetComponent<CharacterBehavior>();
		_boxCollider=GetComponent<BoxCollider2D>();
		_bounds=GameObject.FindGameObjectWithTag("LevelBounds").GetComponent<BoxCollider2D>();
	}
	

	public void Update () 
	{
		if (_player.BehaviorState.IsDead)
			return;			

		var colliderSize=new Vector2(
			_boxCollider.size.x * Mathf.Abs (transform.localScale.x),
			_boxCollider.size.y * Mathf.Abs (transform.localScale.y))/2;

		if (Above != BoundsBehavior.Nothing && transform.position.y + colliderSize.y > _bounds.bounds.max.y)
			ApplyBoundsBehavior(Above, new Vector2(transform.position.x,_bounds.bounds.max.y - colliderSize.y));
		
		if (Below != BoundsBehavior.Nothing && transform.position.y - colliderSize.y < _bounds.bounds.min.y)
			ApplyBoundsBehavior(Below, new Vector2(transform.position.x, _bounds.bounds.min.y + colliderSize.y));
		
		if (Right != BoundsBehavior.Nothing && transform.position.x + colliderSize.x > _bounds.bounds.max.x)
			ApplyBoundsBehavior(Right, new Vector2(_bounds.bounds.max.x - colliderSize.x,transform.position.y));		
		
		if (Left != BoundsBehavior.Nothing && transform.position.x - colliderSize.x < _bounds.bounds.min.x)
			ApplyBoundsBehavior(Left, new Vector2(_bounds.bounds.min.x + colliderSize.x,transform.position.y));
		
	}
	
	private void ApplyBoundsBehavior(BoundsBehavior behavior, Vector2 constrainedPosition)
	{
		if (behavior== BoundsBehavior.Kill)
		{
            _player.Health = 0;
            _player.CmdChangeTag("Die");
            _player.CmdKillPlayer();
        }	
		transform.position = constrainedPosition;	
	}
}
