using MarketPlace.DataLayer.DTOs.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Interfaces
{
    public interface IContactService : IAsyncDisposable
    {
        #region Contact us
        Task ContactUs(CreateContactUsDTO createContactUs, long? userId,string userIP);

        #endregion

        #region Ticket

        Task<AddTicketResult> AddTicket(AddTicketViewModelDTO addTicket, long userId);
        Task<FilterTicketDTO> FilterTicket(FilterTicketDTO filterTicket);
        Task<TicketDetailDTO> GetTicketForShow(long ticketId,long userId);
        Task<AnswerTicketResult> AnswerTicket(AnswerTicketDTO answer, long userId);
        #endregion
    }

}
