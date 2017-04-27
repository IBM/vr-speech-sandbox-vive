# Project Structure

Here is a general overview of some of the more important directories in the project.

```
.
├── data
│   └── workspace.json      // an export of a basic Speech Sandbox conversation setup
├── Creation Sandbox        // the Unity project
│   ├── Assets                  // contains all project assets
│   │   ├── _Scenes                 // all game scenes
│   │   │   ├── MainGame
│   │   │   │   ├── MainMenu            // the main menu scene
│   │   │   │   ├── Tutorial            // the tutorial scene
│   │   │   │   └── Sandbox             // the main sandbox scene
│   │   ├── Prefabs                 // the games prefabs
│   │   │   ├── CreatableObjects        // all objects that can be created
│   │   │   ├── Tutorial                // objects pertaining to the tutorial
│   │   │   └── Widgets                 // contains custom widgets (like VoiceSpawner)
│   │   ├── Scripts                 // all custom game scripts
│   │   │   ├── Controls                // scripts related to the game controls
│   │   │   ├── Objects                 // scripts related to the objects that can be created
│   │   │   ├── SceneManagement         // scripts related to manageing scene state
│   │   │   ├── Tutorial                // scripts related to running the tutorial
│   │   │   └── UI                      // scripts related to any menuing 
│   │   ├── SteamVR                 // the SteamVR plugin
│   │   └── VRTK                    // the VRTK plugin
│   └── ProjectSettings         // contains configuration files for the project
```

For additional help understanding the code you can check out [this blog](https://www.ibm.com/innovation/milab/watson-speech-virtual-reality-unity/) written by [Kyle Craig](https://twitter.com/thekylecraig) about how to implement this type of speech control in your own projects.
