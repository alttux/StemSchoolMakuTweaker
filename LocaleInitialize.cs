using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakuTweaker
{
    internal class LocaleInitialize
    {
        public class LocalizationWrapper
        {
            public List<BaseLocalization> Base { get; set; }
            public List<ErrorsLocalization> Errors { get; set; }
            public List<PCInfoLocalization> PCInfo { get; set; }
            public List<UpdateLocalization> Update { get; set; }
            public List<CategoryLocalization> Categories { get; set; }
            public List<ExplorerLocalization> Explorer { get; set; }
            public List<EnvLocalization> Env { get; set; }
        }

        public class BaseLocalization
        {
            public string aboutButton { get; set; }
            public string restartButton { get; set; }
            public string restartExp { get; set; }
            public string loading { get; set; }
            public string needExpRestart { get; set; }
            public string needReboot { get; set; }
        }

        public class ErrorsLocalization
        {
            public string displayVersion { get; set; }
            public string cantRegistry { get; set; }
            public string noConnection { get; set; }
            public string na { get; set; }
            public string cantUpdate { get; set; }
            public string unsupWin { get; set; }
            public string unsupFeature { get; set; }
            public string andup { get; set; }
            public string anddown { get; set; }
        }

        public class PCInfoLocalization
        {
            public string edition { get; set; }
            public string build { get; set; }
            public string pcName { get; set; }
            public string region { get; set; }
            public string activation { get; set; }
            public string hyperv { get; set; }
            public string complete { get; set; }
            public string notComplete { get; set; }
            public string on { get; set; }
            public string off { get; set; }
        }

        public class UpdateLocalization
        {
            public string autoDialog { get; set; }
            public string newUpdate { get; set; }
            public string oldVersion { get; set; }
            public string build { get; set; }
            public string newVersion { get; set; }
            public string changelog { get; set; }
            public string download { get; set; }
            public string later { get; set; }
            public string checking { get; set; }
            public string noUpdates { get; set; }
            public string yesUpdates { get; set; }
            public string noInfo { get; set; }
            public string noConnection { get; set; }
        }

        public class CategoryLocalization
        {
            public string aboutos { get; set; }
            public string autosetup { get; set; }
            public string explorer { get; set; }
            public string desktop { get; set; }
            public string uac { get; set; }
            public string other { get; set; }
            public string context { get; set; }
            public string activation { get; set; }
            public string clean { get; set; }
            public string settings { get; set; }
            public string about { get; set; }
            public string reboot { get; set; }
            public string restart { get; set; }
        }
        public class ExplorerLocalization
        {
            public string hidelibrary { get; set; }
            public string hiddenfiles { get; set; }
            public string hiddensystem { get; set; }
            public string thispc { get; set; }
            public string winv { get; set; }
            public string showext { get; set; }
            public string gallery { get; set; }

        }
        public class EnvLocalization
        {
            public string pcdesk { get; set; }
            public string nolink { get; set; }
            public string noarrow { get; set; }
            public string minrec { get; set; }
            public string minrecS { get; set; }
            public string nolt { get; set; }
            public string sec { get; set; }
            public string hidebut { get; set; }
            public string hugemini { get; set; }
            public string redmini { get; set; }
            public string noad { get; set; }
            public string noonline { get; set; }
            public string nocont { get; set; }
        }
    }
}
