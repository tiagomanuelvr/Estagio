using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using MvcMovie_API.Models;
using System.Globalization;
using System.Text;

namespace MvcMovie_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[ApiKey]
    public class MoviesApiController : ControllerBase
    {
        private readonly MvcMovieContext _context;

        public MoviesApiController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: api/MoviesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovie(string? searchString, string? movieGenre)
        {
            //// Use LINQ to get list of genres.
            //IQueryable<string> genreQuery = from m in _context.Movie
            //                                orderby m.Genre
            //                                select m.Genre;
            //var movies = from m in _context.Movie
            //             select m;

            var movies = _context.Movie.OrderBy(m => m.Id).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s => s.Title!.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
            }

            return await movies.ToListAsync();
        }

        // GET: api/MoviesApi/getMovies
        [HttpPost("getMovies")]
        [Consumes("application/json")]
        public async Task<ActionResult<DtResult<Movie>>> GetMovies([FromBody] DataTableAjaxPostModel model)
        {
            var movies = _context.Movie.AsQueryable();

            var query = movies;
            
            if (!string.IsNullOrEmpty(model.search.value))
            {
                string searchValue = model.search.value.ToLower();

                query = query.Where(m => m.Title!.ToLower().Contains(searchValue));
            }

            if (model.order != null && model.order.Count > 0)
            {
                var orderColumnIndex = model.order[0].column;
                var orderColumnName = model.columns[orderColumnIndex].name;
                var sortDir = model.order[0].dir;

                if (!string.IsNullOrEmpty(orderColumnName))
                {
                    if (sortDir == "asc")
                        query = query.OrderBy(c => EF.Property<string>(c, orderColumnName));
                    else
                        query = query.OrderByDescending(c => EF.Property<string>(c, orderColumnName));
                }
            }
            //var recordsFiltered = query.Count();

            var sendData = query.Skip(model.start).Take(model.length);

            var obj = new DtResult<Movie>
            {
                Data = sendData,
                Draw = model.draw,
                RecordsTotal = movies.Count(),
                RecordsFiltered = query.Count(),
            };

            //return Newtonsoft.Json.JsonConvert.SerializeObject(obj) ;
            return obj;
        }

        // GET: api/MoviesApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movie.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // PUT: api/MoviesApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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

        // POST: api/MoviesApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("loadMovie")]
        //[ValidateAntiForgeryToken]
        [IgnoreAntiforgeryToken]
        public async Task<ActionResult<Movie>> PostMovie([FromBody]Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Movie.Add(movie);
                await _context.SaveChangesAsync();
            }
            
            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/MoviesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie(int id)
        {
            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movie.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/moviesApi/sendCsv
        [HttpGet("sendCsv")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[Produces("text/csv")]
        public async Task<IActionResult> GetCsv(DateTime? dataInicio, DateTime? dataFim)
        {
            var query = _context.Movie.AsQueryable();
            query = query.Where(m => m.ReleaseDate >= dataInicio && m.ReleaseDate <= dataFim);

            var movies = await query.ToListAsync();

            var csv = new StringBuilder();
            var columnNames = typeof(Movie).GetProperties().Select(p => p.Name);
            csv.AppendLine(string.Join(";", columnNames));

            foreach (var movie in movies)
            {
                var values = movie.GetType().GetProperties().Select(p => p.GetValue(movie)?.ToString());
                csv.AppendLine(string.Join(";", values));
            }

            byte[] buffer = Encoding.UTF8.GetBytes(csv.ToString());

            var result = new FileContentResult(buffer, "application/octet-stream");
            result.FileDownloadName = "my-csv-file.csv";
            
            Response.Headers.Append("Access-Control-Expose-Headers", "Content-Disposition");
            return result;
        }

        [HttpGet("getCsv")]
        public async Task<IActionResult> GetCsv2(DateTime? dataInicio, DateTime? dataFim)
        {
            var query = _context.Movie.AsQueryable();
            query = query.Where(m => m.ReleaseDate >= dataInicio && m.ReleaseDate <= dataFim);

            var movies = await query.ToListAsync();

            var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";" };

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csvWriter = new CsvHelper.CsvWriter(streamWriter, config))
                {
                    csvWriter.WriteRecords(movies);
                }

                return File(memoryStream.ToArray(), "text/csv", $"Export-{DateTime.Now.ToString("s")}.csv");
            }
        }

        [HttpGet("datatable")]
        public async Task<IActionResult> GetDataToDatatable([FromQuery] int skip, [FromQuery]  int length, string? search)
        {
            var query = _context.Movie.AsQueryable();
            var movies = query;
            if(search is not null)
            {
                movies = query.Where(m => m.Title!.Contains(search));
            }

            var sendData = await movies.Skip(skip).Take(length).ToListAsync();
            var recordsTotal = await query.CountAsync();
            var recordsFiltered = await movies.CountAsync();

            return Ok(new
            {
                Data = sendData,
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
            });
        }

        private bool MovieExists(int id)
        {
            return _context.Movie.Any(e => e.Id == id);
        }
    }
}