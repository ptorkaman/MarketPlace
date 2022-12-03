using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Contact;
using MarketPlace.DataLayer.DTOs.Paging;
using MarketPlace.DataLayer.Entities.Account;
using MarketPlace.DataLayer.Entities.Contact;
using MarketPlace.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Application.Services.Implementations
{
    public class ContactService : IContactService
    {
        #region constructor

        private readonly IGenericRepository<ContactUs> _contactUsRepository;
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IGenericRepository<TicketMessage> _ticketMessageRepository;

        public ContactService(IGenericRepository<ContactUs> contactUsRepository
            , IGenericRepository<Ticket> ticketRepository
            ,IGenericRepository<TicketMessage> ticketMessageRepository)
        {
            _contactUsRepository = contactUsRepository;
            _ticketRepository = ticketRepository;
            _ticketMessageRepository = ticketMessageRepository;
        }

        #endregion

        public async Task ContactUs(CreateContactUsDTO createContactUs, long? userId, string userIP)
        {
            var contact = new ContactUs { 
                UserId= userId != null && userId.Value !=0 ? userId.Value : (long?) null,
                UserIP=userIP,
                FullName=createContactUs.FullName,
                Subject=createContactUs.Subject,
                content=createContactUs.content
            };
            await _contactUsRepository.AddEntity(contact);
            await _contactUsRepository.SaveChanges();
        }


        #region ticket
        public async Task<AddTicketResult> AddTicket(AddTicketViewModelDTO addTicket, long userId)
        {
            if (string.IsNullOrEmpty(addTicket.Text)) return AddTicketResult.Error;

            Ticket ticket = new Ticket();
            ticket.OwnerId = userId;
            ticket.TicketPriority = addTicket.TicketPriority;
            ticket.TicketSection = addTicket.TicketSection;
            ticket.Title = addTicket.Title;
            ticket.IsReadByOwner = true;
            ticket.TicketState = TicketState.UnderProgress;

            await _ticketRepository.AddEntity(ticket);
            await _ticketRepository.SaveChanges();

            var newmessage =new TicketMessage() { 
            
                Text=addTicket.Text,
                TicketId=ticket.Id,
                SenderId=userId
            };
            await _ticketMessageRepository.AddEntity(newmessage);
            await _ticketMessageRepository.SaveChanges();

            return AddTicketResult.Success;
        }


        public async Task<FilterTicketDTO> FilterTicket(FilterTicketDTO filterTicket)
        {
            var query = _ticketRepository.GetQuery().AsQueryable();

            #region state
            switch (filterTicket.FilterTicketState)
            {
                case FilterTicketState.All:
                    break;
                case FilterTicketState.Deleted:
                    query = query.Where(s => s.IsDelete);
                    break;
                case FilterTicketState.NotDeleted:
                    query = query.Where(s => !s.IsDelete);
                    break;
            }
            #endregion

            #region OrderBy
            switch (filterTicket.OrderBy)
            {
                case FilterTicketOrder.CreateDate_ASC:
                    query = query.OrderBy(s => s.CreateDate);
                    break;
                case FilterTicketOrder.CreateDate_DSC:
                    query = query.OrderByDescending(s => s.CreateDate);
                    break;
            }
            #endregion

            #region filter
            if (filterTicket.TicketPriority != null)
                query = query.Where(s => s.TicketPriority == filterTicket.TicketPriority.Value);

            if (filterTicket.TicketSection != null)
                query = query.Where(s => s.TicketSection == filterTicket.TicketSection.Value);

            if (!string.IsNullOrEmpty(filterTicket.Title))
                query = query.Where(s => EF.Functions.Like(s.Title, $"%{filterTicket.Title}%"));

            #endregion

            #region paging

            var pager = Pager.Build(filterTicket.PageId, await query.CountAsync(), filterTicket.TakeEntity, filterTicket.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();

            #endregion

            return filterTicket.SetPaging(pager).SetTickets(allEntities);
        }

        public async Task<TicketDetailDTO> GetTicketForShow(long ticketId, long userId)
        {
            var Ticket = await _ticketRepository.GetQuery().AsQueryable().Include(s => s.Owner).
                 SingleOrDefaultAsync(s => s.Id == ticketId);

            if (Ticket == null || Ticket.OwnerId != userId) return null;

            return new TicketDetailDTO { 
                ticket=Ticket,
                ticketMessages = await _ticketMessageRepository.GetQuery().AsQueryable().
                                 OrderByDescending(s=>s.CreateDate).
                                 Where(s=>s.TicketId==ticketId && !s.IsDelete).ToListAsync()
            };

        }

        public async Task<AnswerTicketResult> AnswerTicket(AnswerTicketDTO answer, long userId)
        {
            var ticket = await _ticketRepository.GetEntityById(answer.Id);
            if (ticket == null) return AnswerTicketResult.NotFound;
            if (ticket.OwnerId != userId) return AnswerTicketResult.NotForUser;

            var ticketMessage = new TicketMessage
            {
                TicketId = ticket.Id,
                SenderId = userId,
                Text = answer.Text
            };

            await _ticketMessageRepository.AddEntity(ticketMessage);
            await _ticketMessageRepository.SaveChanges();

            ticket.IsReadByAdmin = false;
            ticket.IsReadByOwner = true;
            await _ticketRepository.SaveChanges();
            return AnswerTicketResult.Success;
        }
        #endregion

        #region dispose
        public async ValueTask DisposeAsync()
        {
            await _contactUsRepository.DisposeAsync();
        }
        #endregion
    }
}
