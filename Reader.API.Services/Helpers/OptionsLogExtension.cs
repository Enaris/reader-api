using Reader.API.DataAccess.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reader.API.Services.Helpers
{
    public static class OptionsLogExtension
    {
        public static string GetSpeedType(this OptionsLog optionsLog)
        {
            return optionsLog.InitialCPM == -1 ? "WPM" : "CPM";
        }

        public static int GetInitialSpeed(this OptionsLog optionsLog)
        {
            return optionsLog.InitialCPM == -1 ? optionsLog.InitialWPM : optionsLog.InitialCPM;
        }

        public static int GetTargetSpeed(this OptionsLog optionsLog)
        {
            return optionsLog.TargetCPM == -1 ? optionsLog.TargetWPM : optionsLog.TargetCPM;
        }

    }
}
