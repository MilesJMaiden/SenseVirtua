using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TranslationTextUI : MonoBehaviour
{
    TMP_Text text;

    public string key;
    void Awake()
    {
        text = GetComponent<TMP_Text>();
    }
    private void Start()
    {
        TranslationMgr.instance.AddListener(ChangedLanguage);

    }

    void ChangedLanguage()
    {
        text.text = TranslationMgr.instance.GetTranslationText(key);
    }

}
