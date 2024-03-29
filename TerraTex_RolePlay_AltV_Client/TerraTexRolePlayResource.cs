﻿using System.Collections;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using AltV.Net.Client;
using AltV.Net.Client.Elements.Data;
using AltV.Net.Client.Elements.Interfaces;
using TerraTex_RolePlay_AltV_Client.Environment;
using TerraTex_RolePlay_AltV_Client.User;

namespace TerraTex_RolePlay_AltV_Client
{
    public class TerraTexRolePlayResource: Resource
    {

        public override void OnStart()
        {
            RunInitFunctions();
            Console.WriteLine("TerraTex Client started");
        }


        public override void OnStop()
        {
            Console.WriteLine("TerraTex Client stopped");
        }

        // @todo: workaround until I Script exists - create Manual Instances of every Client Script
        private void RunInitFunctions()
        {
            new Weather();
            new Login();
            new Register();
            new Chat();
        }
    }
}