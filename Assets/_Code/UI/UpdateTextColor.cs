using System;
using System.Linq;
using System.Text.RegularExpressions;
using LSG.Classes;
using TMPro;
using UnityEngine;

namespace LSG.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class UpdateTextColor : MonoBehaviour
    {
        [SerializeField] private WordColor[] wordColors;

        [Header("Matching")]
        [SerializeField] private bool matchWholeWordsOnly = true;
        [SerializeField] private bool caseSensitive = false;

        private TMP_Text _text;
        private string _originalText;

        private void Awake()
        {
            _text = GetComponent<TMP_Text>();
            _originalText = _text.text;
        }

        private void Start()
        {
            ApplyColors();
        }

        public void ApplyColors()
        {
            if (_text == null)
                _text = GetComponent<TMP_Text>();

            if (_text == null || wordColors == null)
                return;

            if (string.IsNullOrEmpty(_originalText))
                _originalText = _text.text;

            string result = _originalText;

            RegexOptions options = RegexOptions.CultureInvariant;

            if (!caseSensitive)
                options |= RegexOptions.IgnoreCase;

            foreach (WordColor wordColor in wordColors
                         .Where(w => w != null && !string.IsNullOrEmpty(w.word))
                         .OrderByDescending(w => w.word.Length))
            {
                string escapedWord = Regex.Escape(wordColor.word);

                string pattern = matchWholeWordsOnly
                    ? $@"(?<![\p{{L}}\p{{N}}_]){escapedWord}(?![\p{{L}}\p{{N}}_])"
                    : escapedWord;

                string hexColor = ColorUtility.ToHtmlStringRGBA(wordColor.color);

                result = Regex.Replace(
                    result,
                    pattern,
                    match => $"<color=#{hexColor}>{match.Value}</color>",
                    options
                );
            }

            _text.richText = true;
            _text.text = result;
        }

        public void SetText(string newText)
        {
            _originalText = newText;
            ApplyColors();
        }
    }
}