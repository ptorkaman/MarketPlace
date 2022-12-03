using MarketPlace.Application.Services.Interfaces;
using MarketPlace.DataLayer.DTOs.Contact;
using MarketPlace.Web.PresentationExtentions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketPlace.Web.Areas.User.Controllers
{
    public class TicketController : UserBaseController
    {

        #region constructor
        private readonly IContactService _contactService;
        public TicketController(IContactService contactService)
        {
            _contactService = contactService;
        }
        #endregion

        #region List
        [HttpGet("tickets")]
        public async Task<IActionResult>  ListOfTicket(FilterTicketDTO filter)
        {
            filter.UserId = User.GetUserId();
            filter.FilterTicketState = FilterTicketState.NotDeleted;
            filter.OrderBy = FilterTicketOrder.CreateDate_DSC;

            return View(await _contactService.FilterTicket(filter));
        }
        #endregion

        #region add ticket
        [HttpGet("add-ticket")]
        public async Task<IActionResult> AddTicket()
        {
            return View();
        }

        [HttpPost("add-ticket")]
        public async Task<IActionResult> AddTicket(AddTicketViewModelDTO newticket)
        {
            if (ModelState.IsValid)
            {
                var res = await _contactService.AddTicket(newticket, User.GetUserId());
                switch (res)
                {
                    case AddTicketResult.Error:
                        TempData[ErrorMessage] = "عملیات با شکست مواجه شد.";
                        break;
                    case AddTicketResult.Success:
                        TempData[SuccessMessage] = "تیکت شما با موفقیت ارسال شد.";
                        TempData[InfoMessage] = "پاسخ شما به زودی ارسال خواهد شد.";
                        return RedirectToAction("Index");

                }
            }
             return View(newticket);
        }
        #endregion

        #region show ticket detail
        [HttpGet("tickets/{ticketId}")]
        public async Task<IActionResult> ShowTicketDetail(long ticketId)
        {
            var ticket = await _contactService.GetTicketForShow(ticketId, User.GetUserId());
            if (ticket == null) return NotFound();

            return View(ticket);
        }
        #endregion

        #region answer ticket

        [HttpPost("answer-ticket"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AnswerTicket(AnswerTicketDTO answer)
        {
            if (string.IsNullOrEmpty(answer.Text))
            {
                TempData[ErrorMessage] = "لطفا متن پیام خود را وارد نمایید";
            }

            if (ModelState.IsValid)
            {
                var res = await _contactService.AnswerTicket(answer, User.GetUserId());
                switch (res)
                {
                    case AnswerTicketResult.NotForUser:
                        TempData[ErrorMessage] = "عدم دسترسی";
                        TempData[InfoMessage] = "در صورت تکرار این مورد ، دسترسی شما به صورت کلی از سیستم قطع خواهد شد";
                        return RedirectToAction("Index");
                    case AnswerTicketResult.NotFound:
                        TempData[WarningMessage] = "اطلاعات مورد نظر یافت نشد";
                        return RedirectToAction("Index");
                    case AnswerTicketResult.Success:
                        TempData[SuccessMessage] = "اطلاعات مورد نظر با موفقیت ثبت شد";
                        break;
                }
            }

            return RedirectToAction("ShowTicketDetail", "Ticket", new { area = "User", ticketId = answer.Id });
        }

        #endregion
    }
}
