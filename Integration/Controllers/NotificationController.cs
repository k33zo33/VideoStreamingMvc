using AutoMapper;
using Integration.BLModels;
using Integration.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sql;
using MoviesRWA.Integration.BLModels;
using System.Net.Mail;

namespace MoviesRWA.Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly RwaMoviesContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly SmptConfig _smtpConfig;

        public NotificationController(RwaMoviesContext dbContext, IMapper mapper, IConfiguration config, SmptConfig smptConfig)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _config = config;
            _smtpConfig = smptConfig;
        }

        [HttpGet("{id}")]
        public ActionResult<BLNotificationResp> Get(int id)
        {
            try
            {
                var dbNotification = _dbContext.Notifications.FirstOrDefault(x => x.Id == id);
                if (dbNotification == null)
                    return NotFound();
                var blNotificationResponse = _mapper.Map<BLNotificationResp>(dbNotification);

                return Ok(blNotificationResponse);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpGet("[action]")]
        public ActionResult<IEnumerable<BLNotificationResp>> GetAll()
        {
            try
            {
                var allNotifications = _dbContext.Notifications;
                var blNoficationResponse = _mapper.Map<IEnumerable<BLNotificationResp>>(allNotifications);
                return Ok(blNoficationResponse);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost()]
        public ActionResult<BLNotificationResp> Create(BLNotificationReq request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dbNotification = _mapper.Map<Notification>(request);
                dbNotification.CreatedAt = DateTime.Now;

                _dbContext.Notifications.Add(dbNotification);

                _dbContext.SaveChanges();

                var blNofitication = _mapper.Map<BLNotificationResp>(dbNotification);

                return Ok(blNofitication);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<BLNotificationResp> Edit(int id, [FromBody] BLNotificationReq request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var dbNotification = _dbContext.Notifications.FirstOrDefault(x => x.Id == id);
                if (dbNotification == null)
                    return NotFound();

                dbNotification.ReceiverEmail = request.ReceiverEmail;
                dbNotification.Subject = request.Subject;
                dbNotification.Body = request.Body;


                _dbContext.SaveChanges();
                var dbNotificationResponse = _mapper.Map<BLNotificationResp>(dbNotification);
                return Ok(dbNotificationResponse);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpDelete("{id}")]
        public ActionResult<BLNotificationResp> Delete(int id)
        {
            try
            {
                var dbNotification = _dbContext.Notifications.FirstOrDefault(x => x.Id == id);
                if (dbNotification == null)
                    return NotFound();

                _dbContext.Notifications.Remove(dbNotification);

                _dbContext.SaveChanges();
                var blNotificationResponse = _mapper.Map<BLNotificationResp>(dbNotification);

                return Ok(blNotificationResponse);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        [HttpPost("[action]")]
        public ActionResult Send()
        {
            var client = new SmtpClient(_smtpConfig.Server, _smtpConfig.Port);
            var sender = _smtpConfig.SenderEmail;
            try
            {
                var unsentNotifications = _dbContext.Notifications.Where(x => x.SentAt == null).ToList();

                foreach (var notification in unsentNotifications)
                {
                    try
                    {
                        var mail = new MailMessage(
                            from: new MailAddress(sender),
                            to: new MailAddress(notification.ReceiverEmail));

                        mail.Subject = notification.Subject;
                        mail.Body = notification.Body;

                        client.Send(mail);

                        notification.SentAt = DateTime.UtcNow;

                        _dbContext.SaveChanges();

                    }
                    catch (Exception)
                    {
                        // <------------ TODO
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    "There has been a problem while sending notifications");
            }
        }
    }
}
