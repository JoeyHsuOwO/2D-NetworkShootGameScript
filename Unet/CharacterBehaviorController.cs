using UnityEngine;
using System.Collections;

//處理角色移動,物理碰撞,射線偵測等參數
public class CharacterBehaviorController : MonoBehaviour 
{
	public CharacterBehaviorControllerState State { get; private set; }
	public CharacterBehaviorControllerParameters DefaultParameters;
	public CharacterBehaviorControllerParameters Parameters {get{return _overrideParameters ?? DefaultParameters;}}


	
	[Space(10)]	
	[Header("Collision Masks")]
	public LayerMask PlatformMask=0;
	public LayerMask MovingPlatformMask=0;
	public LayerMask EdgeColliderPlatformMask=0;
	public GameObject StandingOn { get; private set; }	
	public Vector2 Speed { get{ return _speed; } }
	
	[Space(10)]	
	[Header("Raycasting")]
	public int NumberOfHorizontalRays = 8;
	public int NumberOfVerticalRays = 8;	
	public float RayOffset=0.05f; 
	
	private CharacterBehaviorControllerParameters _overrideParameters;	
	private Vector2 _speed;
	private float _fallSlowFactor;
	private Vector2 _externalForce;
	private Vector2 _newPosition;
	private Transform _transform;
	private BoxCollider2D _boxCollider;
	private GameObject _lastStandingOn;
	private LayerMask _platformMaskSave;
	
	private const float _largeValue=500000f;
	private const float _smallValue=0.0001f;
	private const float _obstacleHeightTolerance=0.05f;
	
	private Rect _rayBoundsRectangle;
		

	public void Awake()
	{
		_transform=transform;
		_boxCollider = (BoxCollider2D)GetComponent<BoxCollider2D>();
		State = new CharacterBehaviorControllerState();

		_platformMaskSave = PlatformMask;	
		PlatformMask |= EdgeColliderPlatformMask;
		PlatformMask |= MovingPlatformMask;
		
		State.Reset();
		SetRaysParameters();
	}

    //角色透過對rigidbody施加force來進行移動
	public void AddForce(Vector2 force)
	{
		_speed += force;	
		_externalForce += force;
    }

    public void AddHorizontalForce(float x)
    {
        _speed.x += x;
        _externalForce.x += x;
    }

    public void AddVerticalForce(float y)
    {
        _speed.y += y;
		_externalForce.y += y;
    }

	public void SetForce(Vector2 force)
	{
		_speed = force;
		_externalForce = force;	
	}
	
	public void SetHorizontalForce (float x)
	{
		_speed.x = x;
		_externalForce.x = x;
	}

	public void SetVerticalForce (float y)
	{
		_speed.y = y;
		_externalForce.y = y;
		
	}

    //rigidbody更新數值
	private void LateUpdate()
	{	
        if(gameObject.tag != "Daze")
        {
            _speed.y += Parameters.Gravity * Time.deltaTime;

            if (_fallSlowFactor != 0)
            {
                _speed.y *= _fallSlowFactor;
            }

            _newPosition = Speed * Time.deltaTime;

            State.WasGroundedLastFrame = State.IsCollidingBelow;
            State.Reset();

            CastRaysToTheSides();
            CastRaysBelow();
            CastRaysAbove();

            _transform.Translate(_newPosition, Space.World);

            if (Time.deltaTime > 0)
                _speed = _newPosition / Time.deltaTime;

            SetRaysParameters();

            _externalForce.x = 0;
            _externalForce.y = 0;

            _speed.x = Mathf.Min(_speed.x, Parameters.MaxVelocity.x);
            _speed.y = Mathf.Min(_speed.y, Parameters.MaxVelocity.y);

            if (!State.WasGroundedLastFrame && State.IsCollidingBelow)
                State.JustGotGrounded = true;
        }
		
	}

