using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//¨¤¦â¥Í©R­ÈUI
public class HealthBar : NetworkBehaviour 
{

	public Transform ForegroundSprite;
    public Transform TankgroundSprite;
	public Color MaxHealthColor = new Color(255/255f, 63/255f, 63/255f);
	public Color MinHealthColor = new Color(64/255f, 137/255f, 255/255f);
    public Color TankHealthColor = new Color(154 / 255f, 153 / 255f, 152 / 255f);
	
	private CharacterBehavior _character;
    private Tank _tank;


	void Start()
	{
        _character = gameObject.GetComponentInParent<CharacterBehavior>();
        if(gameObject.GetComponentInParent<Tank>() != null)
        {
            _tank = _character.GetComponent<Tank>();
        }
        
    }

	public void Update()
	{
		if (_character==null)
			return;
		var healthPercent = _character.Health / (float) _character.BehaviorParameters.MaxHealth;
        var tankHealthPercent = _character.TankHealth / (float)_character.BehaviorParameters.TankMaxHealth;
        ForegroundSprite.localScale = new Vector3(healthPercent,1,1);
        TankgroundSprite.localScale = new Vector3(tankHealthPercent, 0, 0);

        if (_tank != null)
        {
            if (_tank.PushR == true)
            {
                
                TankgroundSprite.localScale = new Vector3(tankHealthPercent, 1, 1);
                ForegroundSprite.localScale = new Vector3(healthPercent, 0, 0);
            }
            else
            {
                
                ForegroundSprite.localScale = new Vector3(healthPercent, 1, 1);
                TankgroundSprite.localScale = new Vector3(tankHealthPercent, 0, 0);
            }
        }
        else
            return;
        
		
	}
	
}
