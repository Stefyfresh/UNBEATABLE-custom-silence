using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Rhythm;
using UnityEngine;
using System.IO;
using System.Reflection;
using BepInEx.Configuration;

namespace CustomSilence
{
    [BepInPlugin(PLUGIN_GUID, PLUGIN_NAME, PLUGIN_VERSION)]
    [BepInProcess("UNBEATABLE.exe")]
    public class CustomSilence : BaseUnityPlugin
    {
        public const string PLUGIN_GUID = "com.stefyfresh.CustomSilence";
        public const string PLUGIN_NAME = "Custom Silence";
        public const string PLUGIN_VERSION = "1.0.0";
        internal static new ManualLogSource Logger;

        public static readonly float imageDivisor = 110;
        public static readonly float normalRefSize = 15f;
        public static readonly float freestyleSubRefSize = 25f;
        public static readonly float holdTrailRefSize = 25f;
        public static readonly float spamRefSize = 9f;
        public static readonly float nothingRefSize = 13f;


        public static ConfigEntry<bool> modEnabled;

        public static ConfigEntry<float> normalSize;
        public static ConfigEntry<float> doubleSize;
        public static ConfigEntry<float> freestyleSize;
        public static ConfigEntry<float> holdSize;
        public static ConfigEntry<float> freestyleSubSize;
        public static ConfigEntry<float> holdTrailSize;
        public static ConfigEntry<float> spamSize;
        public static Texture2D tex;
        public static bool replacementEnabled;

        public static float SmallestDimension => !replacementEnabled ? 0 : tex.width < tex.height ? tex.width : tex.height;

        private void Awake()
        {
            Logger = base.Logger;
            Logger.LogInfo($"Plugin {PLUGIN_GUID} is loaded!");

            modEnabled = Config.Bind(
                "General",
                "Custom Silence Enabled",
                true,
                "Enables or disables your custom silence!"
            );

            normalSize = Config.Bind(
                "General",
                "Normal Silence Size",
                1f,
                "Sets the scale multiplier of normal notes."
            );

            freestyleSize = Config.Bind(
                "General",
                "Freestyle Size",
                1f,
                "Sets the scale multiplier of freestyle notes."
            );

            doubleSize = Config.Bind(
                "General",
                "Double Size",
                1f,
                "Sets the scale multiplier of double notes."
            );

            holdSize = Config.Bind(
                "General",
                "Hold Size",
                1f,
                "Sets the scale multiplier of hold notes."
            );

            freestyleSubSize = Config.Bind(
                "General",
                "Freestyle Sub Size",
                1f,
                "Sets the scale multiplier of freestyle sub notes."
            );

            holdTrailSize = Config.Bind(
                "General",
                "Hold Trail Size",
                1f,
                "Sets the scale multiplier of the small sprite at the end of a hold note."
            );

            spamSize = Config.Bind(
                "General",
                "Spam Size",
                1f,
                "Sets the scale multiplier of spam notes."
            );

            var harmony = new Harmony(PLUGIN_GUID);
            harmony.PatchAll();
        }

