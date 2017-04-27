# Create voice commands for VR experiences with Watson services

In this developer journey we will create a Virtual Reality game based on Watson's [Speech-to-Text](https://www.ibm.com/watson/developercloud/speech-to-text.html) and Watson's [Conversation](https://www.ibm.com/watson/developercloud/conversation.html) services.

In Virtual Reality, where you truly “inhabit” the space, speech can feel like a more natural interface than other methods. Providing speech controls allows developers to create more immersive experiences. The HTC Vive is the 3rd most popular head-mounted VR devices and an ideal candidate for Speech interaction, selling roughly 2.1 million units in 2016.

When the reader has completed this journey, they will understand how to:

* Add IBM Watson Speech-to-Text and Conversation to a Virtual Reality environment build in Unity.

See the video below for an example:

[![](http://img.youtube.com/vi/FlMvLDw6cYc/0.jpg)](http://www.youtube.com/watch?v=FlMvLDw6cYc)

For more information about the project you can check out our [Case Study](https://www.ibm.com/innovation/milab/work/speech-sandbox/). Or if you just want to play a built version with pre-loaded objects you can download the game on [Viveport](https://www.viveport.com/apps/bbde0cff-98c1-4117-acd8-e808ded515ca).

![](doc/source/images/architecture.png)

## Included components

* [IBM Watson Conversation](https://www.ibm.com/watson/developercloud/conversation.html): Create a chatbot with a program that conducts a conversation via auditory or textual methods.
* [IBM Watson Speech-to-Text](https://www.ibm.com/watson/developercloud/speech-to-text.html): Converts audio voice into written text.

## Featured technologies

* [Unity](https://unity3d.com/): A cross-platform game engine used to develop video games for PC, consoles, mobile devices and websites.

# Steps

1. [Before you begin](#1-before-you-begin)
2. [Create Bluemix services](#2-create-bluemix-services)
3. [Building and Running](#3-building-and-running)

## 1. Before You Begin

* [IBM Bluemix Account](https://console.ng.bluemix.net/registration/)
* ["VR Ready" PC](https://www.vive.com/us/ready/)
* [HTC Vive](https://www.vive.com/us/product/)
* [SteamVR](http://store.steampowered.com/steamvr)
* [Unity](https://unity3d.com/get-unity/download)

## 2. Create Bluemix services

On your local machine:
1. `git clone https://github.com/IBM/vr-speech-sandbox-vive.git`
2. `cd vr-speech-sandbox-vive`

In [Bluemix](https://console.ng.bluemix.net/):

1. Create a [Speech-To-Text](https://console.ng.bluemix.net/catalog/speech-to-text/) service instance.
2. Create a [Conversation](https://console.ng.bluemix.net/catalog/services/conversation/) service instance.
3. Once you see the services in the Dashboard, select the Conversation service you created and click the !["Launch Tool"](/doc/source/images/workspace_launch.png?raw=true) button.
4. After logging into the Conversation Tool, click the !["Import"](/doc/source/images/import_icon.png?raw=true) button.
5. Import the Conversation [`workspace.json`](data/workspace.json) file located in your clone of this repository.

## 3. Building and Running

If you followed the previous steps you should already be inside your local clone and ready to get started running the app from Unity.

1. `git clone https://github.com/watson-developer-cloud/unity-sdk.git`
2. Open Unity and inside the project launcher select the ![Open](doc/source/images/unity_open.png?raw=true) button.
3. Navigate to where you cloned this repository and open the "Creation Sandbox" directory.
4. If prompted to upgrade the project to a newer Unity version, do so.
5. Follow [these instructions](https://github.com/watson-developer-cloud/unity-sdk#getting-the-watson-sdk-and-adding-it-to-unity) to add the Watson Unity SDK downloaded in step 1 to the project.
6. Follow [these instructions](https://github.com/watson-developer-cloud/unity-sdk#configuring-your-service-credentials) to add your Speech To Text and Conversation service credentials (located on [IBM Bluemix](https://console.ng.bluemix.net/)).
7. Select `Advanced Mode` in the configuration window.
8. Click `Add Variable` and name your new variable `ConversationV1_ID` then set its value to the Workspace ID of your Conversation workspace.
    ![Variable Configuration Example](doc/source/images/add_variable.png?raw=true)
 You can find your workspace ID by selecting the expansion menu on your conversation workspace and selecting `View details`.
    ![View Details Location](doc/source/images/workspace_details.png?raw=true)
9. In the Unity editor project tab, select Assets->Scenes->MainGame->MainMenu and double click to load the scene.
10. Press Play

# Sample output

Add output when ready

<!--
Start a conversation with your bot:

![](doc/source/images/convo_init.png)

Add an item to your cart:

![](doc/source/images/convo_add.png)
-->

# Links

* [Viveport](https://www.viveport.com/apps/bbde0cff-98c1-4117-acd8-e808ded515ca)
* [Dev Blog](https://www.ibm.com/innovation/milab/watson-speech-virtual-reality-unity/)
* [Case Study](https://www.ibm.com/innovation/milab/work/speech-sandbox/)
* [Watson Unity SDK](https://github.com/watson-developer-cloud/unity-sdk)
* [IBM Bluemix](https://www.ibm.com/cloud-computing/bluemix/)

# License

[Apache 2.0](LICENSE)
