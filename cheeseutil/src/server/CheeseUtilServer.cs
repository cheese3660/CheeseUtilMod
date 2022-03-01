using LogicAPI.Server;
using LICC;
using System.Collections.Generic;
using System.IO;
namespace CheeseUtilMod.Server
{
    class CheeseUtilServer : ServerMod
    {
        public static List<FileLoadable> fileLoadables = new List<FileLoadable>();
        protected override void Initialize()
        {
            Logger.Info("Cheese Util Mod - Server Loaded");
        }

        [Command("loadram", Description = "Loads a file into any RAM components with the L pin active, does not clear out memory after the end of the file")]
        public static void loadram(string file)
        {
            LineWriter lineWriter = LConsole.BeginLine();
            if (File.Exists(file))
            {
                var bs = File.ReadAllBytes(file);
                foreach (var item in fileLoadables) item.Load(bs,lineWriter);
            }
            else
            {
                lineWriter.WriteLine($"Attempted to load file {file} that does not exist!");
            }
            lineWriter.End();
        }
    }
}
