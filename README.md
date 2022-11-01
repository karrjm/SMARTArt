# SMARTArt
SMART-Art aka Stout's Mobile Augmented Reality Tour of Art is an AR template for Unity to simplify the creation of augmented reality experiences.

# How to Install and Use SMART-Art
Follow this guide to learn how setup and use the SMART-Art templates to make augmented reality experiences.

## Setup
Before we can start creating, we need to make sure we have the correct programs. The programs required include Git, Unity, Adobe XD, and the Xuid plugins for Unity and Adobe XD.

### Downloading Git
Git is a free and open-source version control system that allows us to import Smart-Art package dependencies. Download git here: [https://git-scm.com/downloads](https://git-scm.com/downloads)

When running the Git installer, you will be presented with many options to configure. Fortunately, the default settings in all cases will work and should not be changed unless you understand what the setting does.

There is no need to launch Git, it will be called automatically as we open the project for the first time. However, restarting your computer is required before proceeding.

### Downloading Unity Hub
1. Go to [Unity's Download Page](https://store.unity.com/download) (https://store.unity.com/download).
2. Review the terms and confirm that you are eligible for Unity Personal.
3. Click "Download Unity Hub", an installer called "UnityHubSetup.exe" will be downloaded.
4. Open the downloaded installer.
5. Accept the license and terms.
6. Specify install location (default is fine).
7. You now have Unity Hub installed; it would launch if you left that option checked in the installer. If not, search for "Unity Hub" in the start menu search bar to launch.

### Creating a Unity account
1. Unity requires an account to use. Start by opening Unity Hub through the desktop or the start menu if it is not open already.
2. If you have a Unity account, you can sign into Unity Hub at this point and skip ahead in the guide. If you do not have a Unity account, navigate to the Unity ID button in the top right and attempt to sign in. Unity will prompt you to create an account if you do not have one.
    
    a. Follow the instructions to create a Unity account, including email confirmation.

    b. Activating a license is easy once you have an account. Go to settings in Unity Hub and select the License Management tab. When "Activate New License" is selected, the Unity Personal License can be selected.

### Downloading the required version of Unity using Unity Hub
1. Navigate to the installs tab in Unity Hub.
2. Click the "ADD" button to add a version of Unity.
3. Choose the recommended release. Note: _During development, the SMART-Art team used Unity version 2020.3.8f1. The key is to get this version OR later._
4. _Configure Modules_

    a. Deselect Microsoft Visual Studio Code _(Unless you are continuing development and prefer this IDE)._
    
    b. Now select the modules required to build to mobile, then hit next. We will need:
    
        i. IOS build support.
        ii. Android build support.

5. Read and Accept the End User License Agreement.
6. Installing Unity can take some time. Once done, Unity will be installed on your computer.

### Downloading Adobe XD/Creative Cloud
Adobe XD is an all-in-one UI/UX design tool which will be used to create the augmented reality UI in the form of "cards".

1. Follow this [link](https://auth.services.adobe.com/en_US/index.html?callback=https%3A%2F%2Fims-na1.adobelogin.com%2Fims%2Fadobeid%2Fadobedotcom-cc%2FAdobeID%2Ftoken%3Fredirect_uri%3Dhttps%253A%252F%252Fwww.adobe.com%2523from_ims%253Dtrue%2526old_hash%253D%252523ref_cc%2526api%253Dauthorize%26state%3D%257B%2522ac%2522%253A%2522%2522%257D%26code_challenge_method%3Dplain%26use_ms_for_expiry%3Dtrue&client_id=adobedotcom-cc&scope=AdobeID%2Copenid%2Ccreative_cloud%2Cgnav%2Cread_organizations%2Cadditional_info.projectedProductContext%2Csao.ACOM_CLOUD_STORAGE%2Csao.stock%2Csao.cce_private%2Cadditional_info.roles&denied_callback=https%3A%2F%2Fims-na1.adobelogin.com%2Fims%2Fdenied%2Fadobedotcom-cc%3Fredirect_uri%3Dhttps%253A%252F%252Fwww.adobe.com%2523from_ims%253Dtrue%2526old_hash%253D%252523ref_cc%2526api%253Dauthorize%26response_type%3Dtoken%26state%3D%257B%2522ac%2522%253A%2522%2522%257D&state=%7B%22ac%22%3A%22%22%7D&relay=c38c67f9-41b5-4fbd-95d2-e16ac3bdcbd4&locale=en_US&flow_type=token&idp_flow_type=login#/), it should take you to the adobe login page of which you will use your UW login for. Start with your UW email address and it will redirect you to the university login page.
2. Once in adobe, it should direct you to the creative cloud page of which you should click "Open Creative Cloud".
3. It may also start on one of the other tabs so in case that happens just tab over to Creative Cloud and click Download.
4. Follow all the steps in the download process.
5. you may be promted to login, use your UW-Stout login.
6. Now that you have Adobe Cloud, Navigate the menu to find Adobe XD and install.

### Downloading the SMART-Art Templates
V1.0 is available here: [https://github.com/karrjm/SMARTArt/releases/tag/v1.0](https://github.com/karrjm/SMARTArt/releases/tag/v1.0)

Download the project as a ZIP by clicking the "Source Code" file packaged in the release assets. Once downloaded, move this ZIP folder to a well-known location such as your desktop or documents. Once in a known location, unzip the folder. This is the project folder that will be added to Unity Hub.

#### Opening the SmartArt project in Unity
To add the SMART-Art project to Unity, go into the Unity hub and navigate to projects. There you will see two buttons, "ADD" and "NEW". In this case we want to add the project that was just downloaded and unzipped.

### Adobe XD to Unity plugin and package
[XuidUnity](https://github.com/itouh2-i0plus/XuidUnity) is a plugin that will simplify the content creation pipeline by allowing us to export UI made in Adobe XD to Unity. The plugin and package are required for Adobe XD _ **AND** _ Unity, respectively.

    The plugins should now come packaged with v1.0 of the SmartArt template and thus covered by the previous step.

#### Adobe XD Plugin
To install, ensure that Adobe XD is open. Click the file named "XuidUnityExporter.xdx" this will open a prompt in Adobe XD to confirm you want to install this plugin.

#### Unity Package
The Unity Package comes installed with the template and should not need to be messed with. If for whatever reason it is not present, you can install it here by downloading "XuidUnity.unitypackage".

\*If prompted to update to a newer version of the package in unity or the plugin in Adobe XD, decline as it would change the version of one but not the other.

### Vuforia
Vuforia comes packaged with the SmartArt unity project. If this is not the case, download the Vuforia package using these instructions: https://library.vuforia.com/articles/Training/getting-started-with-vuforia-in-unity.html

#### INSTRUCTORS & DEVELOPERS ONLY
Instructors/Devs should create a Vuforia account to manage the image target database.

Register an account here: [https://developer.vuforia.com/](https://developer.vuforia.com/)

Once an account has been registered:

1. Navigate to developer portal
2. Create license if the smart art template does not have one
3. Navigate to target manger
  1. Vuforia provides an in-depth guide which can be found here: [https://library.vuforia.com/articles/Training/Getting-Started-with-the-Vuforia-Target-Manager.html](https://library.vuforia.com/articles/Training/Getting-Started-with-the-Vuforia-Target-Manager.html)
4. Create database (device/cloud)
5. Select new database and add targets
  1. Choose "single image" (this best represents paintings, murals, portraits)
  2. Choose a file
  3. Enter width of target in METERS
  4. Give it a descriptive name

After a target is added, Vuforia will rate attributes such as detail, contrast, no repetitive patterns, and file format. A low score often means the reference image needs to be of higher quality (high resolution). The worst case is that the target image does not meet the requirements for image recognition. Best practices for image targets can be found here:

[https://library.vuforia.com/features/images/image-targets/best-practices-for-designing-and-developing-image-based-targets.html](https://library.vuforia.com/features/images/image-targets/best-practices-for-designing-and-developing-image-based-targets.html)

Once an image target database is created, the database can be downloaded (or hosted online if cloud was chosen) and shared with content creators.

## Creating the Experience

### Set Up
When the SMART-Art project is first opened, it will open in an empty untitled scene. We need to load the template scene instead, so you should navigate to the scenes folder and double click the "Final Template" scene in the project directory. Unity will prompt you to save any unfinished work, but since this is the default empty scene it can be discarded.

At this point, unity can be put on the backburner. We will need to create the Adobe XD content before we can start working in Unity.

The Adobe XD templates come packaged with the SMART-Art project, to find them, go into the project folder on your PC and open SMARTArt\>Assets\>Smart-Art and within you should see the "AdobeXDTemplates" folder of which will have the XD file named "SMART\_ART Templates.xd" inside with the template assets. The template file can be opened with Adobe XD.

### Prefabs
To add more topic cards, go to the layers tab in the bottom left and then right click a topic and click duplicate. Drag and drop the Artboard into the area you would like and even sort them and label for easy export using a textbox outside of the artboards. Go to the Document Assets tab in the bottom left and chose the prefab text boxes as needed for slides and resize and position how you would like them to be.

### Importing Adobe XD
Once cards are created, use the naming convention of "Topic/Poi#/CardName" before export.

Following this make sure to select all slides as if one is not selected or partially selected it may mess up the exporting process.

In the top bar of Adobe XD go to "Plugins" and then "Xuid Unity Export"

The screen below should pop up, click the … in the Output section in which you should create a project folder somewhere on your computer to save these files to. If done correctly click Export and it should put the files into the correct folders.

Now go back into Unity where you will click on the "Assets" tab where near the bottom you will see "XuidUnity" hover over that and click "Clean Import". This will prompt you to find the Project folder you created previously in which you will select that entire folder and your assets will import.

Now go into the Project tab in Unity, go to the "SMART-Art" folder and open "IO Plus", "XuidUnity", "Created Prefabs" of which you should now see the respective topic slides you have made and within will be the POI slides.

Click on the Image Target you are working on in the Hierarchy tab in Unity, then press F while hovering over the Scene view. Open the "Label Canvas" and put the Information needed. Open the "UICanvas" and then the "Label Stack" and duplicate as many topic labels as needed and change their text input to the correct topics. Now, go into the "Topic Stack" within the Topic Stack you will need to Duplicate as many Topic cards as you have Topics, then within each topic card duplicate as many POI buttons as you are using, THEN within the POI buttons and within the "Card Stack" inside each, duplicate as many cards as you plan to put in each POI button. Drag the corresponding Adobe XD slides on top of the cards in the stacks and once all are selected make sure to resize all the prefabs or they will be far too large for the scene. (Recommended to try an X&Y scale of 0.005 to start.) ￼￼￼￼￼￼￼￼Adjust scene as needed with tests within the unity tester using your note card and make sure everything works!

## Building to a Device
Building to an Android Device:

1. Make sure your phone is in developer mode. If it is, you should find a developer options tab somewhere in your phone settings. If not, you can quickly google how to turn on developer mode for your specific device.

1. Go into your device's developer options in the settings and make sure USB debugging is turned on.

1. Make sure Android build support is installed for your current version of unity. You can check this by going to Edit\>Preferences\>External Tools and scrolling all the way down to the Android section. If all four slots have a path to an SDK and no errors are present, then you should be good to go.

1. Go to File\>Build Settings and switch your platform to Android if you have not already.

1. Go back to Build settings and click the player settings button. Scroll down the left-hand list and click on XR Plug-In Management. Then make sure that ARKit under the iOS tab is checked, and ARCore under the android tab is also checked. The tabs are shown by the respective company symbols.

1. Connect your phone to your computer with a USB/USB-C cable (your charger cable or a cable like it should work fine). Make sure your phone allows the computer to access it. A pop up should appear on your phone screen once you plug your phone in asking for permission, so make sure to allow access.

1. Under build settings, now you just simply hit Build and Run, and you will be prompted to save an apk file. You can save it anywhere, but you typically want to save it somewhere associated with the project, or somewhere easily accessible, such as your desktop. After it is saved you should get a loading bar while the app builds to your phone. Once it is done, the app will automatically launch on your device.

[https://docs.unity3d.com/Manual/android-BuildProcess.html](https://docs.unity3d.com/Manual/android-BuildProcess.html)
