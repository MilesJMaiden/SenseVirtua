using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using UnityEngine.Events;
using Oculus.Voice.Dictation;
public class ChatGPTMgr : MonoBehaviour
{
    public OnResponseEvent OnResponse;
    
    [System.Serializable]
    public class OnResponseEvent : UnityEvent<string> { }

    [TextArea(5,20)]
    public string personality;
    [TextArea(5, 20)]
    public string scene;
    public int maxResponseWordLimit = 15;

    public AppDictationExperience voiceToText;

    public string GetInstructions()
    {
        string instructions = "You are a buddha and will answer to the message the player ask you. \n" +
            "You must reply to the player message only using the information from your Personnality and the Scene that are in Unity. \n"+
            "Do not invent or creat response that are not mentionned in these information. \n" +
            "Do not break character or mention you are an AI. \n" +
            
            "You must answer in less than " + maxResponseWordLimit + "words. \n" +

            "Here is the information about your Personnality : \n" +
            personality + "\n" +

            "Here is the information about the Scene around you : \n" +
            scene + "\n" +

            "Here is the message of the player : \n";

        return instructions;

    }


    private OpenAIApi openAI = new OpenAIApi();
    private List<ChatMessage> messages = new List<ChatMessage>();
    public async void AskChatGPT(string newText)
    {
        ChatMessage newMessage = new ChatMessage();
        newMessage.Content = GetInstructions() + newText;
        newMessage.Role = "user";

        messages.Add(newMessage);

        CreateChatCompletionRequest request = new CreateChatCompletionRequest();
        request.Messages = messages;
        request.Model = "gpt-4o";
        //request.Model = "gpt-3.5-turbo";

        var response = await openAI.CreateChatCompletion(request);

        if (response.Choices != null && response.Choices.Count > 0)
        {
            var chatResponse = response.Choices[0].Message;
            messages.Add(chatResponse);

            OnResponse.Invoke(chatResponse.Content);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        voiceToText.DictationEvents.OnFullTranscription.AddListener(AskChatGPT);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        { 
            voiceToText.Activate();
        }

    }
}
