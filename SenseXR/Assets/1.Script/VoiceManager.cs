using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Oculus.Voice;
using System.Reflection;
using Meta.WitAi.CallbackHandlers;
using OpenAI;

public class VoiceManager : MonoBehaviour
{
    [Header("Wit Configuration")]
    [SerializeField] private AppVoiceExperience appVoiceExperience;
    [SerializeField] private WitResponseMatcher responseMatcher;
    [SerializeField] private TextMeshProUGUI transcriptionText;


    [Header("Voice Events")]
    [SerializeField] private UnityEvent wakeWordDetected;
    [SerializeField] private UnityEvent<string> completeTranscription;

    private bool _voiceCommandReady = false;
    private bool isFirstDeactivationDone = false;

    public GameObject chatGPTManager;
    private void Start()
    {
        if (!isFirstDeactivationDone)
        {
            isFirstDeactivationDone = true;
        }
    }

    private void Update()
    {
        // Optional: Reactivate voice recognition on specific conditions or keys
        ReactivateVoice();

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

        // Setup wake word detection event
        var eventField = typeof(WitResponseMatcher).GetField("onMultiValueEvent", BindingFlags.NonPublic | BindingFlags.Instance);
        if (eventField != null && eventField.GetValue(responseMatcher) is MultiValueEvent onMultiValueEvent)
        {
            onMultiValueEvent.AddListener(WakeWordDetected);
        }

        // Activate voice recognition on app start
        appVoiceExperience.Activate();
    }

    private void OnDestroy()
    {
        // Cleanup listeners
        appVoiceExperience.VoiceEvents.OnRequestCompleted.RemoveListener(ReactivateVoice);
        appVoiceExperience.VoiceEvents.OnPartialTranscription.RemoveListener(OnPartialTranscription);
        appVoiceExperience.VoiceEvents.OnFullTranscription.RemoveListener(OnFullTranscription);

        var eventField = typeof(WitResponseMatcher).GetField("onMultiValueEvent", BindingFlags.NonPublic | BindingFlags.Instance);
        if (eventField != null && eventField.GetValue(responseMatcher) is MultiValueEvent onMultiValueEvent)
        {
            onMultiValueEvent.RemoveListener(WakeWordDetected);
        }
    }

    /// <summary>
    /// Re-activate voice recognition.
    /// </summary>
    public void ReactivateVoice()
    {
        appVoiceExperience.Activate();
        Debug.Log("Voice recognition activated.");
    }

    /// <summary>
    /// Called when a wake word is detected.
    /// </summary>
    private void WakeWordDetected(string[] arg0)
    {
        _voiceCommandReady = true;
        wakeWordDetected?.Invoke();
    }

    /// <summary>
    /// Handles partial transcription during voice recognition.
    /// </summary>
    private void OnPartialTranscription(string transcription)
    {
        if (!_voiceCommandReady) return;
        transcriptionText.text = transcription;
    }

    /// <summary>
    /// Handles full transcription and sends text to ChatGPT.
    /// </summary>
    private void OnFullTranscription(string transcription)
    {
        if (!_voiceCommandReady) return;
        _voiceCommandReady = false;
        completeTranscription?.Invoke(transcription);

        // Spawn the prefab and pass transcription to ChatGPTHandler
        
        SetTranscription(transcription);

        // 비활성화 로직을 첫 번째 대화가 끝난 후 실행
        DeactivateVoiceManager();
        // Disable the manager after processing
        //gameObject.SetActive(false);
        Debug.Log("GPTManager: GameObject has been deactivated.");
    }

    private void SetTranscription(string transcription)
    {

        // Access the ChatGPTHandler from the prefab
        ChatGPTMgr chatGPTMgr = chatGPTManager.GetComponent<ChatGPTMgr>();
        if (chatGPTMgr != null)
        {
            chatGPTMgr.SetTextAndRequestResponse(transcription);
        }
        else
        {
            //Debug.LogWarning("ChatGPTHandler component not found on the instantiated prefab.");
        }
    }
    private void DeactivateVoiceManager()
    {
        // GameObject를 바로 비활성화
        //gameObject.SetActive(false);
        Debug.Log("VoiceManager: GameObject has been deactivated after the first transcription.");
    }
}
