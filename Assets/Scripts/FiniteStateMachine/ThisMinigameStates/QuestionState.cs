using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionState : IFSMState
{
    MinigamesManagerParent owner;

    public QuestionState(MinigamesManagerParent owner) { this.owner = owner; }

    public void Enter()
    {
        owner.Question.SetActive(true);
    }

    public void Execute()
    {
        if (owner.QuestionShowed())
            owner.finiteStateMachine.ChangeState(new AnswerState(owner));
    }

    public void Exit()
    {
        owner.Question.SetActive(false);
    }
}
