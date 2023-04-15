using Nop.Core.Caching;

namespace NopPlus.Plugin.Attachments
{
    /// <summary>
    /// Represents plugin constants
    /// </summary>
    public class PluginDefaults
    {
        /// <summary>
        /// Gets the plugin system name
        /// </summary>
        public static string SystemName => "NopPlus.SnappPay";

        /// <summary>
        /// Gets a key for caching
        /// </summary>
        /// <remarks>
        /// </remarks>
        public static CacheKey TokenExpireTimeCacheKey => new("NopPlus.SnappPay.tokenexpiretime");

    }
}