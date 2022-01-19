using BankApi.Models;
using BankProjectApi.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BankApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
    public class UserManagementController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        ModelBuilder modelBuilder = new ModelBuilder();

        public UserManagementController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("GetItem")]
        public async Task<IActionResult> GetItem([FromQuery] int id)
        {
            if (ModelState.IsValid)
            {
                var items = await _context.UserDatas.FirstOrDefaultAsync(x => x.Id == id);

                if (items != null)
                    return Ok(items);
                else
                    return BadRequest(new
                    {
                        error = "User Not Found"
                    });
            }
            return BadRequest(new
            {
                error = "Something Went Wrong"
            });


        }

        [HttpGet]
        [Route("GetItemsSorted")]
        public async Task<IActionResult> GetItemsSorted()
        {
            var items = await _context.UserDatas.ToListAsync();

            if (items != null)
            {
                var result = items.OrderBy(x => x.FirstName);
                return Ok(result);
            }

            return BadRequest(new
            {
                error = "Something went wrong"
            });
        }

        [HttpGet]
        [Route("ItemsPaging")]
        public async Task<IActionResult> ItemsPaging([FromQuery] Paging paging)
        {

            var item = await _context.UserDatas.Skip((paging.Page - 1) * paging.ItemsPerPage)
                                               .Take(paging.ItemsPerPage)
                                               .ToListAsync();
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpGet]
        [Route("GetItems")]
        public async Task<IActionResult> GetItems()
        {

            var item = await _context.UserDatas.ToListAsync();
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpPost]
        [Route("AddItem")]
        public async Task<IActionResult> AddItem([FromBody] UserData data)
        {
            if (ModelState.IsValid)
            {
                await _context.UserDatas.AddAsync(data);
                await _context.SaveChangesAsync();

                return CreatedAtAction("AddItem", new { data.Id }, data);
            }

            return new JsonResult("Wrong!") { StatusCode = 500 };
        }


        [HttpPut]
        [Route("UpdateItem/{id}")]
        public async Task<IActionResult> UpdateItem([FromRoute] int id, [FromBody] UserData data)
        {
            var existData = await _context.UserDatas.FirstOrDefaultAsync(i => i.Id == id);


            //var AccountUserId = existData.AccountNumbers;
            var accountData = await _context.Accounts.Where(i => i.UserId == id).ToListAsync();
            var accountForDelete = accountData;
            //if (id != data.Id)
            //    return BadRequest(new
            //    {
            //        error = "Id not found"
            //    });

            if (existData == null)
                return NotFound(new
                {
                    error = "Data not found"
                });
            _context.Accounts.RemoveRange(accountForDelete);

            existData.FirstName = data.FirstName;
            existData.LastName = data.LastName;
            existData.PersonalId = data.PersonalId;
            existData.Email = data.Email;
            accountData = data.AccountNumbers;
            existData.AccountNumbers = accountData;
            await _context.SaveChangesAsync();

            return Ok(new
            {
                status = "Object updated successfully"
            });

        }

        [HttpDelete]
        [Route("DeleteItem")]
        public async Task<IActionResult> DeleteItem([FromQuery] int id)
        {
            var existData = await _context.UserDatas.FirstOrDefaultAsync(i => i.Id == id);

            var accountData = await _context.Accounts.Where(i => i.UserId == id).ToListAsync();

            if (existData == null)
                return NotFound(new
                {
                    message = "User not found"
                });
             _context.UserDatas.Remove(existData);
            _context.Accounts.RemoveRange(accountData);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = $"{existData.FirstName} has been deleted",
                User = existData,
                account = accountData
            });

        }
    }
}