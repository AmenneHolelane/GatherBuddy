using System;
using System.Linq;
using Dalamud;
using Dalamud.Plugin;

namespace GatherBuddy.Utility
{
    // ReSharper disable once InconsistentNaming
    public class FFName
    {
        public FFName()
        { }

        public string this[ClientLanguage lang]
        {
            get => Name(lang);
            set => SetName(lang, value);
        }

        public bool AnyEmpty()
        {
            return Enum.GetValues(typeof(ClientLanguage))
                .Cast<ClientLanguage>()
                .Any(lang => Name(lang).Length == 0);
        }

        public override string ToString()
            => $"{Name(ClientLanguage.English)}|{Name(ClientLanguage.German)}|{Name(ClientLanguage.French)}|{Name(ClientLanguage.Japanese)}";

        public static FFName FromPlaceName(DalamudPluginInterface pi, uint id)
        {
            var name = new FFName();
            foreach (ClientLanguage lang in Enum.GetValues(typeof(ClientLanguage)))
            {
                var row = pi.Data.GetExcelSheet<Lumina.Excel.GeneratedSheets.PlaceName>(lang).GetRow(id);
                name[lang] = row?.Name ?? "";
            }

            return name;
        }

        private string Name(ClientLanguage lang)
            => _nameList[(int) lang];

        private void SetName(ClientLanguage lang, string name)
        {
            if (lang == ClientLanguage.German || lang == ClientLanguage.French)
                name = Util.RemoveSplitMarkers(name);
            _nameList[(int) lang] = Util.RemoveItalics(name);
        }

        private readonly string[] _nameList = new string[4]
        {
            "",
            "",
            "",
            "",
        };
    }
}
