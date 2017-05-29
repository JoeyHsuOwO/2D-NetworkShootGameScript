using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class playerScript : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] ComponentsToClose;
    public Camera _camera;
    public Camera _UICamera;
    public GameObject HumanCastle;
    public GameObject MonsterCastle;
    public Transform HumanCastlePosition;
    public Transform MonsterCastlePosition;
    Camera SceneCamera;
    [SyncVar]
    public float calltime = 0.5f;
    [SyncVar]
    public int Switchi;
    [SyncVar]
    public int SwitchE;
    [SyncVar]
    public bool CanCallAgain = true;
    [SyncVar]
    public int Switchj;
    [SyncVar]
    public Vector3 FirePos;
    [SyncVar]
    public int SwitchT;
    [SyncVar]
    public float RRRRRTime;
    [SyncVar]
    public int SwitchC;
    [SyncVar]
    public int SwitchW;
    [SyncVar]
    public float TankTime;
    [SyncVar]
    public int SwichC876;

    public bool TankisActive;

    void Start()
    {
        if (isLocalPlayer)
        {
            gameObject.name = "ME";
            SceneCamera = Camera.main;
        }

        if (!isLocalPlayer)
        {
            for (int i = 0; i < ComponentsToClose.Length; i++)
            {
                ComponentsToClose[i].enabled = false;
            }
        }
    }


}
