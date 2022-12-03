using System;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface ISmsService 
    {
        Task SendVerificationSMS(string mobile,string activationCode);
        Task SendUserPasswordSMS(string mobile,string password);
    }
}
