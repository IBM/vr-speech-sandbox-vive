# Watson 서비스를 사용하여 음성 기반의 VR경험 구현하기

*다른 언어로 보기: [English](README.md).*

이번 개발 과정에서는 Watson [Speech-to-Text](https://www.ibm.com/watson/developercloud/speech-to-text.html) 와 Watson  [Conversation](https://www.ibm.com/watson/developercloud/conversation.html) 서비스를 기반으로 VR(Virtual Reality, 가상 현실) 게임을 만들어보겠습니다.

우리가 실제 공간에 “있는” 것처럼 느껴지는 가상 현실 속에서 ‘말하기’는 다른 어떤 상호작용 방법보다 훨씬 자연스럽게 의사를 주고받을 수 있는 소통 수단입니다. 말하기 기능을 제공함으로써 개발자는 한층 몰입감 넘치는 경험을 만들어낼 수 있습니다. HTC Vive는 (Google Cardboard를 제외하고)
현재 머리에 착용하는 VR 디바이스(HMD: Head Mounted Display) 중 전세계 3번째로 인기있는 제품으로, ‘말하기’ 기능을 적용하기에 가장 적합한 디바이스이며, 2016년 약
[40만대 ](http://www.hypergridbusiness.com/2016/11/report-98-of-vr-headsets-sold-this-year-are-for-mobile-phones)가 판매되었습니다.

이 과정을 마치면 다음 방법을 이해할 수 있습니다.

* Unity에서 빌드한 가상 현실 환경에 IBM Watson Speech-to-Text 및 Conversation 추가.

![](doc/source/images/architecture.png)

### With Watson

Watson 앱을 한 레벨 위로 끌어올리고 싶으신가요? 아니면 Watson 브랜드 기술을 활용하고 싶으신가요? 특별한 브랜딩, 마케팅 및 기술 자료를 제공하여 Watson 기반 상용 솔루션을 한층 업그레이드하고 개발 속도를 앞당겨주는  [With Watson](https://www.ibm.com/watson/with-watson)프로그램에 가입하세요.

## 구성요소

* [IBM Watson Conversation](https://www.ibm.com/watson/developercloud/conversation.html): 음성이나 텍스트 기반의 대화형 프로그램 환경인 챗봇을 만들 수 있습니다.
* [IBM Watson Speech-to-Text](https://www.ibm.com/watson/developercloud/speech-to-text.html): 음성을 텍스트로 변환해 주는 서비스입니다.

## 주요 기술

* [Unity](https://unity3d.com/): PC, 콘솔, 모바일 디바이스, 웹 사이트용 비디오 게임을 개발하는 데 사용하는 크로스 플랫폼 게임 엔진입니다.

# 단계

1. [시작 전 주의사항](#1-시작-전-주의사항)
2. [IBM Cloud 서비스 생성](#2-bluemix-서비스-생성)
3. [빌드 및 실행](#3-빌드-및-실행)

## 1. 시작 전 주의사항

* [IBM Cloud 계정](http://ibm.biz/Bdimr6)
* ["VR 에 적합한" PC](https://www.vive.com/us/ready/)
* [HTC Vive](https://www.vive.com/us/product/)
* [SteamVR](http://store.steampowered.com/steamvr)
* [Unity](https://unity3d.com/get-unity/download)
* [Blender](https://www.blender.org/)

## 2. IBM Cloud 서비스 생성

로컬 시스템에서:
1. `git clone https://github.com/IBM/vr-speech-sandbox-vive.git`
2. `cd vr-speech-sandbox-vive`

[IBM Cloud](https://console.ng.bluemix.net/)에서:

1. [Speech-To-Text](https://console.ng.bluemix.net/catalog/speech-to-text/) 서비스 인스턴스를 생성합니다.
2. [Conversation](https://console.ng.bluemix.net/catalog/services/conversation/) 서비스 인스턴스를 생성합니다.
3. 대시보드에 서비스가 표시되면 생성된 Conversation 서비스를 선택하고 !["Launch Tool"](/doc/source/images/workspace_launch.png?raw=true) 버튼을 클릭합니다.
4. Conversation Tool에 로그인한 후 !["Import"](/doc/source/images/import_icon.png?raw=true) 단추를 클릭합니다.
5. 이 저장소 복제본에 있는 Conversation  [`workspace.json`](data/workspace.json) 파일을 가져옵니다.

## 3. 빌드 및 실행

이전 단계를 수행했다면, 로컬 복제본으로 이미 이동하여 Unity에서 앱 실행을 시작할 수 있도록 준비된 상태여야 합니다.

1. `git clone https://github.com/watson-developer-cloud/unity-sdk.git`
2. Unity를 열고 프로젝트 대시보드에서  ![Open](doc/source/images/unity_open.png?raw=true) 버튼을 선택합니다.
3. 이 저장소를 복제한 곳으로 이동해서 "Creation Sandbox" 디렉토리를 엽니다.
4. If prompted to upgrade the project to a newer Unity version, do so.
5. [이 지침](https://github.com/watson-developer-cloud/unity-sdk#getting-the-watson-sdk-and-adding-it-to-unity) 에 따라 단계 1에서 다운로드한 Watson Unity SDK를 프로젝트에 추가합니다.
6. [이 지침](https://github.com/watson-developer-cloud/unity-sdk#configuring-your-service-credentials) 에 따라 Speech To Text 및 Conversation 서비스 신임 정보 [IBM Cloud](https://console.ng.bluemix.net/)에 있는)를 추가합니다..
7. 설정(configuration) 창에서 `Advanced Mode`를 선택합니다.
8. `Add Variable` 를 클릭하고 새 변수에 `ConversationV1_ID` 라고 이름을 지정한 다음 값을 Conversation 워크스페이스의 Workspace ID로 설정합니다.
    ![Variable Configuration Example](doc/source/images/add_variable.png?raw=true)
 Workspace ID는 Conversation 워크스페이스에서 확장 메뉴(expansion menu)를 선택하고 `View details`를 선택하면 확인할 수 있습니다.
    ![View Details Location](doc/source/images/workspace_details.png?raw=true) 
9. [Blender](https://www.blender.org) 를 설치합니다.
10. Unity 편집기 프로젝트 탭에서 Assets->Scenes->MainGame->MainMenu를 선택하고 두 번 클릭하여 장면을 로딩합니다.
11. Play를 누릅니다.

# 샘플 결과

[![](http://img.youtube.com/vi/FlMvLDw6cYc/0.jpg)](http://www.youtube.com/watch?v=FlMvLDw6cYc)

# 링크

* [Youtube 데모](https://www.youtube.com/watch?v=rZFpUpy4y0g)
* [Viveport](https://www.viveport.com/apps/bbde0cff-98c1-4117-acd8-e808ded515ca)
* [개발자 블로그](https://www.ibm.com/innovation/milab/watson-speech-virtual-reality-unity/)
* [사례 연구](https://www.ibm.com/innovation/milab/work/speech-sandbox/)
* [Watson Unity SDK](https://github.com/watson-developer-cloud/unity-sdk)

# 라이센스

[Apache 2.0](LICENSE)
