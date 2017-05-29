using UnityEngine;
using System.Collections;


//控管角色行為
public class CharacterBehaviorState
{
    public bool CanJump { get; set; }
    public bool CanShoot { get; set; }
    public bool CanMelee { get; set; }
    public bool CanMoveFreely { get; set; }
    public bool CanJetpack { get; set; }
    public int NumberOfJumpsLeft;
    public bool Running { get; set; }
    public bool Crouching { get; set; }
    public bool CrouchingPreviously { get; set; }
    public bool LookingUp { get; set; }
    public bool WallClinging { get; set; }
    public bool Jetpacking { get; set; }
    public bool Firing { get; set; }
    public bool FiringStop { get; set; }
    public bool MeleeAttacking { get; set; }
    public bool LadderColliding { get; set; }
    public bool LadderTopColliding { get; set; }
    public bool LadderClimbing { get; set; }
    public float LadderClimbingSpeed { get; set; }
    public int FiringDirection { get; set; }
    public bool C8763Attack { get; set; }
    public bool IsDead { get; set; }
    public float JetpackFuelDurationLeft { get; set; }


    public void Initialize()
    {
        CanMoveFreely = true;
        CanShoot = true;
        CanMelee = true;
        CanJetpack = true;
        Running = false;
        Crouching = false;
        CrouchingPreviously = false;
        LookingUp = false;
        WallClinging = false;
        Jetpacking = false;
        LadderClimbing = false;
        LadderColliding = false;
        LadderTopColliding = false;
        LadderClimbingSpeed = 0f;
        Firing = false;
        FiringStop = false;
        FiringDirection = 3;
        MeleeAttacking = false;
        C8763Attack = false;
    }
}
