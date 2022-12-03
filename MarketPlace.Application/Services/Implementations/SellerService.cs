using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Common;
using MarketPlace.DataLayer.DTOs.Paging;
using MarketPlace.DataLayer.DTOs.Seller;
using MarketPlace.DataLayer.Entities.Account;
using MarketPlace.DataLayer.Entities.Store;
using MarketPlace.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Implementations
{
    public class SellerService : ISellerService
    {
        #region constructor
        private readonly IGenericRepository<Seller> _sellerRepository;
        private readonly IGenericRepository<User> _userRepository;

        public SellerService(IGenericRepository<Seller> sellerRepository, IGenericRepository<User> userRepository)
        {
            _sellerRepository = sellerRepository;
            _userRepository = userRepository;
        }
        #endregion

        #region seller
        public async Task<RequestSellerResult> AddNewSellerRequest(RequestSellerDTO seller, long userId)
        {
            var user = await _userRepository.GetEntityById(userId);
            if (user.IsBlocked) return RequestSellerResult.HasNotPermision;

            var HasUnderProgressRequest = await _sellerRepository.GetQuery().AsQueryable().AnyAsync(
                s => s.users.Id == userId && s.StoreAcceptanceState == StoreAcceptanceState.UnderProgress);
            if (HasUnderProgressRequest) return RequestSellerResult.HasUnderProgress;

            var newSeller = new Seller
            {
                UserId = userId,
                StoreName = seller.StoreName,
                Address = seller.Address,
                Phone = seller.Phone,
                StoreAcceptanceState = StoreAcceptanceState.UnderProgress
            };

            await _sellerRepository.AddEntity(newSeller);
            await _sellerRepository.SaveChanges();

            return RequestSellerResult.Success;
        }

        public async Task<FilterSellerDTO> FilterSellers(FilterSellerDTO filter)
        {
            var query = _sellerRepository.GetQuery()
               .Include(s => s.users)
               .AsQueryable();

            #region state

            switch (filter.State)
            {
                case FilterSellerState.All:
                    query = query.Where(s => !s.IsDelete);
                    break;
                case FilterSellerState.Accepted:
                    query = query.Where(s => s.StoreAcceptanceState == StoreAcceptanceState.Accepted && !s.IsDelete);
                    break;

                case FilterSellerState.UnderProgress:
                    query = query.Where(s => s.StoreAcceptanceState == StoreAcceptanceState.UnderProgress && !s.IsDelete);
                    break;
                case FilterSellerState.Rejected:
                    query = query.Where(s => s.StoreAcceptanceState == StoreAcceptanceState.Rejected && !s.IsDelete);
                    break;
            }

            #endregion

            #region filter

            if (filter.UserId != null && filter.UserId != 0)
                query = query.Where(s => s.UserId == filter.UserId);

            if (!string.IsNullOrEmpty(filter.StoreName))
                query = query.Where(s => EF.Functions.Like(s.StoreName, $"%{filter.StoreName}%"));

            if (!string.IsNullOrEmpty(filter.Address))
                query = query.Where(s => EF.Functions.Like(s.Address, $"%{filter.Address}%"));

            if (!string.IsNullOrEmpty(filter.Phone))
                query = query.Where(s => EF.Functions.Like(s.Phone, $"%{filter.Phone}%"));

            if (!string.IsNullOrEmpty(filter.Mobile))
                query = query.Where(s => EF.Functions.Like(s.Mobile, $"%{filter.Mobile}%"));


            #endregion

            #region paging

            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filter.SetPaging(pager).SetSellers(allEntities);

        }

        public async Task<EditRequestSellerDTO> GetRequestSellerForEdit(long id, long CurrentUserId)
        {
            var seller = await _sellerRepository.GetEntityById(id);
            if (seller == null || seller.UserId != CurrentUserId) return null;

            return new EditRequestSellerDTO
            {
                Id=seller.Id,
                StoreName = seller.StoreName,
                Address = seller.Address,
                Phone = seller.Phone
            };
        }

        public async Task<EditRequestResult> EditRequestSeller(EditRequestSellerDTO request, long CurrentUserId)
        {
            var seller = await _sellerRepository.GetEntityById(request.Id);
            if (seller == null || seller.UserId != CurrentUserId) return EditRequestResult.NotFound;

            seller.StoreName = request.StoreName;
            seller.Phone = request.Phone;
            seller.Address = request.Address;
            seller.StoreAcceptanceState = StoreAcceptanceState.UnderProgress;

             _sellerRepository.UpdateEntity(seller);
            await _sellerRepository.SaveChanges();

            return EditRequestResult.success;

        }

        public async Task<bool> AcceptSellerRequest(long requestId)
        {
            var sellerReq = await _sellerRepository.GetEntityById(requestId);
            if(sellerReq != null)
            {
                sellerReq.StoreAcceptanceState = StoreAcceptanceState.Accepted;
                sellerReq.Description = "درخواست فروشندگی شما با موفقیت تایید شد.";
                 _sellerRepository.UpdateEntity(sellerReq);
                await _sellerRepository.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<bool> RejectSellerRequest(RejectItemDTO reject)
        {
            var sellerReq = await _sellerRepository.GetEntityById(reject.Id);
            if (sellerReq != null)
            {
                sellerReq.StoreAcceptanceState = StoreAcceptanceState.Rejected;
                sellerReq.Description = reject.RejectMessage;
                _sellerRepository.UpdateEntity(sellerReq);
                await _sellerRepository.SaveChanges();
                return true;
            }
            return false;
        }

        #endregion


        #region dispose
        public async ValueTask DisposeAsync()
        {
            await _sellerRepository.DisposeAsync();
            await _userRepository.DisposeAsync();
        }




        #endregion
    }
}
