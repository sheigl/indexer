using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using indexer.common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace indexer.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IndexerContext _context;
        private readonly ILogger<FileController> _logger;

        public FileController(IndexerContext context, ILogger<FileController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int pagesize = 10, int page = 1)
        {
            _logger.LogInformation($"Getting page {page}");


            var query = _context
                .FileEntry
                .OrderBy(file => file.FullName)
                .Skip((page <= 0 ? 0 : page - 1) * pagesize)
                .Take(pagesize);

            return Ok(new 
            {
                Page = page,
                Size = pagesize,
                Results = await query.ToListAsync(),
                TotalResults = await _context.FileEntry.CountAsync()
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] FileEntry file)
        {
            var dbFile = await _context.FileEntry.FindAsync(file.FullName);

            if (dbFile == null)
            {
                dbFile = file;
                //_logger.LogInformation($"Adding \n{JsonSerializer.Serialize(file)}");
                await _context.FileEntry.AddAsync(dbFile);
            }
            else 
            {
                //_logger.LogInformation($"Updating \n{JsonSerializer.Serialize(file)}");
                foreach (var item in typeof(FileEntry).GetProperties())
                {
                    object value = item.GetValue(file);
                    item.SetValue(dbFile, value);
                }

                _context.FileEntry.Update(dbFile);
            }

            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpPost, Route("all")]
        public async Task<IActionResult> Post([FromBody] List<FileEntry> files)
        {
            await _context.FileEntry.AddRangeAsync(files);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
