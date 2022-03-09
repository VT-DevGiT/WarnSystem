using Synapse.Api.Plugin;

namespace WarnSystem
{
    [PluginInformation(
        Name = "WarnSystem",
        Author = "Antoniofo",
        Description = "Add a warn system",
        LoadPriority = 0,
        SynapseMajor = SynapseController.SynapseMajor,
        SynapseMinor = SynapseController.SynapseMinor,
        SynapsePatch = SynapseController.SynapsePatch,
        Version = "v1.0.0"
        )]
    public class Plugin : AbstractPlugin
    {
        public override void Load()
        {
            base.Load();
        }
    }
}
