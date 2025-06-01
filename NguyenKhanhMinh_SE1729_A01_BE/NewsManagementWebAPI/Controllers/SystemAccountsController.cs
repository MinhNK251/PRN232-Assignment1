using Microsoft.AspNetCore.Mvc;
using ServiceLayer;
using BusinessObjectsLayer.Entity;

namespace NewsManagementWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAccountsController : ControllerBase
    {
        private readonly ISystemAccountService _context;

        public SystemAccountsController(ISystemAccountService context)
        {
            _context = context;
        }

        // GET: api/SystemAccounts
        [HttpGet]
        public ActionResult<IEnumerable<SystemAccount>> GetSystemAccounts()
        {
            var accounts = _context.GetAccounts();
            return Ok(accounts);
        }

        // GET: api/SystemAccounts/5
        [HttpGet("{id}")]
        public ActionResult<SystemAccount> GetSystemAccount(short id)
        {
            var account = _context.GetAccountById(id);

            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        // PUT: api/SystemAccounts/5
        [HttpPut("{id}")]
        public IActionResult PutSystemAccount(short id, SystemAccount systemAccount)
        {
            if (id != systemAccount.AccountId)
            {
                return BadRequest();
            }

            if (_context.GetAccountById(id) == null)
            {
                return NotFound();
            }

            _context.UpdateAccount(systemAccount);
            return NoContent();
        }

        // POST: api/SystemAccounts
        [HttpPost]
        public ActionResult<SystemAccount> PostSystemAccount(SystemAccount systemAccount)
        {
            if (_context.GetAccountById(systemAccount.AccountId) != null)
            {
                return Conflict("An account with this ID already exists.");
            }

            _context.AddAccount(systemAccount);
            return CreatedAtAction(nameof(GetSystemAccount), new { id = systemAccount.AccountId }, systemAccount);
        }

        // DELETE: api/SystemAccounts/5
        [HttpDelete("{id}")]
        public IActionResult DeleteSystemAccount(short id)
        {
            var account = _context.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.RemoveAccount(id);
            return NoContent();
        }
    }
}
