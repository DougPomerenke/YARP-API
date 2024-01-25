using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System;

namespace BalanceCalculatorAccountHolderApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountHoldersController : ControllerBase
    {
        private readonly AccountHoldersContext _dbContext;
        private static bool _ensureCreated { get; set; } = false;


        public AccountHoldersController(AccountHoldersContext dbContext)
        {
            _dbContext = dbContext;

            if (!_ensureCreated)
            {
                _dbContext.Database.EnsureCreated();
                _ensureCreated = true;
            }
        }

        [HttpGet("/api/AccountHolders")]
        public async Task<IActionResult> GetAllAccountHolders()
        {
            try
            {
                 return Ok(await _dbContext.AccountHolders.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("/api/AccountHolders/{id}")]
        public async Task<IActionResult> GetAccountHolder(string id)
        {
            try
            {
                return Ok(await _dbContext.AccountHolders.Where(c=>c.Id==id).ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("api/AccountHolders/")]
        public async Task<IActionResult> CreateAccountHolder(AccountHolder accountHolder)
        {
            try
            {
                accountHolder.Id = Guid.NewGuid().ToString();
  
                var addedAccountHolder = await _dbContext.AddAsync(accountHolder);
                await _dbContext.SaveChangesAsync();

                return Ok(addedAccountHolder.Entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut("api/AccountHolders/{id}")]
        public async Task<IActionResult> UpdateAccountHolder(string id, AccountHolder updateObject)
        {
            if (id != updateObject.Id)
            {
                return BadRequest();
            }

            //_dbContext.Entry(updateObject).State = EntityState.Modified;

            try
            {
                var dbEntity = await _dbContext.AccountHolders.Where(e => e.Id == id).SingleOrDefaultAsync();
                if (dbEntity != null)
                {
                    dbEntity.AccountStartingBalance = updateObject.AccountStartingBalance;
                    dbEntity.DateOfBirth = updateObject.DateOfBirth;
                    dbEntity.FirstName = updateObject.FirstName;
                    dbEntity.LastName = updateObject.LastName;
                    dbEntity.Scenarios = updateObject.Scenarios;
                    dbEntity.FinancialEvents = updateObject.FinancialEvents;
                
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountHolderExists(updateObject.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpDelete("api/AccountHolders/{id}")]
        public async Task<IActionResult> DeleteAccountHolder(string id)
        {

            try
            {
                var dbEntity = await _dbContext.AccountHolders.Where(e => e.Id == id).SingleOrDefaultAsync();
                if (dbEntity != null)
                {
                    _dbContext.Remove(dbEntity);

                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                 return NotFound();

            }

            return NoContent();
        }



        private bool AccountHolderExists(string id)
        {
            return _dbContext.AccountHolders.Any(e => e.Id == id);
        }
    }
}
