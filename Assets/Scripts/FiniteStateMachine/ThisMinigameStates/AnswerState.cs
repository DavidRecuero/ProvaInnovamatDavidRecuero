using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//State to choose the anser in this games 
public class AnswerState : IFSMState
{
    MinigamesManagerParent owner;

    public AnswerState(MinigamesManagerParent owner) { this.owner = owner;}

    public void Enter()
    {
        owner.Answers.SetActive(true);
    }

    public void Execute()
    {
    }

    public void Exit()
    {
        owner.Answers.SetActive(false);
    }
}
