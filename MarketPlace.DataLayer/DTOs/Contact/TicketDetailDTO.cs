using MarketPlace.DataLayer.Entities.Contact;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.DTOs.Contact
{
    public class TicketDetailDTO
    {
        public Ticket ticket { get; set; }
        public List<TicketMessage> ticketMessages { get; set; }
    }
}
