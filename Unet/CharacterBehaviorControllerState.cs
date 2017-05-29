using UnityEngine;
using System.Collections;


//����I����T���� �שY���קP�_
public class CharacterBehaviorControllerState
{
	public bool IsCollidingRight { get; set; }
	public bool IsCollidingLeft { get; set; }
	public bool IsCollidingAbove { get; set; }
	public bool IsCollidingBelow { get; set; }
	public bool HasCollisions { get { return IsCollidingRight || IsCollidingLeft || IsCollidingAbove || IsCollidingBelow; }}
	
	public bool IsMovingDownSlope { get; set; }
	public bool IsMovingUpSlope { get; set; }
	public float SlopeAngle { get; set; }
	public bool SlopeAngleOK { get; set; }

	public bool IsGrounded { get { return IsCollidingBelow; } }
	public bool IsFalling { get; set; }
	public bool WasGroundedLastFrame { get ; set; }
	public bool JustGotGrounded { get ; set;  }

	public void Reset()
	{
		IsMovingUpSlope =
		IsMovingDownSlope =
		IsCollidingLeft = 
		IsCollidingRight = 
		IsCollidingAbove =
		SlopeAngleOK =
		JustGotGrounded = false;
		IsFalling=true;
		SlopeAngle = 0;
	}
	
	public override string ToString ()
	{
		return string.Format("(controller: r:{0} l:{1} a:{2} b:{3} down-slope:{4} up-slope:{5} angle: {6}",
		IsCollidingRight,
		IsCollidingLeft,
		IsCollidingAbove,
		IsCollidingBelow,
		IsMovingDownSlope,
		IsMovingUpSlope,
		SlopeAngle);
	}	
}