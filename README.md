# ChillFrames
[![Download count](https://img.shields.io/endpoint?url=https://vz32sgcoal.execute-api.us-east-1.amazonaws.com/ChillFrames)](https://github.com/MidoriKami/ChillFrames)

Chill Frames is a XivLauncher/Dalamud plugin.

This is a very simple utility plugin that allows you to conditionally enable a framerate limiter ingame.

## Why is this useful?

This plugin allows you set a frame limiter for the parts of the game that aren't that important to have constantly running at a high framerate.
When your framerate isn't limited your GPU is using maximum power, generating maximum heat, for minimal visual impact.

For those that game on a mobile laptop this will also allow you to save significant battery as well.

## How does it work?

This plugin hooks into the games renderer and delays frame rendering to generate the user specified framerate.

The delay happens *after* the games default logic, and will only act to reduce framerates.

This means if you have the games settings to reduce the framerate when afk, that setting will still apply and will take priority over the delay Chill Frames causes.

## Configuration Window

The various options allow you to turn off the frame limiter (that is, to have maximum fps) under specific conditions.

For example, if you want maximum fps while crafting you would check `Disable during Crafting`

| General Configuration             |  Blacklist Configuration |
|------------------------|-------------------------|
| ![image](https://github.com/MidoriKami/ChillFrames/assets/9083275/2435dd16-a1b8-4cea-810d-591a59b80480) |  ![image](https://github.com/MidoriKami/ChillFrames/assets/9083275/020e46d7-f0a1-4fc5-8eae-75fecb885a48) |
| ![image](https://github.com/MidoriKami/ChillFrames/assets/9083275/a48c2dc0-bc55-477a-a941-5013513824ea) | |


## Testimonials

![image](https://user-images.githubusercontent.com/9083275/159103862-54542bbb-6dd4-49ef-a7fb-358e9e116ca9.png)
