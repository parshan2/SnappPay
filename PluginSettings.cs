using Nop.Core.Configuration;

namespace NopPlus.Plugin.SnappPay
{
    /// <summary>
    /// Represents settings of snapppay payment plugin
    /// </summary>
    public class PluginSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to "additional fee" is specified as percentage. true - percentage, false - fixed value.
        /// </summary>
        public bool AdditionalFeePercentage { get; set; }

        /// <summary>
        /// Gets or sets an additional fee
        /// </summary>
        public decimal AdditionalFee { get; set; }
    }
}
