using System;
using System.Collections.Generic;
using UnityEngine;
using JotunnLib.Entities;
using JotunnLib.Utils;

namespace JotunnLib.Managers
{
    public class CommandManager : Manager
    {
        public static CommandManager Instance { get; private set; }
        private static List<Terminal.ConsoleCommand> queuedCommands = new List<Terminal.ConsoleCommand>();

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("Error, two instances of singleton: " + this.GetType().Name);
                return;
            }

            Instance = this;
        }

        [Obsolete("Please use RegisterConsoleCommand(Terminal.ConsoleCommand cmd) instead", false)]
        public void RegisterConsoleCommand(ConsoleCommand cmd)
        {
            // Legacy wrapper over ConsoleCommand
            RegisterConsoleCommand(new Terminal.ConsoleCommand(cmd.Name, cmd.Help, (args =>
            {
                cmd.Run(args.Args);
            })));
        }

        public void RegisterConsoleCommand(Terminal.ConsoleCommand cmd)
        {
            // Only register commands if the terminal has finished initializing
            if (!ReflectionUtils.GetPrivateStaticField<bool>(typeof(Terminal), "m_terminalInitialized"))
            {
                // Add to queue to initialize later
                queuedCommands.Add(cmd);
                return;
            }

            var commands = ReflectionUtils.GetPrivateField<Dictionary<string, Terminal.ConsoleCommand>>(Console.instance, "commands");

            // Register queued commands
            if (queuedCommands.Count > 0)
            {
                foreach (Terminal.ConsoleCommand c in queuedCommands)
                {
                    // Cannot have two commands with same name
                    if (commands.ContainsKey(c.Command))
                    {
                        Debug.LogError("Cannot have two console commands with same name: " + cmd.Command);
                        return;
                    }

                    commands.Add(c.Command, c);
                }

                queuedCommands.Clear();
            }

            // Cannot have two commands with same name
            if (commands.ContainsKey(cmd.Command))
            {
                Debug.LogError("Cannot have two console commands with same name: " + cmd.Command);
                return;
            }

            // Add command
            commands.Add(cmd.Command, cmd);
        }
    }
}
