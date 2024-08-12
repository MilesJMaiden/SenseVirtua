using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VRDebuglogger : MonoBehaviour
{
    #region Fields

    // Reference to the TMP_Text component where logs will be displayed
    public TMP_Text debugText;

    // List to store log messages
    private List<string> logMessages = new List<string>();

    // Maximum number of log messages to retain
    public int maxLogMessages = 20;

    // String to store the combined output of all log messages
    string output = "";

    // String to store the stack trace of the last log
    string stack = "";
    #endregion

    #region Unity Methods

    /// <summary>
    /// Subscribes to the logMessageReceived event when the script is enabled.
    /// </summary>
    private void OnEnable()
    {
        // Add the HandleLog method to the logMessageReceived event and log that logger is enabled
        Application.logMessageReceived += HandleLog;
        Debug.Log("Log Enabled!");
    }

    /// <summary>
    /// Unsubscribes from the logMessageReceived event when the script is disabled.
    /// </summary>
    private void OnDisable()
    {
        // Remove the HandleLog method from the logMessageReceived event and clear log messages
        Application.logMessageReceived -= HandleLog; 
        ClearLog();
    }

    /// <summary>
    /// Updates the displayed text in the TMP_Text component on the GUI.
    /// </summary>
    private void OnGUI()
    {
        // Check if the debugText component is assigned
        if (debugText != null) 
        {
            // Update the text with the current log output
            debugText.text = output;
        }
    }
    #endregion

    #region Custom Methods

    /// <summary>
    /// Handles the log messages by adding them to the list and updating the output string.
    /// </summary>
    /// <param name="logString">The log message.</param>
    /// <param name="stackTrace">The stack trace associated with the log message.</param>
    /// <param name="type">The type of log message (Error, Warning, etc.).</param>
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        // Add the new log message to the list
        logMessages.Add(logString);

        // Check if the list exceeds the maximum count and the remove the oldest log message
        if (logMessages.Count > maxLogMessages) 
        {
            logMessages.RemoveAt(0);
        }
        
        // Update the output string with all current log messages and update stack with latest one
        output = string.Join("\n", logMessages); 
        stack = stackTrace;
    }

    /// <summary>
    /// Clears all the log messages from the list and resets the output string.
    /// </summary>
    public void ClearLog()
    {
        // Clear the list of log messages and reset output string
        logMessages.Clear(); 
        output = "";
    }
    #endregion
}
