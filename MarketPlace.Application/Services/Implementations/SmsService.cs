

using MarketPlace.Application.Services.Interfaces;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Implementations
{
    public class SmsService : ISmsService
    {

        private string apikey = "5A733367794D364F786B45416953764B4C36632B2B66576856534A4762537558777653453671457738504D3D";


        public async Task SendVerificationSMS(string mobile, string activationCode)
        {
            Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apikey);
            await api.VerifyLookup(mobile, activationCode, "MarketTemplate");

        }

        public async Task SendUserPasswordSMS(string mobile, string password) 
        {
            Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apikey);
            await api.VerifyLookup(mobile, password, "NewPassword");

        }
    }
}