        public static void ReloadTexture()
        {
            tex = new Texture2D(2, 2);
            if (File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "silence.png")))
            {
                replacementEnabled = true;

                byte[] bytes = File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "silence.png"));
                tex.LoadImage(bytes);
            }
            else if (File.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "silence.jpg")))
            {
                replacementEnabled = true;

                byte[] bytes = File.ReadAllBytes(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "silence.jpg"));
                tex.LoadImage(bytes);
            }
            else
            {
                replacementEnabled = false;
            }
        }
    }



    [HarmonyPatch(typeof(RhythmController))]
    [HarmonyPatch(nameof(RhythmController.Awake))]
    internal class ControllerAwakePatch
    {
        static void Postfix()
        {
            CustomSilence.ReloadTexture();
        }
    }




    [HarmonyPatch(typeof(FreestyleNote))]
    [HarmonyPatch(nameof(FreestyleNote.Start))]
    internal class CustomFreestyle
    {
        static void Postfix(ref FreestyleNote __instance)
        {
            if (!CustomSilence.modEnabled.Value || !CustomSilence.replacementEnabled) return;

            Sprite sprite;
            if (__instance.isChild)
            {
                sprite = Sprite.Create(
                    CustomSilence.tex,
                    new Rect(0f, 0f, CustomSilence.tex.width, CustomSilence.tex.height),
                    new Vector2(0.5f, 0.5f),
                    CustomSilence.freestyleSubRefSize * CustomSilence.SmallestDimension / CustomSilence.imageDivisor / CustomSilence.freestyleSubSize.Value,
                    0,
                    SpriteMeshType.FullRect);
            }
            else
            {
                sprite = Sprite.Create(
                    CustomSilence.tex,
                    new Rect(0f, 0f, CustomSilence.tex.width, CustomSilence.tex.height),
                    new Vector2(0.5f, 0.5f),
                    CustomSilence.normalRefSize * CustomSilence.SmallestDimension / CustomSilence.imageDivisor / CustomSilence.freestyleSize.Value,
                    0,
                    SpriteMeshType.FullRect);
            }

            __instance.sprite.GetComponent<Animator>().enabled = false;
            __instance.sprite.sprite = sprite;
        }
    }



    [HarmonyPatch(typeof(DefaultNote))]
    [HarmonyPatch(nameof(DefaultNote.Start))]
    internal class CustomDefault
    {
        static void Postfix(ref DefaultNote __instance)
        {
            if (!CustomSilence.modEnabled.Value || !CustomSilence.replacementEnabled) return;

            Sprite sprite = Sprite.Create(
                CustomSilence.tex,
                new Rect(0f, 0f, CustomSilence.tex.width, CustomSilence.tex.height),
                new Vector2(0.5f, 0.5f),
                CustomSilence.normalRefSize * CustomSilence.SmallestDimension / CustomSilence.imageDivisor / CustomSilence.normalSize.Value,
                0,
                SpriteMeshType.FullRect);
            __instance.sprite.GetComponent<Animator>().enabled = false;
            __instance.sprite.sprite = sprite;
        }
    }



    [HarmonyPatch(typeof(DoubleNote))]
    [HarmonyPatch(nameof(DoubleNote.Start))]
    internal class CustomDouble
    {
        static void Postfix(ref DoubleNote __instance)
        {
            if (!CustomSilence.modEnabled.Value || !CustomSilence.replacementEnabled) return;

            Sprite sprite = Sprite.Create(
                CustomSilence.tex,
                new Rect(0f, 0f, CustomSilence.tex.width, CustomSilence.tex.height),
                new Vector2(0.5f, 0.5f),
                CustomSilence.normalRefSize * CustomSilence.SmallestDimension / CustomSilence.imageDivisor / CustomSilence.doubleSize.Value,
                0,
                SpriteMeshType.FullRect);
            __instance.sprite.GetComponent<Animator>().enabled = false;
            __instance.sprite.sprite = sprite;
        }
    }



    [HarmonyPatch(typeof(HoldNote))]
    [HarmonyPatch(nameof(HoldNote.Start))]
    internal class CustomHold
    {
        static void Postfix(ref HoldNote __instance)
        {
            if (!CustomSilence.modEnabled.Value || !CustomSilence.replacementEnabled) return;

            Sprite sprite = Sprite.Create(
                CustomSilence.tex,
                new Rect(0f, 0f, CustomSilence.tex.width, CustomSilence.tex.height),
                new Vector2(0.5f, 0.5f),
                CustomSilence.normalRefSize * CustomSilence.SmallestDimension / CustomSilence.imageDivisor / CustomSilence.holdSize.Value,
                0,
                SpriteMeshType.FullRect);
            Sprite small = Sprite.Create(
                CustomSilence.tex,
                new Rect(0f, 0f, CustomSilence.tex.width, CustomSilence.tex.height),
                new Vector2(0.5f, 0.5f),
                CustomSilence.holdTrailRefSize * CustomSilence.SmallestDimension / CustomSilence.imageDivisor / CustomSilence.holdTrailSize.Value,
                0,
                SpriteMeshType.FullRect);
            __instance.sprite.GetComponent<Animator>().enabled = false;
            __instance.sprite.sprite = sprite;
            __instance.end.GetComponent<Animator>().enabled = false;
            __instance.end.sprite = small;
        }
    }



    [HarmonyPatch(typeof(SpamNote))]
    [HarmonyPatch(nameof(SpamNote.Start))]
    internal class CustomSpam
    {
        static void Postfix(ref SpamNote __instance)
        {
            if (!CustomSilence.modEnabled.Value || !CustomSilence.replacementEnabled) return;

            Sprite sprite = Sprite.Create(
                CustomSilence.tex,
                new Rect(0f, 0f, CustomSilence.tex.width, CustomSilence.tex.height),
                new Vector2(0.5f, 0.5f),
                CustomSilence.spamRefSize * CustomSilence.SmallestDimension / CustomSilence.imageDivisor / CustomSilence.spamSize.Value,
                0,
                SpriteMeshType.FullRect);
            __instance.sprite.GetComponent<Animator>().enabled = false;
            __instance.sprite.sprite = sprite;
        }
    }



    [HarmonyPatch(typeof(NothingNote))]
    [HarmonyPatch(nameof(NothingNote.Start))]
    internal class CustomNothing
    {
        static void Postfix(ref NothingNote __instance)
        {
            if (!CustomSilence.modEnabled.Value || !CustomSilence.replacementEnabled) return;

            Sprite sprite = Sprite.Create(
                CustomSilence.tex,
                new Rect(0f, 0f, CustomSilence.tex.width, CustomSilence.tex.height),
                new Vector2(0.5f, 0.2f),
                CustomSilence.nothingRefSize * CustomSilence.SmallestDimension / CustomSilence.imageDivisor / CustomSilence.normalSize.Value,
                0,
                SpriteMeshType.FullRect);
            __instance.sprite.GetComponent<Animator>().enabled = false;
            __instance.sprite.sprite = sprite;
        }
    }
}