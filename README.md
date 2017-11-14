# 581a2-joycon
Unity Joycon Input for [Skippy](https://github.com/murrrkle/581a2/releases).

Your device ***must*** be Bluetooth enabled to work properly.

## Compilation Instructions:
1. Download `joyconInput.zip` and import the `.unitypackage` file into the latest version of Unity 2017
2. In **Edit...** \> **Project Settings...** \> **Player** \> **Other Settings**, change the following:
* **Configuration** \> **API Compatibility Level** should be set to `.Net 2.0`
* **Bundle Identifier** should be set to use the appropriate *Company Name* and *Product Name*
3. In **Build Settings**, click **Build**.

## Running JoyconInput:
1. On the JoyCon, press the Sync button, found on the left side of the remote
2. Open your device's Bluetooth settings and find `Joycon (R)` on the list of nearby devices
3. Once the JoyCon is paired, run the executable compiled earlier
4. If everything is working properly, the rectangular prism in the scene should move with the JoyCon

## Using the JoyCon:
Pressing the trigger button (ZR) starts recording accelerometer values. Releasing it sends the maximum accelerometer value recorded to the stone skipping simulation and tells it to throw a stone using it.

## Libraries used:
* [NetworkIt](https://github.com/kevinta893/NetworkIt/releases)
* [JoyconLib](https://github.com/Looking-Glass/JoyconLib/releases)
