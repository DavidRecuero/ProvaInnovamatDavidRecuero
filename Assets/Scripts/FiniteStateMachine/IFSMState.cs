using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Finite State Machine states interfaces

public interface IFSMState
{
    void Enter();
    void Execute();
    void Exit();
}
