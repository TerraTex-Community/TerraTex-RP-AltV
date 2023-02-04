using System.Collections;
using System.Xml.Linq;
using AltV.Net.Client;
using AltV.Net.Client.Elements.Data;
using AltV.Net.Client.Elements.Interfaces;
using TerraTex_RolePlay_AltV_Client.utils;

namespace TerraTex_RolePlay_AltV_Client
{
    public class TerraTexRolePlayResource: Resource
    {

        public override void OnStart()
        {
            TerraTexClientInit.RunInitFunctions();
            Console.WriteLine("TerraTex Client started");
        }


        public override void OnStop()
        {
            Console.WriteLine("TerraTex Client stopped");
        }
    }
}