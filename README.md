# UNBEATABLE Custom Silence

A mod to modify the appearance of the silence in the hit rhythm game UNBEATABLE!

## Configuration

To load a custom silence image, simply put an image file called `silence.png` or `silence.jpg` inside the same folder as the mod DLL file. 

The image file will be reloaded on every restart, and if it is not present will simply show the normal silence sprites (i.e., you can easily disable it by simply renaming or deleting the image).

There are also some options available in the BepInEx configuration file:

**Custom Silence Enabled** is an easy global toggle for turning the features of the mod on and off.

There are also options for each specific note type to change the scale of the image in case you want the custom silence images to be bigger or smaller.

## Mod Installation Instructions

- Download the latest release of the mod from the releases page, and extract the DLL file from inside the zip.
- Download BepInEx from [here](https://github.com/BepInEx/BepInEx/releases) and extract the BepInEx folder from the zip into the main UNBEATABLE game code folder (the one that contains UNBEATABLE.exe). You must extract ALL the files from that zip into the main UNBEATABLE folder (do NOT make a new folder!).
- Run the game once and close it.
- Put the mod DLL into the BepInEx\plugins folder.

The structure should then be:

<pre>
UNBEATABLE
├─── UNBEATABLE.exe
├─── UNBEATABLE_Data
├─── {some other folders and files}
├─── .doorstop_version
├─── changelog.txt
├─── doorstop_config.ini
├─── winhttp.dll
└─── BepInEx
    ├─── cache
    ├─── config
    ├─── core
    ├─── patchers
    └─── plugins
        ├─── SomeMod.dll
        └─── SomeOtherMod.dll
</pre>

Once the mod is in the folder, restart the game and it should load.

