using MarketPlace.DataLayer.DTOs.Common;
using MarketPlace.DataLayer.DTOs.Seller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface ISellerService : IAsyncDisposable
    {
        Task<RequestSellerResult> AddNewSellerRequest(RequestSellerDTO seller, long UserId);
        Task<FilterSellerDTO> FilterSellers(FilterSellerDTO filter);
        Task<EditRequestSellerDTO> GetRequestSellerForEdit(long id,long CurrentUserId);
        Task<EditRequestResult> EditRequestSeller(EditRequestSellerDTO request,long CurrentUserId);
        Task<bool> AcceptSellerRequest(long requestId);
        Task<bool> RejectSellerRequest(RejectItemDTO reject);
    }
}
