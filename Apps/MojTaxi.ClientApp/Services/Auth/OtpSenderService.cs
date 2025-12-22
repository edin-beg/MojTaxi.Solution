using MojTaxi.ClientApp.ViewModels;

namespace MojTaxi.ClientApp.Services.Auth
{
    public sealed class OtpSenderService : IOtpSenderService
    {
        private readonly IOtpSender _smsSender;
        private readonly IOtpSender _emailSender;

        public OtpSenderService(
            InfobipSmsSender smsSender,
            EmailOtpSender emailSender)
        {
            _smsSender = smsSender;
            _emailSender = emailSender;
        }

        public async Task SendOtpAsync(LoginMethod method, string destination)
        {
            var otp = GenerateOtp();

            if (method == LoginMethod.Sms)
                await _smsSender.SendAsync(destination, otp);
            else
                await _emailSender.SendAsync(destination, otp);

            // TODO:
            // - zapamti OTP u SecureStorage
            // - ili lokalni cache (za Verify)
        }

        private static string GenerateOtp()
            => Random.Shared.Next(100000, 999999).ToString();
    }
}
