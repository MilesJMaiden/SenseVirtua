using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IlluminationGame))]
public class IlluminationGameEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        IlluminationGame illuminationGame = (IlluminationGame)target;

        if (GUILayout.Button("Complete All Zones"))
        {
            illuminationGame.CompleteAllZones();
        }

        if (GUILayout.Button("Trigger Completion Light"))
        {
            illuminationGame.TriggerCompletionLight();
        }

        if (GUILayout.Button("Trigger Special Game Object"))
        {
            illuminationGame.TriggerSpecialGameObject();
        }
    }
}
