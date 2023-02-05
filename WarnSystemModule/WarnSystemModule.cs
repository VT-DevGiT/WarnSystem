using Neuron.Core.Modules;
using Neuron.Modules.Commands;
using Neuron.Modules.Configs;
using Synapse3.SynapseModule;
using System.Xml.Linq;
using YamlDotNet.Core.Tokens;

namespace WarnSystemModule
{
    [Module(
    Name = "WarnSystem Module",
    Author = "VT",
    Dependencies = new[]
    {
        typeof(Synapse),
    },
    Description = "The base module to manage the Warn and the db",
    Version = "1.0.0",
    Repository = "https://github.com/VT-DevGiT/WarnSystem"
    )]

    public class WarnSystemModule : Module
    {

        public WarnService WarnService { get; private set; }

        public override void Enable()
        {
            WarnService = Synapse.Get<WarnService>();
        }

    }
}