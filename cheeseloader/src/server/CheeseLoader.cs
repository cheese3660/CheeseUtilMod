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
            var directory = System.IO.Path.GetDirectoryName(location);
            foreach (var path in System.IO.Directory.GetFiles(directory))
            {
                if (path.EndsWith(".dll"))
                {
                    var asm_name = System.IO.Path.GetFileNameWithoutExtension(path);
                    Logger.Trace("work - " + asm_name);
                    try
                    {
                        Assembly.Load(asm_name);
                    } catch (FileLoadException fle)
                    {
                        Logger.Trace("nope");
                    }
                    catch (FileNotFoundException fle)
                    {
                        Logger.Trace("nope");
                    }
                }
            }
        }
    }
}
