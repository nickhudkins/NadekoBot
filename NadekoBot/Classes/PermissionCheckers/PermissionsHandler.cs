﻿using Discord;
using Discord.Commands.Permissions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace NadekoBot.Classes.Permissions {
    public static class PermissionsHandler {
        public static ConcurrentDictionary<Server, ServerPermissions> _permissionsDict =
            new ConcurrentDictionary<Server, ServerPermissions>();

        private static void WriteServerToJson(Server server) {
            string pathToFile = $"data/permissions/{server.Id}.json";
            File.WriteAllText(pathToFile, Newtonsoft.Json.JsonConvert.SerializeObject(_permissionsDict[server], Newtonsoft.Json.Formatting.Indented));
        }

        public static void WriteToJson() {
            Directory.CreateDirectory("data/permissions/");
            foreach (var kvp in _permissionsDict) {
                WriteServerToJson(kvp.Key);
            }
        }

        public static void SetServerModulePermission(Server server, string moduleName, bool value) {
            if (!_permissionsDict.ContainsKey(server)) {
                _permissionsDict.TryAdd(server, new ServerPermissions(server.Id.ToString(), server.Name));
            }
            var modules = _permissionsDict[server].Permissions.modules;
            if (modules.ContainsKey(moduleName))
                modules[moduleName] = value;
            else
                modules.Add(moduleName, value);
            WriteServerToJson(server);
        }

        public static void SetServerCommandPermission(Server server, string commandName, bool value) {
            if (!_permissionsDict.ContainsKey(server)) {
                _permissionsDict.TryAdd(server, new ServerPermissions(server.Id.ToString(), server.Name));
            }
            var commands = _permissionsDict[server].Permissions.commands;
            if (commands.ContainsKey(commandName))
                commands[commandName] = value;
            else
                commands.Add(commandName, value);
            WriteServerToJson(server);
        }

        public static void SetChannelModulePermission(Channel channel, string moduleName, bool value) {
            var server = channel.Server;
            if (!_permissionsDict.ContainsKey(server)) {
                _permissionsDict.TryAdd(server, new ServerPermissions(server.Id.ToString(), server.Name));
            }
            if(!_permissionsDict[server].ChannelPermissions.ContainsKey(channel.Id.ToString()))
                _permissionsDict[server].ChannelPermissions.Add(channel.Id.ToString(), new Permissions(channel.Name));

            var modules = _permissionsDict[server].ChannelPermissions[channel.Id.ToString()].modules;

            if (modules.ContainsKey(moduleName))
                modules[moduleName] = value;
            else
                modules.Add(moduleName, value);
            WriteServerToJson(server);
        }

        public static void SetChannelCommandPermission(Channel channel, string commandName, bool value) {
            var server = channel.Server;
            if (!_permissionsDict.ContainsKey(server)) {
                _permissionsDict.TryAdd(server, new ServerPermissions(server.Id.ToString(), server.Name));
            }
            if (!_permissionsDict[server].ChannelPermissions.ContainsKey(channel.Id.ToString()))
                _permissionsDict[server].ChannelPermissions.Add(channel.Id.ToString(), new Permissions(channel.Name));

            var commands = _permissionsDict[server].ChannelPermissions[channel.Id.ToString()].commands;

            if (commands.ContainsKey(commandName))
                commands[commandName] = value;
            else
                commands.Add(commandName, value);
            WriteServerToJson(server);
        }

        public static void SetRoleModulePermission(Role role, string moduleName, bool value) {
            var server = role.Server;
            if (!_permissionsDict.ContainsKey(server)) {
                _permissionsDict.TryAdd(server, new ServerPermissions(server.Id.ToString(), server.Name));
            }
            if (!_permissionsDict[server].RolePermissions.ContainsKey(role.Id.ToString()))
                _permissionsDict[server].RolePermissions.Add(role.Id.ToString(), new Permissions(role.Name));

            var modules = _permissionsDict[server].RolePermissions[role.Id.ToString()].modules;

            if (modules.ContainsKey(moduleName))
                modules[moduleName] = value;
            else
                modules.Add(moduleName, value);
            WriteServerToJson(server);
        }

        public static void SetRoleCommandPermission(Role role, string commandName, bool value) {
            var server = role.Server;
            if (!_permissionsDict.ContainsKey(server)) {
                _permissionsDict.TryAdd(server, new ServerPermissions(server.Id.ToString(), server.Name));
            }
            if (!_permissionsDict[server].RolePermissions.ContainsKey(role.Id.ToString()))
                _permissionsDict[server].RolePermissions.Add(role.Id.ToString(), new Permissions(role.Name));

            var commands = _permissionsDict[server].RolePermissions[role.Id.ToString()].commands;

            if (commands.ContainsKey(commandName))
                commands[commandName] = value;
            else
                commands.Add(commandName, value);
            WriteServerToJson(server);
        }

        public static void SetUserModulePermission(User user, string moduleName, bool value) {
            var server = user.Server;
            if (!_permissionsDict.ContainsKey(server)) {
                _permissionsDict.TryAdd(server, new ServerPermissions(server.Id.ToString(), server.Name));
            }
            if (!_permissionsDict[server].UserPermissions.ContainsKey(user.Id.ToString()))
                _permissionsDict[server].UserPermissions.Add(user.Id.ToString(), new Permissions(user.Name));

            var modules = _permissionsDict[server].UserPermissions[user.Id.ToString()].modules;

            if (modules.ContainsKey(moduleName))
                modules[moduleName] = value;
            else
                modules.Add(moduleName, value);
            WriteServerToJson(server);
        }

        public static void SetUserCommandPermission(User user, string commandName, bool value) {
            var server = user.Server;
            if (!_permissionsDict.ContainsKey(server)) {
                _permissionsDict.TryAdd(server, new ServerPermissions(server.Id.ToString(), server.Name));
            }
            if (!_permissionsDict[server].UserPermissions.ContainsKey(user.Id.ToString()))
                _permissionsDict[server].UserPermissions.Add(user.Id.ToString(), new Permissions(user.Name));

            var commands = _permissionsDict[server].UserPermissions[user.Id.ToString()].commands;

            if (commands.ContainsKey(commandName))
                commands[commandName] = value;
            else
                commands.Add(commandName, value);
            WriteServerToJson(server);
        }
    }
    /// <summary>
    /// Holds a permission list
    /// </summary>
    public class Permissions {
        /// <summary>
        /// Name of the parent object whose permissions these are
        /// </summary>
        public string ParentName { get; set; }
        /// <summary>
        /// Module name with allowed/disallowed
        /// </summary>
        public Dictionary<string, bool> modules { get; set; }
        /// <summary>
        /// Command name with allowed/disallowed
        /// </summary>
        public Dictionary<string, bool> commands { get; set; }

        public Permissions(string name) {
            ParentName = name;
            modules = new Dictionary<string, bool>();
            commands = new Dictionary<string, bool>();
        }
    }

    public class ServerPermissions {
        /// <summary>
        /// The guy who can edit the permissions
        /// </summary>
        public string PermissionsControllerRoleName { get; set; }
        /// <summary>
        /// Does it print the error when a restriction occurs
        /// </summary>
        public bool Verbose { get; set; }
        /// <summary>
        /// The id of the thing (user/server/channel)
        /// </summary>
        public string Id { get; set; } //a string because of the role name.
        /// <summary>
        /// Permission object bound to the id of something/role name
        /// </summary>
        public Permissions Permissions { get; set; }

        public Dictionary<string, Permissions> UserPermissions { get; set; }
        public Dictionary<string, Permissions> ChannelPermissions { get; set; }
        public Dictionary<string, Permissions> RolePermissions { get; set; }

        public ServerPermissions(string id, string name) {
            Id = id;
            PermissionsControllerRoleName = "PermissionsKing";
            Verbose = true;

            Permissions = new Permissions(name);
            UserPermissions = new Dictionary<string, Permissions>();
            ChannelPermissions = new Dictionary<string, Permissions>();
            RolePermissions = new Dictionary<string, Permissions>();
        }
    }
}