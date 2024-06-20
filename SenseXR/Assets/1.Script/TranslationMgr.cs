using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TranslationMgr : MonoBehaviour
{
    public static TranslationMgr instance;
    public Language language;
    public TranslationText[] translationTexts;
    public TranslationVoice[] translationVoices;

    private void Awake()
    {
        language = Language.Chinese;
        instance = this;

    }

    public string GetTranslationText(string key)
    {
        for (int i = 0; i < translationTexts.Length; i++)
        {
            if (translationTexts[i].key == key)
            {
                if (language == Language.English)
                {
                    return translationTexts[i].english;
                }
                else if (language == Language.Chinese)
                {
                    return translationTexts[i].chinese;
                }
            }
        }
        return null;
    }
    public AudioClip GetTranslationVoice(string key)
    {
        for (int i = 0; i < translationVoices.Length; i++)
        {
            if (translationVoices[i].key == key)
            {
                if (language == Language.English)
                {
                    return translationVoices[i].english;
                }
                else if (language == Language.Chinese)
                {
                    return translationVoices[i].chinese;
                }
            }
        }
        return null;
    }

    public void ChangeLanguage(Language l)
    {
        language = l;

        for (int i = 0; i < list.Count; i++)
        {
            list[i].Invoke();
        }
    }

    List<Action> list = new List<Action>(); 
    public void AddListener(Action action)
    {
        list.Add(action);
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