﻿using System;
using System.Collections.Generic;
using UnityEngine;
using BepInEx;
using HarmonyLib;
using JotunnLib.Managers;

namespace JotunnLib
{
    [BepInPlugin(ModGuid, "JotunnLib", Version)]
    public class JotunnLib : BaseUnityPlugin
    {
        // BepInEx plugin parameters
        public const string Version = "0.1.9";
        public const string ModGuid = "com.bepinex.plugins.jotunnlib";

        // Load order for managers
        private readonly List<Type> managerTypes = new List<Type>()
        {
            typeof(LocalizationManager),
            typeof(EventManager),
            typeof(CommandManager),
            typeof(InputManager),
            typeof(SkillManager),
            typeof(PrefabManager),
            typeof(PieceManager),
            typeof(ObjectManager),
            typeof(ZoneManager),
        };

        private readonly List<Manager> managers = new List<Manager>();

        internal static GameObject RootObject;

        private void Awake()
        {
            // Initialize harmony patches
            Harmony harmony = new Harmony("jotunnlib");
            harmony.PatchAll();

            // Create and initialize all managers
            RootObject = new GameObject("_JotunnLibRoot");
            GameObject.DontDestroyOnLoad(RootObject);

            foreach (Type managerType in managerTypes)
            {
                managers.Add((Manager)RootObject.AddComponent(managerType));
            }

            foreach (Manager manager in managers)
            {
                manager.Init();
            }

            initCommands();

            Debug.Log("JotunnLib v" + Version + " loaded successfully");
        }

        private void initCommands()
        {
            // No commands to initialize
        }
    }
}
