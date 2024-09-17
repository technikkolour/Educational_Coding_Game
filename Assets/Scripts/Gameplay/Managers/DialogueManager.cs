using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public Dictionary<string, List<List<string>>> Dialogues = new()
    {
        {"Variable", new(){ new(){ "Hi! I’ve never seen you before, you must be new here!",
                    "My name is Variable, and my brother, Constant, lives just up the road. He hasn’t changed one bit since he was born.",
                    "You think these names are unusual?Everyone here has names like that, we love computer science so much!",
                    "You probably caught on already, but variables can change their values, some more often than others. You should write that down in your journal so you don’t forget!",
                    "Anyways, if you follow the road to the end you’ll get to Robot-tropolis. Have a safe trip!" } } },

        {"Constant", new(){ new(){ "Hi! I’m Constant, it seems you’ve already met my brother!",
                    "He said what? That I have been the same since the day I was born? I think that’s much better than always changing!",
                    "You can probably tell that constants never change their values, unlike variables.",
                    "Anyway, you should get going towards the city. But before you set off, you should try looking inside my house, there are a lot of books there that might help you make sense of this world!"} } },

        {"Claire", new(){ new(){ "Hi! Welcome to Robot-tropolis! You’re not from around here, are you?",
                    "Do you remember how you got here? No?",
                    "I will help you if you can answer this one question for me!" },
                          new(){ "Good job, kid!",
                    "I hear that the founder of the city, and the inventor of all of the robots living here, has access to technology capable of anything.",
                    "He is running a robot battle championship right now. I’m sure if you win he will help you get home.",
                    "Good luck!"} } },

        {"Number", new(){ new() {"Hey, kid! My name’s Number and I love numbers!",
                    "No one is that excited about numbers and maths, eh? I disagree! Numbers are all around us and there’s so many of them!",
                    "There are integers and floating point numbers and…",
                    "What do you mean what are those? Kid you’re not the sharpest tool in the shed, are you?",
                    "Integers are whole numbers, like your age. And floats are decimals, like your height (measured in meters, of course)!",
                    "Now that I’ve told you all of this, how about you solve this one riddle for me!"} } },

        {"Conditional", new(){ new() { "Hello! If you’re new here I would love to get to know you! My name is Conditional!",
                    "I know, it’s such an interesting name! I don’t know what I would have done if we didn’t have conditional statements in programming…",
                    "You don’t know what they are? Well, we use conditional statements to express how our actions change depending on certain conditions.",
                    "For example, if it is sunny today, I will go to the park.",
                    "My favourite conditional function is actually “if”! I love the word itself, I use it so much when speaking, I’m not sure if you’ve noticed!",
                    "Well, I should get going! If I didn’t have so much to do I would have sat here and chatted for longer. See you!"} } },

        {"Rob", new(){ new(){ "Hi!! Are you here for the Robot Academy? You don’t need to do anything special to enrol, just enter the school!",
                    "Where is it? Oh, it’s right behind me, just walk straight on." } } },

        {"April", new(){ new(){ "Yo! How's it going? I've never seen you before!",
                    "I don't really like school and studying is really boring, but robots are just so awesome!"} } },

        {"Maria", new(){ new(){ "Hmm... You didn't pass the test. Let's try again." },
                         new() { "" } } },

        {"Very Upset Robot", new(){ new(){ "Hello! I am a robot! Beep!",
                    "I apologise, but I cannot let you move past me. Beep!",
                    "Please do not open my control panel and change my code! Bee…" },
                                    new() { "Okay! You can go past now!" } } },

        {"Function", new(){ new(){ "Hey! I’ve never seen ya before, are ya new here? Name’s Function, nice to meet ya!",
                    "I get everything done in this city, ya just gotta give me a call and I’m there! But ya must tell me what ya want me to do beforehand, otherwise I can’t help!",
                    "At the moment I’m guarding the robot warehouse and I can’t let ya in without a robot licence!",
                    "Sorry kid!" },
                            new() { "I get everything done in this city, ya just gotta give me a call and I’m there! But ya must tell me what ya want me to do beforehand, otherwise I can’t help!",
                    "At the moment I’m guarding the robot warehouse and I can’t let ya in without a robot licence!",
                    "I see ya got one! Step right in kid!" }} },

        {"William", new(){ new(){ "Kid, what I saw just now was quite the feat, congratulations!",
                    "What would you like as a prize for winning?",
                    "You’d like to go home? Say no more!"} } }
    };

    public Dictionary<int, string> BookcaseInformation = new() {
        { 1, "Strings are used to store text. The value can be anything,such as a name, an address, or even a phone number!" },
        { 2, "In programming we may need to produce a message that informs us of the current state or the result of the program. This is done with the use of output statements." },
        { 3, "Integers are used to store whole numbers, like the number of people present in a room." },
        { 4, "Floats are used to store decimals, such as your height." },
        { 5, "If statements are used when we would like the program to perform some actions based on one or more conditions." },
        { 6, "A set of actions is performed as long as a condition is true." },
        { 7, "For loops are used when we would like a set of actions to be performed every time a value is updated. The starting and final values, as well as the step size, are defined." } };

    public Player Player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Display the lines of dialogue;
    public void DisplayLine(string Line)
    {

    }

    // Getters for the lines of dialogue and contents of game;
    // Return the set of lines for the character and the correct dialogue phase;
    public List<string> GetDialogueLines(string Character, int Index)
    {
        return Dialogues[Character][Index];
    }
    // Return bookcase contents depending on index;
    public string GetBookcaseContents(int Index)
    {
        return BookcaseInformation[Index];
    }
}
