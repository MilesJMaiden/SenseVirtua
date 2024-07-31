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
    public class ContextMapDemo : MonoBehaviour
    {
        private Text _contextMapText;
        public ComposerService composer;
        public string contextData;

        void Awake()
        {
            _contextMapText = GetComponent<Text>();
            UpdateContextMapGui();
        }

        public void HandleResponse(ComposerSessionData sessionData)
        {
            // Check context map
            if (sessionData.contextMap == null || sessionData.contextMap.Data == null)
            {
                VLog.E("No Context Map");
                return;
            }

            UpdateContextMapGui();

            sessionData.contextMap.SetData(
                "other_color", $"black");
        }

        private void UpdateContextMapGui()
        {
            _contextMapText.text = "Context map = " + composer.CurrentContextMap.GetJson();
        }

        public void SetContextMap()
        {
            composer.CurrentContextMap.SetData("updated_info",
                string.IsNullOrEmpty(contextData) ? "a button has been pushed" : contextData);
            UpdateContextMapGui();
        }

        public void ClearContextMap()
        {
            composer.CurrentContextMap.ClearAllNonReservedData();
            UpdateContextMapGui();
        }

        public void SendContextMap()
        {
            composer.SendContextMapEvent();
        }

        public void OnContextMapChange()
        {
            Debug.Log("Context map has changed to:" + composer.CurrentContextMap.GetJson());
        }

    }
}
