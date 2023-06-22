using LogicAPI.Server;
using System.IO;
using System.Reflection;

namespace CheeseLoader.Server
{
    public class CheeseLoader : ServerMod
    {
        protected override void Initialize()
        {
            Logger.Info("Cheese Loader Mod - Server Loaded");
            var location = Assembly.GetEntryAssembly().Location;
            Logger.Info("Found server at - " + location);
            var directory = Path.GetDirectoryName(location);
            foreach (var path in Directory.GetFiles(directory))
            {
                if (path.EndsWith(".dll"))
                {
                    var asm_name = Path.GetFileNameWithoutExtension(path);
                    Logger.Trace("work - " + asm_name);
                    try
                    {
                        Assembly.Load(asm_name);
                    }
                    catch (FileLoadException)
                    {
                        Logger.Trace("nope");
                    }
                    catch (FileNotFoundException)
                    {
                        Logger.Trace("nope");
                    }
                }
            }
        }
    }
}
