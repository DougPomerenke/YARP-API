﻿using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("/api")]
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

        [HttpGet("/api/{id}")]
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

        [HttpPost("api")]
        public async Task<IActionResult> CreateAccountHolder([FromBody] AccountHolder accountHolder)
        {
            try
            {
                if(accountHolder.Id.Length>0 == false)
                {
                    accountHolder.Id = Guid.NewGuid().ToString();
                }   

                var addedAccountHolder = await _dbContext.AddAsync(accountHolder);
                await _dbContext.SaveChangesAsync();

                return Ok(addedAccountHolder.Entity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("api/{id}")]
        public async Task<IActionResult> PutAccountHolder(string id, AccountHolder accountHolder)
        {
            if (id != accountHolder.Id)
            {
                return BadRequest();
            }

            //_dbContext.Entry(accountHolder).State = EntityState.Modified;

            try
            {
                var accountHolderEntity = await _dbContext.AccountHolders.Where(e=>e.Id==id).SingleOrDefaultAsync();
                if (accountHolderEntity != null)
                {
                    accountHolderEntity.AccountStaringBalance = accountHolder.AccountStaringBalance;
                    await _dbContext.SaveChangesAsync();
                }


                //_dbContext.Update(accountHolder);
                //await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccountHolderExists(id))
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

        private bool AccountHolderExists(string id)
        {
            return _dbContext.AccountHolders.Any(e => e.Id == id);
        }
    }
}