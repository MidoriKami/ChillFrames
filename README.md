# ChillFrames
[![Download count](https://img.shields.io/endpoint?url=https://vz32sgcoal.execute-api.us-east-1.amazonaws.com/ChillFrames)](https://github.com/MidoriKami/ChillFrames)

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

The various options allow you to turn off the frame limiter (that is, to have maximum fps) under specific conditions.

For example, if you want maximum fps while crafting you would check `Disable during Crafting`

| General Configuration                                                                                   | Blacklist Configuration                                                                                 |
|---------------------------------------------------------------------------------------------------------|---------------------------------------------------------------------------------------------------------|
| ![image](https://github.com/MidoriKami/ChillFrames/assets/9083275/2435dd16-a1b8-4cea-810d-591a59b80480) | ![image](https://github.com/MidoriKami/ChillFrames/assets/9083275/020e46d7-f0a1-4fc5-8eae-75fecb885a48) |
| ![image](https://github.com/MidoriKami/ChillFrames/assets/9083275/a48c2dc0-bc55-477a-a941-5013513824ea) |                                                                                                         |


## Testimonials

![image](https://user-images.githubusercontent.com/9083275/159103862-54542bbb-6dd4-49ef-a7fb-358e9e116ca9.png)
