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
    [Route("api/[controller]/[action]")]
    public class AggregateController : ControllerBase
    {
        private readonly IndexerContext _context;
        private readonly ILogger<FileController> _logger;

        public AggregateController(IndexerContext context, ILogger<FileController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Types(string extentions = null, int pagesize = 10, int page = 1)
        {
            IEnumerable<string> extensionSplit = String.IsNullOrEmpty(extentions) 
                ? await _context.FileEntry.Select(file => file.Extension).Distinct().ToListAsync()
                : extentions.Split("|")?.Select(ext => $".{ext}")?.ToList();

            List<Result> results = new List<Result>();

            var all = await _context
                    .FileEntry
                    .Select(file => file.Extension.ToLower())
                    .Where(ext => extensionSplit.Contains(ext))
                    .ToListAsync();

            var grouped = all.GroupBy(ext => ext);

            foreach (var extentionGroup in grouped)
            {
                results.Add(new Result { Extension = extentionGroup.Key, Count = extentionGroup.ToList().Count()});
            }

            return Ok(new 
            {
                Page = page,
                Size = pagesize,
                Results = results
                .OrderByDescending(file => file.Count)
                .Skip((page <= 0 ? 0 : page - 1) * pagesize)
                .Take(pagesize),
                TotalResults = results.Count()
            });
        }

        private class Result
        {
            public string Extension { get; set; }
            public int Count { get; set; }
        }
    }
}
