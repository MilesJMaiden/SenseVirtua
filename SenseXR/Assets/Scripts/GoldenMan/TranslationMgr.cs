using System.Collections.Generic;
using UnityEngine;
using System;

public class TranslationMgr : MonoBehaviour
{
    public static TranslationMgr instance;
    public Language language;
    public TranslationText[] translationTexts;
    public TranslationVoice[] translationVoices;

    private List<Action> listeners = new List<Action>();

    private void Awake()
    {
        language = Language.English;
        instance = this;
    }

    public string GetTranslationText(string key)
    {
        foreach (var translation in translationTexts)
        {
            if (translation.key == key)
            {
                return language == Language.English ? translation.english : translation.chinese;
            }
        }
        return null;
    }

    public AudioClip GetTranslationVoice(string key)
    {
        foreach (var translation in translationVoices)
        {
            if (translation.key == key)
            {
                return language == Language.English ? translation.english : translation.chinese;
            }
        }
        return null;
    }

    public void ChangeLanguage(Language lang)
    {
        language = lang;
        foreach (var listener in listeners)
        {
            listener.Invoke();
        }

        UpdateCurrentDialogueAndVoice();
    }

    public void AddListener(Action action)
    {
        listeners.Add(action);
    }

    private void UpdateCurrentDialogueAndVoice()
    {
        Voice[] voices = FindObjectsOfType<Voice>();
        foreach (var voice in voices)
        {
            voice.UpdateDialogueAndVoice();
        }
    }
}

[System.Serializable]
public class TranslationText
{
    public string key;
    public string english;
    public string chinese;
}

[System.Serializable]
public class TranslationVoice
{
    public string key;
    public AudioClip english;
    public AudioClip chinese;
}

public enum Language
{
    English,
    Chinese
}
