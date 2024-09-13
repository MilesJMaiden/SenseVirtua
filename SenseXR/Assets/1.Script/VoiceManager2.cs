using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Oculus.Voice;
using OpenAI;

public class VoiceManager2 : MonoBehaviour
{
    [Header("Wit Configuration")]
    [SerializeField] private AppVoiceExperience appVoiceExperience;  // Wit.ai voice experience
    [SerializeField] private TextMeshProUGUI transcriptionText;  // Display transcribed text

    [Header("Voice Events")]
    [SerializeField] private UnityEvent<string> completeTranscription;  // Event when transcription is complete

    public GameObject chatGPTManager;  // Reference to ChatGPT manager

    private bool _voiceCommandReady = true;  // Ready to accept voice commands

    private void Start()
    {
        // Activate voice recognition immediately
        ReactivateVoice();
    }

    private void Update()
    {
        // Optional: If you want to activate voice with a specific key
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReactivateVoice();
        }
    }

    private void Awake()
    {
        // Setup voice events listeners
        appVoiceExperience.VoiceEvents.OnRequestCompleted.AddListener(ReactivateVoice);
        appVoiceExperience.VoiceEvents.OnPartialTranscription.AddListener(OnPartialTranscription);
        appVoiceExperience.VoiceEvents.OnFullTranscription.AddListener(OnFullTranscription);

        // Automatically start voice recognition when the app starts
        appVoiceExperience.Activate();
    }

    private void OnDestroy()
    {
        // Cleanup listeners
        appVoiceExperience.VoiceEvents.OnRequestCompleted.RemoveListener(ReactivateVoice);
        appVoiceExperience.VoiceEvents.OnPartialTranscription.RemoveListener(OnPartialTranscription);
        appVoiceExperience.VoiceEvents.OnFullTranscription.RemoveListener(OnFullTranscription);
    }

    /// <summary>
    /// Re-activate voice recognition.
    /// </summary>
    public void ReactivateVoice()
    {
        if (_voiceCommandReady)
        {
            appVoiceExperience.Activate();  // Start voice recognition immediately
            Debug.Log("Voice recognition activated.");
        }
    }

    /// <summary>
    /// Handles partial transcription during voice recognition.
    /// </summary>
    private void OnPartialTranscription(string transcription)
    {
        // Display partial transcription
        transcriptionText.text = transcription;
    }

    /// <summary>
    /// Handles full transcription and sends text to ChatGPT.
    /// </summary>
    private void OnFullTranscription(string transcription)
    {
        // Display full transcription
        transcriptionText.text = transcription;

        // Process transcription with ChatGPT
        SetTranscription(transcription);

        Debug.Log("Transcription complete, processing with GPT...");
    }

    /// <summary>
    /// Set the transcription text and send it to ChatGPT.
    /// </summary>
    private void SetTranscription(string transcription)
    {
        // Access the ChatGPT manager and send the transcription
        ChatGPTMgr chatGPTMgr = chatGPTManager.GetComponent<ChatGPTMgr>();
        if (chatGPTMgr != null)
        {
            chatGPTMgr.SetTextAndRequestResponse(transcription);
        }
        else
        {
            Debug.LogWarning("ChatGPTMgr component not found.");
        }
    }
}
