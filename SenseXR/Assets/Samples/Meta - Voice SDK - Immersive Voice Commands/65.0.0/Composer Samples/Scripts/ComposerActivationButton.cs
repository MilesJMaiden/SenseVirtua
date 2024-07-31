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

using UnityEngine;
using UnityEngine.UI;

namespace Meta.WitAi.Composer.Samples
{
    public class ComposerActivationButton : MonoBehaviour
    {
        public ComposerService composerService;
        public string unavailableText;

        private Text _label;
        private string _originalText;
        private ComposerActivationButton[] _otherButtons;

        /// <summary>
        /// General setup required to use the component.
        /// </summary>
        public void Awake()
        {
            _label = GetComponentInChildren<Text>();
            _originalText = _label.text;
            _otherButtons = FindObjectsOfType<ComposerActivationButton>();
        }

        /// <summary>
        /// Toggles whether to use Composer or not.
        /// </summary>
        public void SetComposerUsage(bool useComposer)
        {
            if (null == composerService) return;
            composerService.RouteVoiceServiceToComposer = useComposer;
        }

        public void UpdateAllButtonsText()
        {
            foreach (var composerBtn in _otherButtons)
            {
                composerBtn.UpdateText();
            }
        }

        /// <summary>
        /// Sets text to be appropriate to the status of the VoiceService
        /// </summary>
        private void UpdateText()
        {
            _label.text = composerService.VoiceService.Active?
                     unavailableText:_originalText;
        }

        public void ToggleVoiceService()
        {
            if (null == composerService) return;
            if (composerService.VoiceService.Active)
            {
                composerService.VoiceService.Deactivate();
            }
            else
            {
                composerService.VoiceService.Activate();
            }
        }

        /// <summary>
        /// A simple logger to show the switching of the composer status
        /// </summary>
        public void LogComposerStatus()
        {
            Debug.Log("Composer Service is " + (composerService.RouteVoiceServiceToComposer ? "active" : "inactive"));
        }
    }
}
