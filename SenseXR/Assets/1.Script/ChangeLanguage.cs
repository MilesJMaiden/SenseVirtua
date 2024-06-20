using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeLanguage : MonoBehaviour
{
    public Language language;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickedBtn);
    }
    public void OnClickedBtn()
    {
        TranslationMgr.instance.ChangeLanguage(language);
    }
}
