using MarketPlace.DataLayer.DTOs.Paging;
using MarketPlace.DataLayer.Entities.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.DTOs.Contact
{
    public class FilterTicketDTO :BasePaging
    {
        public long? UserId { get; set; }
        public string Title { get; set; }
        public TicketSection? TicketSection { get; set; }
        public TicketPriority? TicketPriority { get; set; }
        public List<Ticket> Tickets { get; set; }
        public FilterTicketState FilterTicketState { get; set; }
        public FilterTicketOrder OrderBy { get; set; }

        #region methods

        public FilterTicketDTO SetTickets(List<Ticket> tickets)
        {
            this.Tickets = tickets;
            return this;
        }

        public FilterTicketDTO SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            this.PageCount = paging.PageCount;
            return this;
        }

        #endregion
    }

    public enum FilterTicketState
    {
        All,
        Deleted,
        NotDeleted
    }
    public enum FilterTicketOrder
    {
        CreateDate_ASC,
        CreateDate_DSC
    }
}
