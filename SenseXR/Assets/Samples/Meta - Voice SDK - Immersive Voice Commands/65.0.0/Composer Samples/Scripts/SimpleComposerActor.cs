/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using Meta.WitAi.TTS.Data;
using Meta.WitAi.TTS.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Meta.WitAi.Composer.Samples
{
    public class SimpleComposerActor : MonoBehaviour
    {
        [Header("VoiceService Settings")]
        [SerializeField] private VoiceService _voiceService;

        [Header("Speaker Settings")]
        [SerializeField] private TTSSpeaker _speaker;
        [SerializeField] private Text _captionLabel;
        private string _defaultCaption = "";

        [Header("Status Settings")]
        [SerializeField] private Text _statusLabel;
        [SerializeField] private string _statusIdle = "Idle";
        [SerializeField] private string _statusListening = "Listening";
        [SerializeField] private string _statusLoadResponse = "Loading Response";
        [SerializeField] private string _statusLoadAudio = "Loading Audio";
        [SerializeField] private string _statusLoadError = "Loading Error";
        [SerializeField] private string _statusSpeaking = "Speaking";

        // Get default text
        private void Awake()
        {
            _defaultCaption = _captionLabel.text;
        }

        // Add delegates
        private void OnEnable()
        {
            ResetText();
            _voiceService.VoiceEvents.OnStartListening.AddListener(OnListenBegin);
            _voiceService.VoiceEvents.OnStoppedListening.AddListener(OnListenStop);
            _voiceService.VoiceEvents.OnStoppedListeningDueToDeactivation.AddListener(OnListenCancel);
            _voiceService.VoiceEvents.OnStoppedListeningDueToInactivity.AddListener(OnListenCancel);
            _voiceService.VoiceEvents.OnStoppedListeningDueToTimeout.AddListener(OnListenCancel);
            _voiceService.VoiceEvents.OnRequestCompleted.AddListener(OnListenComplete);
            _speaker.Events.OnLoadBegin.AddListener(OnLoadBegin);
            _speaker.Events.OnLoadAbort.AddListener(OnIdle);
            _speaker.Events.OnLoadFailed.AddListener(OnLoadFailed);
            _speaker.Events.OnPlaybackStart.AddListener(OnPlaybackStart);
            _speaker.Events.OnPlaybackCancelled.AddListener(OnPlaybackCancelled);
            _speaker.Events.OnPlaybackComplete.AddListener(OnIdle);
        }

        // Remove delegates
        private void OnDisable()
        {
            _voiceService.VoiceEvents.OnStartListening.RemoveListener(OnListenBegin);
            _voiceService.VoiceEvents.OnStoppedListening.RemoveListener(OnListenStop);
            _voiceService.VoiceEvents.OnStoppedListeningDueToDeactivation.RemoveListener(OnListenCancel);
            _voiceService.VoiceEvents.OnStoppedListeningDueToInactivity.RemoveListener(OnListenCancel);
            _voiceService.VoiceEvents.OnStoppedListeningDueToTimeout.RemoveListener(OnListenCancel);
            _voiceService.VoiceEvents.OnRequestCompleted.RemoveListener(OnListenComplete);
            _speaker.Events.OnLoadBegin.RemoveListener(OnLoadBegin);
            _speaker.Events.OnLoadAbort.RemoveListener(OnIdle);
            _speaker.Events.OnLoadFailed.RemoveListener(OnLoadFailed);
            _speaker.Events.OnPlaybackStart.RemoveListener(OnPlaybackStart);
            _speaker.Events.OnPlaybackCancelled.RemoveListener(OnPlaybackCancelled);
            _speaker.Events.OnPlaybackComplete.RemoveListener(OnIdle);
        }

        // Reset
        private void ResetText()
        {
            _captionLabel.text = _defaultCaption;
            _statusLabel.text = _statusIdle;
        }

        // Listen start
        private void OnListenBegin()
        {
            _statusLabel.text = _statusListening;
        }
        // Listen request begin
        private void OnListenStop()
        {
            _statusLabel.text = _statusLoadResponse;
        }
        // Listen request cancelled
        private void OnListenCancel()
        {
            ResetText();
        }
        // Listen request completion
        private void OnListenComplete()
        {
            if (string.Equals(_statusLabel.text, _statusLoadResponse))
            {
                _statusLabel.text = _statusIdle;
            }
        }

        // Load begin
        private void OnLoadBegin(TTSSpeaker speaker, TTSClipData clipData)
        {
            _statusLabel.text = _statusLoadAudio;
        }
        // Load begin
        private void OnLoadFailed(TTSSpeaker speaker, TTSClipData clipData, string error)
        {
            _statusLabel.text = $"{_statusLoadError} {error}";
        }
        // Speak begin
        private void OnPlaybackStart(TTSSpeaker speaker, TTSClipData clipData)
        {
            _captionLabel.text = clipData?.textToSpeak;
            _statusLabel.text = _statusSpeaking;
        }
        // Playback cancelled
        private void OnPlaybackCancelled(TTSSpeaker speaker, TTSClipData clipData, string error)
        {
            OnIdle(speaker, clipData);
        }
        // Back to idle
        private void OnIdle(TTSSpeaker speaker, TTSClipData clipData)
        {
            _statusLabel.text = _statusIdle;
        }
    }
}
