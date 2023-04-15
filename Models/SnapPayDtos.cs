using Newtonsoft.Json;
using System.Collections.Generic;

namespace NopPlus.Plugin.SnappPay.Dtos
{
    public class TokenDto
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpireIn { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("iat")]
        public string Iat { get; set; }

        [JsonProperty("jti")]
        public string Jti { get; set; }
    }

    public class PaymentRequestDto
    {
        public PaymentRequestDto()
        {
            CartList = new List<CartList>();
        }

        /// <summary>
        /// required
        /// </summary>
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        /// <summary>
        /// required
        /// </summary>
        [JsonProperty("amount")]
        public int Amount { get; set; }

        /// <summary>
        /// required
        /// </summary>
        [JsonProperty("returnURL")]
        public string ReturnURL { get; set; }

        /// <summary>
        /// required
        /// </summary>
        [JsonProperty("paymentMethodTypeDto")]
        public string PaymentMethodTypeDto { get; set; }

        /// <summary>
        /// required
        /// </summary>
        [JsonProperty("cartList")]
        public List<CartList> CartList { get; set; }

        [JsonProperty("discountAmount")]
        public int DiscountAmount { get; set; }

        [JsonProperty("externalSourceAmount")]
        public int ExternalSourceAmount { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }
    }

    public class CartList
    {
        public CartList()
        {
            CartItems = new List<CartItem>();
        }
        /// <summary>
        /// required
        /// </summary>
        [JsonProperty("cartId")]
        public int CartId { get; set; }

        /// <summary>
        /// required
        /// </summary>
        [JsonProperty("cartItems")]
        public List<CartItem> CartItems { get; set; }

        /// <summary>
        /// required
        /// </summary>
        [JsonProperty("totalAmount")]
        public int TotalAmount { get; set; }

        [JsonProperty("isShipmentIncluded")]
        public bool IsShipmentIncluded { get; set; }

        [JsonProperty("shippingAmount")]
        public int ShippingAmount { get; set; }

        [JsonProperty("isTaxIncluded")]
        public bool IsTaxIncluded { get; set; }

        [JsonProperty("taxAmount")]
        public int TaxAmount { get; set; }
    }

    public class CartItem
    {
        /// <summary>
        /// required
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// required
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// required
        /// </summary>
        [JsonProperty("count")]
        public int Count { get; set; }

        /// <summary>
        /// required
        /// </summary>
        [JsonProperty("amount")]
        public int Amount { get; set; }

        /// <summary>
        /// required
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; }

        //[JsonProperty("commissionType")]
        //public int CommissionType { get; set; }
    }

    public class ResultDto<T>
    {
        [JsonProperty("successful")]
        public bool Successfull { get; set; }

        [JsonProperty("response")]
        public T Response { get; set; }

        [JsonProperty("errorData")]
        public ErrorDataDto ErrorData { get; set; }
    }

    public class ErrorDataDto
    {
        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
    }

    public class EligibleDto
    {
        [JsonProperty("eligible")]
        public bool Eligible { get; set; }

        [JsonProperty("title_message")]
        public string TitleMessage { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public class PaymentTokenDto
    {
        [JsonProperty("paymentToken")]
        public string PaymentToken { get; set; }

        [JsonProperty("paymentPageUrl")]
        public string PaymentPageUrl { get; set; }
    }

    public class VerifyDto
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }
    }
}