    //角色左右上下射線碰撞偵測
	private void CastRaysToTheSides() 
	{	
		float movementDirection=1;	
		if ((_newPosition.x < 0) || (_externalForce.x<0))
			movementDirection = -1;
			
		float horizontalRayLength = Mathf.Abs(_speed.x*Time.deltaTime) + _rayBoundsRectangle.width/2 + RayOffset;
		
		Vector2 horizontalRayCastFromBottom=new Vector2(_rayBoundsRectangle.center.x,
		                                          	_rayBoundsRectangle.yMin+_obstacleHeightTolerance);										
		Vector2 horizontalRayCastToTop=new Vector2(	_rayBoundsRectangle.center.x,
		                                        	_rayBoundsRectangle.yMax);				
		
		RaycastHit2D[] hitsStorage = new RaycastHit2D[NumberOfHorizontalRays];	
			
			
		for (int i=0; i<NumberOfHorizontalRays;i++)
		{	
			Vector2 rayOriginPoint = Vector2.Lerp(horizontalRayCastFromBottom,horizontalRayCastToTop,(float)i/(float)(NumberOfHorizontalRays-1));
			
			if ( State.WasGroundedLastFrame && i == 0 )			
				hitsStorage[i] = GameTools.GameRaycast (rayOriginPoint,movementDirection*Vector2.right,horizontalRayLength,PlatformMask,true,Color.red);	
			else
				hitsStorage[i] = GameTools.GameRaycast(rayOriginPoint,movementDirection*Vector2.right,horizontalRayLength,PlatformMask & ~EdgeColliderPlatformMask,true,Color.red);			
			
			if (hitsStorage[i].distance >0)
			{		
				float hitAngle = Mathf.Abs(Vector2.Angle(hitsStorage[i].normal, Vector2.up));								
								
				if (hitAngle > Parameters.MaximumSlopeAngle)
				{														
					if (movementDirection < 0)		
						State.IsCollidingLeft=true;
					else
						State.IsCollidingRight=true;						
					
					State.SlopeAngleOK=false;
					
					if (movementDirection<=0)
					{
						_newPosition.x = -Mathf.Abs(hitsStorage[i].point.x - horizontalRayCastFromBottom.x) 
											+ _rayBoundsRectangle.width/2 
											+ RayOffset;
					}
					else
					{
						_newPosition.x = Mathf.Abs(hitsStorage[i].point.x - horizontalRayCastFromBottom.x) 
							- _rayBoundsRectangle.width/2 
								- RayOffset;						
					}					
					
					_speed = new Vector2(0, _speed.y);
					break;
				}
			}						
		}	
	}

	private void CastRaysBelow()
    {
		if (_newPosition.y < -_smallValue)
		{
			State.IsFalling=true;
		}
        else
        {
            State.IsFalling = false;
        }

        if ((Parameters.Gravity > 0) && (!State.IsFalling))
            return;
        
		float rayLength = _rayBoundsRectangle.height/2 + RayOffset ; 		
		if (_newPosition.y<0)
		{
			rayLength+=Mathf.Abs(_newPosition.y);
		}
		
						
		Vector2 verticalRayCastFromLeft=new Vector2(_rayBoundsRectangle.xMin+_newPosition.x,
		                                            _rayBoundsRectangle.center.y+RayOffset);	
		Vector2 verticalRayCastToRight=new Vector2(	_rayBoundsRectangle.xMax+_newPosition.x,
		                                           _rayBoundsRectangle.center.y+RayOffset);					
	
		RaycastHit2D[] hitsStorage = new RaycastHit2D[NumberOfVerticalRays];
		float smallestDistance=_largeValue; 
		int smallestDistanceIndex=0; 						
		bool hitConnected=false; 		
		
		for (int i=0; i<NumberOfVerticalRays;i++)
		{			
			Vector2 rayOriginPoint = Vector2.Lerp(verticalRayCastFromLeft,verticalRayCastToRight,(float)i/(float)(NumberOfVerticalRays-1));
			
			if ((_newPosition.y>0) && (!State.WasGroundedLastFrame))
				hitsStorage[i] = GameTools.GameRaycast(rayOriginPoint,-Vector2.up,rayLength,PlatformMask & ~EdgeColliderPlatformMask,true,Color.blue);	
			else
				hitsStorage[i] = GameTools.GameRaycast(rayOriginPoint,-Vector2.up,rayLength,PlatformMask,true,Color.blue);					
						
			if ((Mathf.Abs(hitsStorage[smallestDistanceIndex].point.y - verticalRayCastFromLeft.y)) <  _smallValue)
			{
				break;
			}		
									
			if (hitsStorage[i])
			{
				hitConnected=true;
				if (hitsStorage[i].distance<smallestDistance)
				{
					smallestDistanceIndex=i;
					smallestDistance = hitsStorage[i].distance;
				}
			}								
		}
		if (hitConnected)
		{
			State.IsFalling=false;			
			State.IsCollidingBelow=true;
					
			_newPosition.y = -Mathf.Abs(hitsStorage[smallestDistanceIndex].point.y - verticalRayCastFromLeft.y) 
								+ _rayBoundsRectangle.height/2 
								+ RayOffset;
								
            if (_externalForce.y>0)
			{
				_newPosition.y += _speed.y * Time.deltaTime;
                State.IsCollidingBelow = false;
            }

            if (!State.WasGroundedLastFrame && _speed.y>0)
            {
                _newPosition.y += _speed.y * Time.deltaTime;
            }
			
			if (Mathf.Abs(_newPosition.y)<_smallValue)
				_newPosition.y = 0;

			StandingOn=hitsStorage[smallestDistanceIndex].collider.gameObject;
			PathFollow movingPlatform = hitsStorage[smallestDistanceIndex].collider.GetComponent<PathFollow>();
			if (movingPlatform!=null)
				_transform.Translate(movingPlatform.CurrentSpeed*Time.deltaTime);
		}
		else
		{
			State.IsCollidingBelow=false;
		}						
	}

