using UnityEngine;

/// <summary>Manages data for persistance between play sessions.</summary>
public class SaveCredentials : MonoBehaviour
{
    #region PLEASE SET THESE VARIABLES IN THE INSPECTOR
    [Header("Speech To Text")]
    [Tooltip("The service URL (optional). This defaults to \"https://stream.watsonplatform.net/speech-to-text/api\"")]
    public string speechToTextServiceUrl = "https://stream.watsonplatform.net/speech-to-text/api";

    [Header("CF Authentication")]
    [Tooltip("The authentication username.")]
    public string speechToTextUsername = "";

    [Tooltip("The authentication password.")]
    public string speechToTextPassword = "";

    [Header("IAM Authentication")]
    [Tooltip("The IAM apikey.")]
    public string speechToTextIamApikey = "";

    [Tooltip("The IAM url used to authenticate the apikey (optional). This defaults to \"https://iam.bluemix.net/identity/token\".")]
    public string speechToTextIamUrl = "https://iam.bluemix.net/identity/token";

    [Header("Watson Assistant")]
    [Tooltip("The service URL (optional). This defaults to \"https://gateway.watsonplatform.net/assistant/api\"")]
    public string assistantServiceUrl = "https://gateway.watsonplatform.net/assistant/api";

    [Tooltip("The workspaceId to run the example.")]
    public string assistantWorkspaceId = "";

    [Tooltip("The version date with which you would like to use the service in the form YYYY-MM-DD. Current is 2018-07-10")]
    public string assistantVersionDate = "";

    [Header("CF Authentication")]
    [Tooltip("The authentication username.")]
    public string assistantUsername = "";

    [Tooltip("The authentication password.")]
    public string assistantPassword = "";

    [Header("IAM Authentication")]
    [Tooltip("The IAM apikey.")]
    public string assistantIamApikey = "";

    [Tooltip("The IAM url used to authenticate the apikey (optional). This defaults to \"https://iam.bluemix.net/identity/token\".")]
    public string assistantIamUrl = "https://iam.bluemix.net/identity/token";
    #endregion

    private static string speechToTextServiceUrlKey = "SPEECH_TO_TEXT_SERVICE_URL";
    private static string speechToTextUsernameKey = "SPEECH_TO_TEXT_USERNAME";
    private static string speechToTextPasswordKey = "SPEECH_TO_TEXT_PASSWORD";
    private static string speechToTextIamApikeyKey = "SPEECH_TO_TEXT_IAM_APIKEY";
    private static string speechToTextIamUrlKey = "SPEECH_TO_TEXT_IAM_URL";
    private static string assistantServiceUrlKey = "ASSISTANT_SERVICE_URL";
    private static string assistantWorkspaceIdKey = "ASSISTANT_WORKSPACE_ID";
    private static string assistantVersionDateKey = "ASSISTANT_VERSION_DATE";
    private static string assistantUsernameKey = "ASSISTANT_USERNAME";
    private static string assistantPasswordKey = "ASSISTANT_PASSWORD";
    private static string assistantIamApikeyKey = "ASSISTANT_IAM_APIKEY";
    private static string assistantIamUrlKey = "ASSISTANT_IAM_URL";


    public void Save()
    {
        // Set the values to the PlayerPrefs file using their corresponding keys.
        PlayerPrefs.SetString(speechToTextServiceUrlKey, speechToTextServiceUrl);
        PlayerPrefs.SetString(speechToTextUsernameKey, speechToTextUsername);
        PlayerPrefs.SetString(speechToTextPasswordKey, speechToTextPassword);
        PlayerPrefs.SetString(speechToTextIamApikeyKey, speechToTextIamApikey);
        PlayerPrefs.SetString(speechToTextIamUrlKey, speechToTextIamUrl);
        PlayerPrefs.SetString(assistantServiceUrlKey, assistantServiceUrl); 
        PlayerPrefs.SetString(assistantWorkspaceIdKey, assistantWorkspaceId);
        PlayerPrefs.SetString(assistantVersionDateKey, assistantVersionDate);
        PlayerPrefs.SetString(assistantUsernameKey, assistantUsername);
        PlayerPrefs.SetString(assistantPasswordKey, assistantPassword);
        PlayerPrefs.SetString(assistantIamApikeyKey, assistantIamApikey);
        PlayerPrefs.SetString(assistantIamUrlKey, assistantIamUrl);

        // Manually save the PlayerPrefs file to disk, in case we experience a crash
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (PlayerPrefs.HasKey(speechToTextServiceUrlKey))
        {
            // load playerName from the PlayerPrefs file.
            speechToTextServiceUrl = PlayerPrefs.GetString(speechToTextServiceUrlKey);
        }

        if (PlayerPrefs.HasKey(speechToTextUsernameKey))
        {
            // load playerName from the PlayerPrefs file.
            speechToTextUsername = PlayerPrefs.GetString(speechToTextUsernameKey);
        }

        if (PlayerPrefs.HasKey(speechToTextPasswordKey))
        {
            // load playerName from the PlayerPrefs file.
            speechToTextPassword = PlayerPrefs.GetString(speechToTextPasswordKey);
        }

        if (PlayerPrefs.HasKey(speechToTextIamApikeyKey))
        {
            // load playerName from the PlayerPrefs file.
            speechToTextIamApikeyKey = PlayerPrefs.GetString(speechToTextIamApikey);
        }

        if (PlayerPrefs.HasKey(speechToTextIamUrlKey))
        {
            // load playerName from the PlayerPrefs file.
            speechToTextIamUrl = PlayerPrefs.GetString(speechToTextIamUrlKey);
        }

        if (PlayerPrefs.HasKey(assistantServiceUrlKey))
        {
            // load playerName from the PlayerPrefs file.
            assistantServiceUrl = PlayerPrefs.GetString(assistantServiceUrlKey);
        }

        if (PlayerPrefs.HasKey(assistantWorkspaceIdKey))
        {
            // load playerName from the PlayerPrefs file.
            assistantWorkspaceId = PlayerPrefs.GetString(assistantWorkspaceIdKey);
        }

        if (PlayerPrefs.HasKey(assistantVersionDateKey))
        {
            // load playerName from the PlayerPrefs file.
            assistantVersionDate = PlayerPrefs.GetString(assistantVersionDateKey);
        }

        if (PlayerPrefs.HasKey(assistantUsernameKey))
        {
            // load playerName from the PlayerPrefs file.
            assistantUsername = PlayerPrefs.GetString(assistantUsernameKey);
        }

        if (PlayerPrefs.HasKey(assistantPasswordKey))
        {
            // load playerName from the PlayerPrefs file.
            assistantPassword = PlayerPrefs.GetString(assistantPasswordKey);
        }

        if (PlayerPrefs.HasKey(assistantIamApikeyKey))
        {
            // load playerName from the PlayerPrefs file.
            assistantIamApikey = PlayerPrefs.GetString(assistantIamApikeyKey);
        }

        if (PlayerPrefs.HasKey(assistantIamUrlKey))
        {
            // load playerName from the PlayerPrefs file.
            assistantIamUrl = PlayerPrefs.GetString(assistantIamUrlKey);
        }
    }

    /// <summary>Deletes all values from the PlayerPrefs file.</summary>
    public void Delete()
    {
        // Delete all values from the PlayerPrefs file.
        PlayerPrefs.DeleteAll();
    }
}
