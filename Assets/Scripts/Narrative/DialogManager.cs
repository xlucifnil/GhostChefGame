using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using Unity.VisualScripting;
using JetBrains.Annotations;

public class DialogueManager : MonoBehaviour
{
    public TextAsset inkFile;
    public GameObject Canvas;
    public GameObject TextBacking;
    public Text textBox;
    public GameObject button;
    public GameObject optionPanel;
    public bool isTalking = false;
    public float textBoxXShift;
    public float textBoxYShift;
    public float buttonYShift;
    public float firstButtonYAdjust;

    static Story story;
    Text message;
    List<string> tags;
    static Choice choiceSelected;


    // Start is called before the first frame update
    void Start()
    {
        Canvas.SetActive(true);
        //textBox.SetActive(true);
        story = new Story(inkFile.text);
        message = textBox;
        tags = new List<string>();
        choiceSelected = null;
        //Is there more to the story
        if (story.canContinue)
        {
            AdvanceDialogue();

            //Are there any choices
            if (story.currentChoices.Count != 0)
            {
                StartCoroutine(ShowChoices());
            }
        }
        else
        {
            FinishDialogue();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (story.canContinue)
        {
            AdvanceDialogue();
        }
            if (Input.GetMouseButtonDown(0))
        {
            //Is there more to the story?
            if (story.canContinue)
            {
                AdvanceDialogue();

                //Are there any choices?
                if (story.currentChoices.Count != 0)
                {
                    StartCoroutine(ShowChoices());
                }
            }
            else
            {
                FinishDialogue();
            }
        }
    }

    // Finished the Story (Dialogue)
    private void FinishDialogue()
    {
        Debug.Log("End of Dialogue!");
    }

    //Move through the Dialogue
    void AdvanceDialogue()
    {
        string currentSentence = story.Continue();
        ParseTags();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        message.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            message.text += letter;
            yield return null;
        }
        //Used to do animation changes for when we get there but isn't needed for now.
        /*
        CharacterScript tempSpeaker = GameObject.FindObjectOfType<CharacterScript>();
        if(tempSpeaker.isTalking)
        {
            SetAnimation("idle");
        }
        yield return null;
         
       */
    }

    //Create and then show the choices on the screen until one got selected
    IEnumerator ShowChoices()
    {
        Debug.Log("Choices must be made");
        List<Choice> choices = story.currentChoices;

        for (int i = 0; i < choices.Count; i++)
        {
            GameObject temp = Instantiate(button, optionPanel.transform);
            temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y + firstButtonYAdjust - i * buttonYShift, temp.transform.position.z);
            temp.transform.GetChild(0).GetComponent<Text>().text = choices[i].text;
            temp.AddComponent<Selectable>();
            //temp.GetComponent<Selectable>().element = choices[i];
            //temp.GetComponent<Button>().onClick.AddListener(() => { temp.GetComponent<Selectable>().Decide(); });
        }

        optionPanel.SetActive(true);

        yield return new WaitUntil(() => { return choiceSelected != null; });

        AdvanceFromDecision();
    }

    //Tells the story which branch to go to
    public static void SetDecision(object element)
    {
        choiceSelected = (Choice)element;
        story.ChooseChoiceIndex(choiceSelected.index);
    }

    void AdvanceFromDecision()
    {
        optionPanel.SetActive(false);
        for (int i = 0; i < optionPanel.transform.childCount; i++)
        {
            Destroy(optionPanel.transform.GetChild(i).gameObject);
        }
        choiceSelected = null;
        AdvanceDialogue();
    }

    /*** Tag Parser ***/
    // In Inky, you can use tags to cue stuff in the game.
    // This is what does that.
    void ParseTags()
    {
        tags = story.currentTags;
        foreach (string t in tags)
        {
            string prefix = t.Split(' ')[0];
            string param = t.Split(" ")[1];

            //For when we want to do wierd things with the text and change the characters stance in the talking bits.
            switch (prefix.ToLower())
            {
                case "anim":
                    //SetAnimation(param);
                    break;
                case "color":
                    // SetTextColor(param);
                    break;
                //case "talking":
                //    SetSpeaker(param);
                //    break;
                case "end":
                    EndScript(param);
                    break;
            }
        }
    }

    //void SetSpeaker(string character)
    //{
    //    speaker = GameObject.Find(character);
    //}
    void EndScript(string param)
    {
        TextBacking.SetActive(false);
        //textBox.SetActive(false);
        Debug.Log(param);
        Destroy(gameObject);
    }

}

