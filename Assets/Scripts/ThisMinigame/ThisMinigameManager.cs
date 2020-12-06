using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Class to manage some useful parameters of the answer buttons
class ThisMinigameButton
{
    public Button Button;
    public bool CorrectAnswer;
    public bool Pressed;

    public ThisMinigameButton(Button _button, bool _correctAnswer)
    {
        Button = _button;
        CorrectAnswer = _correctAnswer;
        Pressed = false;
    }
}

public class ThisMinigameManager : MinigamesManagerParent
{
    //Scriptable Object with the numbers and their transcription
    [Header ("Numbers/Word Scriptable Object")]
    [SerializeField] protected NumberAndWordScriptableObject NumberAndWordSO;

    //Choosen number the player should find
    private int QuestionNumber; 

    //List with the answers buttons 
    private List<ThisMinigameButton> ThisMinigameButtons = new List<ThisMinigameButton>();
        
    public override void Start()
    {
        base.Start();

        QuestionNumber = RandomDictionaryElement(NumberAndWordSO.dictionary);
        Question.GetComponent<TextMeshProUGUI>().text = NumberAndWordSO.dictionary[QuestionNumber];

        ThisMinigameAnswerGenerator();

        finiteStateMachine.ChangeState(new QuestionState(this));
    }

    protected override void Restart()
    {
        base.Restart();

        foreach (ThisMinigameButton _thisGameButton in ThisMinigameButtons)
            _thisGameButton.Button.interactable = true;

        ThisMinigameButtons.Clear();
        ThisMinigameButtons = new List<ThisMinigameButton>();

        QuestionNumber = RandomDictionaryElement(NumberAndWordSO.dictionary);
        Question.GetComponent<TextMeshProUGUI>().text = NumberAndWordSO.dictionary[QuestionNumber];

        ThisMinigameAnswerGenerator();

        finiteStateMachine.ChangeState(new QuestionState(this));
    }

    //Generates the buttons and their answer options. Doesnt repeat options.
    void ThisMinigameAnswerGenerator()
    {
        var correctAnswer = Random.Range(0, AnswerButtons.Length);  //Selects which of the buttons will contain the correct answer
        var falseAnswer = QuestionNumber;                           //Variable to check that the wrong answer doesnt repeat. Not contains                                            
        int randomWrongNumber;                                      //Variable which will contain the the random number generated to the wrong answers

        foreach (Button button in AnswerButtons)
        {
            if (button == AnswerButtons[correctAnswer])
            {
                var thisMinigameButton = new ThisMinigameButton(button, true);

                button.onClick.AddListener(() => OnCorrectAnswer(thisMinigameButton));
                button.GetComponentInChildren<TextMeshProUGUI>().text = QuestionNumber.ToString();

                ThisMinigameButtons.Add(thisMinigameButton);
            }
            else
            {
                var thisMinigameButton = new ThisMinigameButton(button, false);

                button.onClick.AddListener(() => OnWrongAnswer(thisMinigameButton));

                //The randoWrongNumber value is accepted if its different to the correct answer and to the other 
                do
                    randomWrongNumber = RandomDictionaryElement(NumberAndWordSO.dictionary);
                while (randomWrongNumber == QuestionNumber || randomWrongNumber == falseAnswer);

                falseAnswer = randomWrongNumber;
                button.GetComponentInChildren<TextMeshProUGUI>().text = randomWrongNumber.ToString();

                ThisMinigameButtons.Add(thisMinigameButton);
            }

            //The colour of the button is setted as its default colour
            button.GetComponent<Image>().color = _standardAnswerColor;


            //Buttons not interactables until in animation has ended
            button.interactable = false;
            Invoke("SetButtonsInteractables", AnswerInAnimation.length + QuestionAnimation.length);
        }
    }

    private void OnCorrectAnswer(ThisMinigameButton thisMinigameButton)
    {
        OnCorrectAnswer();  //Parent class funcion

        thisMinigameButton.Button.GetComponent<Image>().color = CorrectAnswerColor;
        thisMinigameButton.Button.GetComponent<Animator>().SetBool("OutAnimation", true);     //Begins the animation to hide the button
        thisMinigameButton.Pressed = true;

        //Hides the wrong answers buttons
        foreach (ThisMinigameButton _thisMinigameButton in ThisMinigameButtons)
        {
            _thisMinigameButton.Button.onClick.RemoveAllListeners();
            _thisMinigameButton.Button.interactable = false;

            if(!_thisMinigameButton.Pressed)
                _thisMinigameButton.Button.GetComponent<Animator>().SetBool("OutAnimation", true);
        }
        
        Invoke("CorrectAnswerFound", AnswerOutAnimation.length);
    }

    private void OnWrongAnswer(ThisMinigameButton thisMinigameButton)
    {
        OnWrongAnswer();    //Parent class funcion

        thisMinigameButton.Button.GetComponent<Animator>().SetBool("OutAnimation", true);
        thisMinigameButton.Pressed = true;

        thisMinigameButton.Button.GetComponent<Image>().color = WrongAnswerColor;
        thisMinigameButton.Button.interactable = false;

        //Checks if its the last chance of the player to find the correct answer
        var secondWrongAnswers = false;
        foreach (ThisMinigameButton _thisMinigameButton in ThisMinigameButtons)
        {
            if (_thisMinigameButton.Button != thisMinigameButton.Button && _thisMinigameButton.Pressed && !_thisMinigameButton.CorrectAnswer)
                secondWrongAnswers = true;
        }
            
        if(secondWrongAnswers)
        {
            foreach (ThisMinigameButton _thisMinigameButton in ThisMinigameButtons)
            {
                _thisMinigameButton.Button.onClick.RemoveAllListeners();
                _thisMinigameButton.Button.interactable = false;

                //Shows and then hides the correct answer
                if (_thisMinigameButton.CorrectAnswer)
                {
                    _thisMinigameButton.Button.GetComponent<Image>().color = CorrectAnswerColor;
                    _thisMinigameButton.Button.GetComponent<Animator>().SetBool("OutAnimation", true);

                    Invoke("CorrectAnswerFound", AnswerOutAnimation.length);
                }
            }
        }
    }
}
