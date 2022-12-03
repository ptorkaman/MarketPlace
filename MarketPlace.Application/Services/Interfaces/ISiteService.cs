using MarketPlace.DataLayer.Entities.Site;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface ISiteService : IAsyncDisposable
    {
        #region site setting
        Task<SiteSetting> GetDefaultSiteSetting();

        #endregion

        #region Slider

        Task<List<Slider>> GetActiveSliders();


        #endregion

        #region SiteBanner

        Task<List<SiteBanner>> GetSiteBannersByPlacement(List<BannerPlacement> placements);

        #endregion

    }
}
