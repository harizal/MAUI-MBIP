using System;
using System.IO;
using AndroidCompound5.AimforceUtils;
using AndroidCompound5.BusinessObject.DTOs;
using AndroidCompound5.DataAccess;

namespace AndroidCompound5
{
    /// <summary>
    /// ConfigApp = "ConfigAppMps.xml"
    /// </summary>
    public static class ConfigAccess
    {
        public static ConfigAppDto? GetConfigAccess()
        {
            return DbContextProvider.Instance.ConfigApp?.FirstOrDefault() ?? null;
		}
    }
}