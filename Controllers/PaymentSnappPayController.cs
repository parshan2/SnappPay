using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using NopPlus.Plugin.SnappPay.Models;
using Nop.Services;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace NopPlus.Plugin.SnappPay.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class PaymentSnappPayController : BasePaymentController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        #endregion

        #region Ctor

        public PaymentSnappPayController(ILocalizationService localizationService,
            INotificationService notificationService,
            IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
        }

        #endregion

        #region Methods

        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var pluginSettings = await _settingService.LoadSettingAsync<PluginSettings>(storeScope);

            var model = new ConfigurationModel
            {
                Username = pluginSettings.Username,
                Password = pluginSettings.Password,
                ClientId = pluginSettings.ClientId,
                ClientSecret = pluginSettings.ClientSecret,
                AdditionalFee = pluginSettings.AdditionalFee,
                AdditionalFeePercentage = pluginSettings.AdditionalFeePercentage,
                ActiveStoreScopeConfiguration = storeScope
            };
            if (storeScope > 0)
            {
                model.Username_OverrideForStore = await _settingService.SettingExistsAsync(pluginSettings, x => x.Username, storeScope);
                model.Password_OverrideForStore = await _settingService.SettingExistsAsync(pluginSettings, x => x.Password, storeScope);
                model.ClientId_OverrideForStore = await _settingService.SettingExistsAsync(pluginSettings, x => x.ClientId, storeScope);
                model.ClientSecret_OverrideForStore = await _settingService.SettingExistsAsync(pluginSettings, x => x.ClientSecret, storeScope);
                model.AdditionalFee_OverrideForStore = await _settingService.SettingExistsAsync(pluginSettings, x => x.AdditionalFee, storeScope);
                model.AdditionalFeePercentage_OverrideForStore = await _settingService.SettingExistsAsync(pluginSettings, x => x.AdditionalFeePercentage, storeScope);
            }

            return View("~/Plugins/NopPlus.SnappPay/Views/Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return await Configure();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var pluginSettings = await _settingService.LoadSettingAsync<PluginSettings>(storeScope);

            //save settings
            pluginSettings.Username = model.Username;
            pluginSettings.Password = model.Password;
            pluginSettings.ClientId = model.ClientId;
            pluginSettings.ClientSecret = model.ClientSecret;
            pluginSettings.AdditionalFee = model.AdditionalFee;
            pluginSettings.AdditionalFeePercentage = model.AdditionalFeePercentage;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */

            await _settingService.SaveSettingOverridablePerStoreAsync(pluginSettings, x => x.Username, model.Username_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(pluginSettings, x => x.Password, model.Password_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(pluginSettings, x => x.ClientId, model.ClientId_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(pluginSettings, x => x.ClientSecret, model.ClientSecret_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(pluginSettings, x => x.AdditionalFee, model.AdditionalFee_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(pluginSettings, x => x.AdditionalFeePercentage, model.AdditionalFeePercentage_OverrideForStore, storeScope, false);

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }

        #endregion
    }
}