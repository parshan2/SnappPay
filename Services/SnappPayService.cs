using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing.Charts;
using Newtonsoft.Json;
using Nop.Core.Caching;
using Nop.Services.Messages;
using NopPlus.Plugin.Attachments;
using NopPlus.Plugin.SnappPay.Dtos;

namespace NopPlus.Plugin.SnappPay
{
    public class SnappPayService : ISnappPayService
    {
        private readonly PluginSettings _pluginSettings;
        private readonly HttpClient _client;
        private readonly IStaticCacheManager _staticCacheManager;


        public SnappPayService(PluginSettings pluginSettings,
            IStaticCacheManager staticCacheManager)
        {
            _pluginSettings = pluginSettings;
            _staticCacheManager = staticCacheManager;

            _client = new HttpClient();
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

        #region Utilities

        private async Task SetAuthorizationHeader(HttpClient client)
        {
            //get access token
            var token = await GetAccessTokenAsync();

            //authorization
            _client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", $"{token.AccessToken}");
        }

        private async Task<TokenDto> GetAccessTokenAsync()
        {
            var token = await _staticCacheManager.GetAsync(PluginDefaults.TokenExpireTimeCacheKey,
                async () => await Task.FromResult<TokenDto>(null));

            if (token != null)
                return token;

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "scope", "online-merchant" },
                { "username", _pluginSettings.Username },
                { "password", _pluginSettings.Password },
            });

            //authorization
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes($"{_pluginSettings.ClientId}:{_pluginSettings.ClientSecret}")));

            var response = _client.PostAsync($"{_pluginSettings.BaseUrl}/api/online/v1/oauth/token", content).Result;
            if (!response.IsSuccessStatusCode)
            {

            }

            var tokenDto = JsonConvert.DeserializeObject<TokenDto>(response.Content.ReadAsStringAsync().Result);

            //cache token
            PluginDefaults.TokenExpireTimeCacheKey.CacheTime = tokenDto.ExpireIn - 10;
            await _staticCacheManager.SetAsync(PluginDefaults.TokenExpireTimeCacheKey, tokenDto);

            return tokenDto;
        }

        #endregion


        public async Task<ResultDto<EligibleDto>> EligibleAsync(int amount)
        {

            //authorization
            await SetAuthorizationHeader(_client);

            var response = _client.GetAsync($"{_pluginSettings.BaseUrl}/api/online/offer/v1/eligible?amount={amount}").Result;
            if (!response.IsSuccessStatusCode)
            {

            }

            return JsonConvert.DeserializeObject<ResultDto<EligibleDto>>(response.Content.ReadAsStringAsync().Result);
        }


        public async Task<ResultDto<PaymentTokenDto>> GetPaymentTokenAsync(PaymentRequestDto request)
        {
            //authorization
            await SetAuthorizationHeader(_client);

            //serialize
            var jsonString = JsonConvert.SerializeObject(request);
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = _client.PostAsync($"{_pluginSettings.BaseUrl}/api/online/payment/v1/token", content).Result;
            if (!response.IsSuccessStatusCode)
            {

            }

            return JsonConvert.DeserializeObject<ResultDto<PaymentTokenDto>>(response.Content.ReadAsStringAsync().Result);
        }

        public async Task<ResultDto<VerifyDto>> VerifyPaymentAsync(string paymentToken)
        {
            //authorization
            await SetAuthorizationHeader(_client);

            //serialize
            var jsonString = JsonConvert.SerializeObject(new { paymentToken = paymentToken });
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = _client.PostAsync($"{_pluginSettings.BaseUrl}/api/online/payment/v1/verify", content).Result;
            if (!response.IsSuccessStatusCode)
            {

            }

            return JsonConvert.DeserializeObject<ResultDto<VerifyDto>>(response.Content.ReadAsStringAsync().Result);
        }

        public async Task<ResultDto<VerifyDto>> SettlePaymentAsync(string paymentToken)
        {
            //authorization
            await SetAuthorizationHeader(_client);

            //serialize
            var jsonString = JsonConvert.SerializeObject(new { paymentToken = paymentToken });
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = _client.PostAsync($"{_pluginSettings.BaseUrl}/api/online/payment/v1/settle", content).Result;
            if (!response.IsSuccessStatusCode)
            {

            }

            return JsonConvert.DeserializeObject<ResultDto<VerifyDto>>(response.Content.ReadAsStringAsync().Result);
        }

        public async Task<ResultDto<VerifyDto>> RevertPaymentAsync(string paymentToken)
        {
            //authorization
            await SetAuthorizationHeader(_client);

            //serialize
            var jsonString = JsonConvert.SerializeObject(new { paymentToken = paymentToken });
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = _client.PostAsync($"{_pluginSettings.BaseUrl}/api/online/payment/v1/revert", content).Result;
            if (!response.IsSuccessStatusCode)
            {

            }

            return JsonConvert.DeserializeObject<ResultDto<VerifyDto>>(response.Content.ReadAsStringAsync().Result);
        }
    }
}
