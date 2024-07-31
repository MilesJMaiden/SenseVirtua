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
using Meta.WitAi.Json;
using Meta.WitAi.Data.Entities;
using UnityEngine.Scripting;

namespace Meta.WitAi.Composer.Samples
{
    public class SimpleColorChanger : MonoBehaviour
    {
        [SerializeField] private Renderer _renderer;
        [SerializeField] private string entityID = "color";

        /// <summary>
        /// Set color action
        /// </summary>
        /// <param name="sessionData">Composer data, persisted throughout the session.</param>
        public void SetColorAction(ComposerSessionData sessionData)
        {
            // Check context map
            var contextMap = sessionData.contextMap;
            if (contextMap == null)
            {
                VLog.E("Set Color Action Failed - No Context Map");
                return;
            }

            // Check color array from context map
            var colorArray = contextMap.GetData<WitResponseNode>(entityID)?.AsArray;
            if (colorArray == null)
            {
                VLog.E($"Set Color Action Failed - Context map does not contain '{entityID}'\n{contextMap.Data?.ToString() ?? "Null"}");
                return;
            }

            // Get color value from entity
            var colorName = colorArray?.Count > 0 ? colorArray[0].AsWitEntity()?.value : null;
            if (string.IsNullOrEmpty(colorName))
            {
                VLog.E($"Set Color Action Failed - Could not determine '{entityID}' value");
                return;
            }

            // Get & set color
            var color = GetColor(colorName);
            SetColor(color);
        }

        /// <summary>
        /// Retrieves the first color in the provided entity
        /// </summary>
        /// <param name="colorEntity">entity data, regardless of original parsing source</param>
        /// <returns>The color in the data, or the original material color if none found</returns>
        private Color GetColor(string colorName)
        {
            if (!ColorUtility.TryParseHtmlString(colorName, out var color))
            {
                VLog.E($"Set Color Action Failed - Could not parse color\nName: {colorName}");
                return _renderer.material.color;
            }
            return color;
        }

        /// <summary>
        /// Set the specified color
        /// </summary>
        public void SetColor(Color newColor)
        {
            _renderer.material.color = newColor;
        }
    }
}
