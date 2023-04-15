using System.Threading.Tasks;
using NopPlus.Plugin.SnappPay.Dtos;

namespace NopPlus.Plugin.SnappPay
{
    public interface ISnappPayService
    {

        Task<ResultDto<EligibleDto>> EligibleAsync(int amount);

        Task<ResultDto<PaymentTokenDto>> GetPaymentTokenAsync(PaymentRequestDto request);

        Task<ResultDto<VerifyDto>> VerifyPaymentAsync(string paymentToken);

        Task<ResultDto<VerifyDto>> SettlePaymentAsync(string paymentToken);

        Task<ResultDto<VerifyDto>> RevertPaymentAsync(string paymentToken);
    }
}
