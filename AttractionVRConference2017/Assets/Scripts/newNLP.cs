using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrostweepGames.Plugins.GoogleCloud.SpeechRecognition.Examples
{
    public class newNLP : MonoBehaviour
    {
        private bool recognitionOn = false;

        public TextMesh _speechRecognitionResult;
        public TextMesh _speechRecognitionState;

        private GCSpeechRecognition _speechRecognition;

        private Button _startRecordButton,
                       _stopRecordButton;

        private bool _isRuntimeDetectionToggle = false;

        private Dropdown _languageDropdown;

        private InputField _contextPhrases;

        private void Start()
        {
            _speechRecognition = GCSpeechRecognition.Instance;
            _speechRecognition.RecognitionSuccessEvent += RecognitionSuccessEventHandler;
            _speechRecognition.NetworkRequestFailedEvent += SpeechRecognizedFailedEventHandler;
            _speechRecognition.LongRecognitionSuccessEvent += LongRecognitionSuccessEventHandler;

            _speechRecognitionState.color = Color.white;
        }

        private void Update()
        {
             if (Input.GetKeyDown("space")){
                if (recognitionOn==false)
                {
                    _speechRecognitionResult.text = "...";
                    _speechRecognition.StartRecord(_isRuntimeDetectionToggle);
                    recognitionOn = true;
                }
                else
                {
                    //ApplySpeechContextPhrases();
                    _speechRecognitionState.color = Color.yellow;
                    _speechRecognition.StopRecord();
                    recognitionOn = false;
                }
            }
        }

        private void OnDestroy()
        {
            _speechRecognition.RecognitionSuccessEvent -= RecognitionSuccessEventHandler;
            _speechRecognition.NetworkRequestFailedEvent -= SpeechRecognizedFailedEventHandler;
            _speechRecognition.LongRecognitionSuccessEvent -= LongRecognitionSuccessEventHandler;
        }


        private void StartRecordButtonOnClickHandler()
        {
            _startRecordButton.interactable = false;
            _stopRecordButton.interactable = true;
            _speechRecognitionState.color = Color.red;
            _speechRecognitionResult.text = string.Empty;
            _speechRecognition.StartRecord(_isRuntimeDetectionToggle);
        }

        private void StopRecordButtonOnClickHandler()
        {
            ApplySpeechContextPhrases();

            _stopRecordButton.interactable = false;
            _speechRecognitionState.color = Color.yellow;
            _speechRecognition.StopRecord();
        }

        private void LanguageDropdownOnValueChanged(int value)
        {
            _speechRecognition.SetLanguage((Enumerators.LanguageCode)value);
        }

        private void ApplySpeechContextPhrases()
        {
            string[] phrases = _contextPhrases.text.Trim().Split(","[0]);

            if (phrases.Length > 0)
                _speechRecognition.SetContext(new List<string[]>() { phrases });
        }

        private void SpeechRecognizedFailedEventHandler(string obj, long requestIndex)
        {
            Debug.Log("Speech Recognition failed with error: " + obj);
            _speechRecognitionResult.text = "Speech Recognition failed with error: " + obj;

            if (!_isRuntimeDetectionToggle)
            {
                _speechRecognitionState.color = Color.red;
                
            }
        }

        private void RecognitionSuccessEventHandler(RecognitionResponse obj, long requestIndex)
        {
            if (!_isRuntimeDetectionToggle)
            {
                _speechRecognitionState.color = Color.green;
            }

            if (obj != null && obj.results.Length > 0)
            {
                _speechRecognitionResult.text = "Speech Recognition succeeded! Detected Most useful: " + obj.results[0].alternatives[0].transcript;

                string other = "\nDetected alternative: ";

                foreach (var result in obj.results)
                {
                    foreach (var alternative in result.alternatives)
                    {
                        if (obj.results[0].alternatives[0] != alternative)
                            other += alternative.transcript + ", ";
                    }
                }

                _speechRecognitionResult.text += other;
            }
            else
            {
                _speechRecognitionResult.text = "Speech Recognition succeeded! Words are no detected.";
            }
        }

        private void LongRecognitionSuccessEventHandler(OperationResponse operation, long index)
        {
            if (!_isRuntimeDetectionToggle)
            {
                _startRecordButton.interactable = true;
                _speechRecognitionState.color = Color.green;
            }

            if (operation != null && operation.response.results.Length > 0)
            {
                _speechRecognitionResult.text = "Long Speech Recognition succeeded! Detected Most useful: " + operation.response.results[0].alternatives[0].transcript;

                string other = "\nDetected alternative: ";

                foreach (var result in operation.response.results)
                {
                    foreach (var alternative in result.alternatives)
                    {
                        if (operation.response.results[0].alternatives[0] != alternative)
                            other += alternative.transcript + ", ";
                    }
                }

                _speechRecognitionResult.text += other;
                _speechRecognitionResult.text += "\nTime for the recognition: " +
                    (operation.metadata.lastUpdateTime - operation.metadata.startTime).TotalSeconds + "s";
            }
            else
            {
                _speechRecognitionResult.text = "Speech Recognition succeeded! Words are no detected.";
            }
        }
    }
}