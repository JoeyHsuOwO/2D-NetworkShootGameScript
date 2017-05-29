using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//角色狀態欄UI設定
public class GUIManager : MonoBehaviour 
{

	public GameObject HUD;
	public GameObject JetPackBar;

	public void SetHUDActive(bool state)
	{
		HUD.SetActive(state);
	}

	public void SetJetpackBar(bool state)
	{
		JetPackBar.SetActive(state);
	}


}
