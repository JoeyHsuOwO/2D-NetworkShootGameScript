using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//角色傷害接收元件
public interface CanTakeDamage
{
    [Command]
	void CmdTakeDamage(int damage,GameObject instigator);
}
