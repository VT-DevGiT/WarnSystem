using Neuron.Core.Meta;
using Syml;
using System.ComponentModel;

namespace WarnSystem
{
    [Automatic]
    [DocumentSection("WarnSystem")]
    public class PluginConfig : IDocumentSection
    {
        [Description("Allow the player to use the client console command to his own warn")]
        public bool PlayerCommand { get; set; } = true;

        [Description("Player have a 5 second message at the first connection to tell them that WarnSystem will register data linked to him (If you are in EU, YOU HAVE TO DISPLAY THAT MESSAGE)")]
        public bool DisclamerAtFirstConnection { get; set; } = true;
    }
}