using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniprofilerTest.MVC.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        private readonly TestDbContext _dbContext;

        public IndexModel(ILogger<IndexModel> logger, TestDbContext dbContext)
        {
            _logger = logger;

            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();
        }

        public async Task OnGet()
        {
            //var miniProfiler = MiniProfiler.Current;
            //var storage = miniProfiler.Storage;
            //storage.Save(miniProfiler);

            await _dbContext.MyClasses.AddAsync(new MyClass(DateTime.Now.ToString("O")));
            await _dbContext.SaveChangesAsync();

            ViewData["datas"] = _dbContext.MyClasses.AsAsyncEnumerable();
        }
    }
}