	private void CastRaysAbove()
	{
		
		float rayLength = State.IsGrounded?RayOffset : _newPosition.y*Time.deltaTime;
		rayLength+=_rayBoundsRectangle.height/2;
		
		bool hitConnected=false; 
		int hitConnectedIndex=0; 
		
		Vector2 verticalRayCastStart=new Vector2(_rayBoundsRectangle.xMin+_newPosition.x,
		                                            _rayBoundsRectangle.center.y);	
		Vector2 verticalRayCastEnd=new Vector2(	_rayBoundsRectangle.xMax+_newPosition.x,
		                                           _rayBoundsRectangle.center.y);	
		                                           
		RaycastHit2D[] hitsStorage = new RaycastHit2D[NumberOfVerticalRays];
		
		for (int i=0; i<NumberOfVerticalRays;i++)
		{							
			Vector2 rayOriginPoint = Vector2.Lerp(verticalRayCastStart,verticalRayCastEnd,(float)i/(float)(NumberOfVerticalRays-1));
			hitsStorage[i] = GameTools.GameRaycast (rayOriginPoint,Vector2.up,rayLength,PlatformMask & ~EdgeColliderPlatformMask,true,Color.green);	
			
			if (hitsStorage[i])
			{
				hitConnected=true;
				hitConnectedIndex=i;
				break;
			}
		}	
		
		if (hitConnected)
		{
			_speed.y=0;
			_newPosition.y = hitsStorage[hitConnectedIndex].distance - _rayBoundsRectangle.height/2   ;
			State.IsCollidingAbove=true;
		}	
	}
	
	public void SetRaysParameters() 
	{		
	
		_rayBoundsRectangle = new Rect(_boxCollider.bounds.min.x,
		                               _boxCollider.bounds.min.y,
		                               _boxCollider.bounds.size.x,
		                               _boxCollider.bounds.size.y);	
		
		Debug.DrawLine(new Vector2(_rayBoundsRectangle.center.x,_rayBoundsRectangle.yMin),new Vector2(_rayBoundsRectangle.center.x,_rayBoundsRectangle.yMax));  
		Debug.DrawLine(new Vector2(_rayBoundsRectangle.xMin,_rayBoundsRectangle.center.y),new Vector2(_rayBoundsRectangle.xMax,_rayBoundsRectangle.center.y));
		
		
		
	}
	

	public IEnumerator DisableCollisions(float duration)
	{
		CollisionsOff();
		yield return new WaitForSeconds (duration);
		CollisionsOn();
	}
	
	public void CollisionsOn()
	{
		PlatformMask=_platformMaskSave;
		PlatformMask |= EdgeColliderPlatformMask;
		PlatformMask |= MovingPlatformMask;
	}
	
	public void CollisionsOff()
	{
		PlatformMask=0;
	}

    public void ResetParameters()
    {
        _overrideParameters = DefaultParameters;
    }
    
    public void SlowFall(float factor)
    {
    	_fallSlowFactor=factor;
    }
	
	public void OnTriggerEnter2D(Collider2D collider)
	{

		CharacterBehaviorControllerPhysicsVolume2D parameters = collider.gameObject.GetComponent<CharacterBehaviorControllerPhysicsVolume2D>();
		if (parameters == null)
			return;
		_overrideParameters = parameters.ControllerParameters;
	}	

	public void OnTriggerStay2D( Collider2D collider )
	{
	}	

	public void OnTriggerExit2D(Collider2D collider)
	{
        CharacterBehaviorControllerPhysicsVolume2D parameters = collider.gameObject.GetComponent<CharacterBehaviorControllerPhysicsVolume2D>();
		if (parameters == null)
			return;

		_overrideParameters = null;
	}
	
	
}
