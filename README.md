# ChillFrames
[![Download count](https://img.shields.io/endpoint?url=https://qzysathwfhebdai6xgauhz4q7m0mzmrf.lambda-url.us-east-1.on.aws/ChillFrames)](https://github.com/MidoriKami/ChillFrames)

Chill Frames is a XivLauncher/Dalamud plugin.

This is a very simple utility plugin that allows you to conditionally enable a framerate limiter in-game.

## Why is this useful?

This plugin allows you set a frame limiter for the parts of the game that aren't that important to have constantly running at a high framerate.
When your framerate isn't limited your GPU is using maximum power, generating maximum heat, for minimal visual impact.

For those that game on a mobile device this will also allow you significantly increase your battery life.

## How does it work?

This plugin uses the games built in framework update system to correctly limit your framerate to the desired value.

If the time between the last frame and the current frame was too short, 
then a "sleep" is introduced that suspends the calculation that the game performs each frame.

This results in the framerate being reduced to the correct values, while conserving battery power/energy.

ChillFrames can only *reduce* the games framerate, 
if you use the games setting to reduce the framerate while afk, 
that setting will still apply and the lower of the two limits will be used.

## Dalamud Multi-Monitor Support

When you enable Dalamud's Multi-Monitor Support mode that allows you to have plugin windows on other monitors, 
the game splits the "viewport" used for rendering, 
this causes most graphics card driver-based framerate limiters to incorrectly calculate how to limit the framerate.

ChillFrames mitigates this problem by using the games framework system itself to determine when a frame has ended,
allowing you to maintain the correct framerate while using Multi-Monitor Support mode.

## Configuration Window

ChillFrames lets you set a specific upper limit, and a specific lower limit, you can set what limit ChillFrames uses in the configuration window.

The various options allow you to turn off the frame limiter (that is, to have maximum fps) under specific conditions.

For example, if you want maximum fps while crafting you would set the Crafting condition to "Use Upper Limit ( 120 fps )" in this example.

![image](https://github.com/user-attachments/assets/67817bd1-3b7b-4d55-9434-b43c97fd8d2f)

## Server Info Bar Entry (DTR Entry)

ChillFrames also offers a **precise framerate display** that shows the exact fps you are currently getting.

This display does not average your fps at all intentionally, it is very useful to see frame stuttering issues or inconsistent frametimes.

Clicking on the Server Info Bar Entry will toggle enable/disable the plugins framelimiter, 
without needing to use a chat command, or open the configuration window to temporarly disable the limiter.

![image](https://github.com/user-attachments/assets/6560f9df-201c-485e-8288-0ebb2b131630)

## Zone Blacklist

ChillFrames allows you to specify specific zones that you want to disable the limiter entirely while in those zones.

![image](https://github.com/user-attachments/assets/afacce67-6e1f-4dfa-b9cc-12ccda6e5f60)


## Testimonials

![image](https://user-images.githubusercontent.com/9083275/159103862-54542bbb-6dd4-49ef-a7fb-358e9e116ca9.png)
