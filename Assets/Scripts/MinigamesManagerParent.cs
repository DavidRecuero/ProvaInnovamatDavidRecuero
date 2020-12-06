using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class MinigamesManagerParent : MonoBehaviour
{
    [Header ("Question & Answer")]
    public GameObject Question;
    public GameObject Answers;                              //GameObject that contains all the AnswerButtons
    [SerializeField] protected Button[] AnswerButtons;

    [Header("Counters")]
    [SerializeField] protected Text Correct;
    [SerializeField] protected Text Wrong;
                     protected int _mistakes = 0;
                     protected int _corrects = 0;

    [Header ("Animations")]
    [SerializeField] protected AnimationClip QuestionAnimation;
    [SerializeField] protected AnimationClip AnswerOutAnimation;
    [SerializeField] protected AnimationClip AnswerInAnimation;
                     protected float currentTime;

    [Header("Buttons Colours")]
    [SerializeField] protected Color CorrectAnswerColor;
    [SerializeField] protected Color WrongAnswerColor;
                     protected Color _standardAnswerColor;

    [HideInInspector] public FiniteStateMachine finiteStateMachine = new FiniteStateMachine();

    public virtual void Start()
    {
        Question.SetActive(false);
        Answers.SetActive(false);

        Wrong.text = _mistakes.ToString();
        Correct.text = _corrects.ToString();

        _standardAnswerColor = AnswerButtons[0].GetComponent<Image>().color;

        currentTime = 0f;
    }

    //Restarts the initial scene state whithout reopen it/recall Start()
    protected virtual void Restart()
    {
        Question.SetActive(false);
        Answers.SetActive(false);

        currentTime = 0f;
    }

    protected virtual void Update()
    {
        finiteStateMachine.Update();

        currentTime += Time.deltaTime;
    }

    //Finds a random key from the Dictionary dict
    protected int RandomDictionaryElement(Dictionary<int, string> dict)
    {
        List<int> DictionaryKeys = new List<int>();

        foreach (var entry in dict)
        {
            DictionaryKeys.Add(entry.Key);
        }

        var random = new System.Random();
        var index = random.Next(DictionaryKeys.Count);

        return DictionaryKeys[index];
    }

    //True when the question animation has ended
    public bool QuestionShowed()
    {
        return (QuestionAnimation.length <= currentTime) ? true : false;
    }

    //Called when the player choose the correct answer
    protected void OnCorrectAnswer()
    {
        _corrects++;
        Correct.text = _corrects.ToString();
    }

    //Called when the player choose a wrong answer
    protected void OnWrongAnswer()
    {
        _mistakes++;
        Wrong.text = _mistakes.ToString();
    }

    //Manage the state of the game when the correct answer is shown (found by the player or after lose all his opportunities to do it)
    protected void CorrectAnswerFound()
    {
        this.Restart();

        finiteStateMachine.ChangeState(new QuestionState(this));
    }

    //Set all buttons as interactables
    protected void SetButtonsInteractables()
    {
        foreach (Button button in AnswerButtons)
            button.interactable = true;
    }
}
