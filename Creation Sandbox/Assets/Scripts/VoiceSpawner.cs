using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using IBM.Watson.DeveloperCloud.Widgets;
using IBM.Watson.DeveloperCloud.DataTypes;
using IBM.Watson.DeveloperCloud.Utilities;
using IBM.Watson.DeveloperCloud.Services.Conversation.v1;
using IBM.Watson.DeveloperCloud.Services.SpeechToText.v1;

#pragma warning disable 414

public class VoiceSpawner : Widget {

    public GameManager gameManager;
    public AudioClip sorryClip;
    public List<AudioClip> helpClips;

    private Conversation m_Conversation = new Conversation();
    private string m_WorkspaceID;

    [SerializeField]
    private Input m_SpeechInput = new Input("SpeechInput", typeof(SpeechToTextData), "OnSpeechInput");

    #region InitAndLifecycle
    //------------------------------------------------------------------------------------------------------------------
    // Initialization and Lifecycle
    //------------------------------------------------------------------------------------------------------------------

    protected override void Start()
    {
        base.Start();
        m_WorkspaceID = Config.Instance.GetVariableValue("ConversationV1_ID");
    }

    protected override string GetName()
    {
        return "VoiceSpawner";
    }
    #endregion

    #region EventHandlers
    //------------------------------------------------------------------------------------------------------------------
    // Event Handler Functions
    //------------------------------------------------------------------------------------------------------------------

    private void OnSpeechInput(Data data)
    {
        SpeechRecognitionEvent result = ((SpeechToTextData)data).Results;
        if (result != null && result.results.Length > 0)
        {
            foreach (var res in result.results)
            {
                foreach (var alt in res.alternatives)
                {
                    if (res.final && alt.confidence > 0)
                    {
                        string text = alt.transcript;
                        Debug.Log("Result: " + text + " Confidence: " + alt.confidence);
                        m_Conversation.Message(OnMessage, m_WorkspaceID, text);
                    }
                }
            }
        }
    }

    void OnMessage(MessageResponse resp, string customData)
    {
        if (resp != null && (resp.intents.Length > 0 || resp.entities.Length > 0))
        {
            string intent = resp.intents[0].intent;
            Debug.Log("Intent: " + intent);
            string currentMat = null;
            string currentScale = null;
            if (intent == "create")
            {
                bool createdObject = false;
                foreach (EntityResponse entity in resp.entities)
                {
                    Debug.Log("entityType: " + entity.entity + " , value: " + entity.value);
                    if (entity.entity == "material")
                    {
                        currentMat = entity.value;
                    }
                    if (entity.entity == "scale")
                    {
                        currentScale = entity.value;
                    }
                    else if (entity.entity == "object")
                    {
                        gameManager.CreateObject(entity.value, currentMat, currentScale);
                        createdObject = true;
                        currentMat = null;
                        currentScale = null;
                    } 
                }

                if (!createdObject)
                {
                    gameManager.PlayError(sorryClip);
                }
            } else if (intent == "destroy")
            {
                gameManager.DestroyAtRight();
                gameManager.DestroyAtLeft();
            } else if (intent == "help")
            {
                if (helpClips.Count > 0)
                {
                    gameManager.PlayClip(helpClips[Random.Range(0, helpClips.Count)]);
                }
            } else if (intent == "screenshot")
            {
                //Assumes is attached to the [CameraRig]
                Camera camera = transform.parent.FindChild("Camera (eye)").GetComponent<Camera>();
                DemoScreen.takeScreenshot(camera);
            }
        } else
        {
            Debug.Log("Failed to invoke OnMessage();");
        }
    }
    #endregion
}
