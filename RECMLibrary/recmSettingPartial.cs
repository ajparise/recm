using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;
using System.Collections;

namespace Parise.RaisersEdge.ConnectionMonitor.Data.Entities
{
    ///// <summary>
    ///// Partial class for recmSetting Object
    ///// </summary>
    //public partial class recmSetting
    //{
    //    /// <summary>
    //    /// Overloaded Constructor
    //    /// </summary>
    //    /// <param name="setting">Setting value to use</param>
    //    public recmSetting(MonitorSettings setting)
    //    {
    //        this.SettingName = setting.ToString();
    //    }
        
    //    /// <summary>
    //    /// Parses the SettingName field into recmSetting.Setting type
    //    /// </summary>
    //    public MonitorSettings SettingEnum
    //    {
    //        get
    //        {
    //            return (MonitorSettings)Enum.Parse(typeof(MonitorSettings), this._SettingName);
    //        }
    //        set
    //        {
    //            this.SettingName = value.ToString();
    //        }
    //    }
    //}

    ///// <summary>
    ///// Extensions for recmSetting table
    ///// Must include "using Parise.RaisersEdge.ConnectionMonitor.Data.Entities" to use extensions
    ///// </summary>
    //public static class recmSettingExtensions
    //{
    //    /// <summary>
    //    /// Creates all settings not specified in the DB table
    //    /// </summary>
    //    /// <param name="table">recmSetting table entity</param>
    //    /// <returns>Enumerable recmSetting</returns>
    //    public static IEnumerable<recmSetting> CreateMissingSettingsOnSubmit(this Table<Parise.RaisersEdge.ConnectionMonitor.Data.Entities.recmSetting> table)
    //    {
    //        var settings = Enum.GetValues(typeof(MonitorSettings)).Cast<MonitorSettings>().Where(s => s != MonitorSettings.Unknown).Select(a => SetSettingByEnum(table, a, string.Empty)).AsEnumerable();
    //        return settings;
    //    }

    //    public static recmSetting SetSettingByEnum(this Table<Parise.RaisersEdge.ConnectionMonitor.Data.Entities.recmSetting> table, MonitorSettings setting, string value)
    //    {
    //        var s = findSetting(table, setting);
    //        s.Value = value;
    //        return s;
    //    }

    //    public static recmSetting GetSettingByEnum(this Table<Parise.RaisersEdge.ConnectionMonitor.Data.Entities.recmSetting> table, MonitorSettings setting)
    //    {
    //        return findSetting(table, setting);
    //    }

    //    private static recmSetting findSetting(this Table<Parise.RaisersEdge.ConnectionMonitor.Data.Entities.recmSetting> table, MonitorSettings toFind)
    //    {
    //        var foundSetting = table.ToList().Where(s => s.SettingEnum == toFind).FirstOrDefault();

    //        if (foundSetting == null)
    //        {
    //            foundSetting = new recmSetting(toFind);
    //            table.InsertOnSubmit(foundSetting);
    //        }

    //        return foundSetting;
    //    }
    //}

}
