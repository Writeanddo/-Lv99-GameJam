using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Localization;
using UnityEngine.UI;
using System;

public class ChangeLanguageButton : MonoBehaviour
{
    [SerializeField] private string language;

    private LeanLocalization localization;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        localization = FindObjectOfType<LeanLocalization>();
    }

    private void Start()
    {
        button.onClick.AddListener(ChangeLanguage);
    }

    private void ChangeLanguage()
    {
        localization.SetCurrentLanguage(language);
    }
}
