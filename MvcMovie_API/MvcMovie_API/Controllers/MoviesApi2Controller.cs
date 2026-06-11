using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using MvcMovie_API.Models;
using MvcMovie_API.Models.DTO;
using MvcMovie_API.Services;

namespace MvcMovie_API.Controllers;

[Route("api2/[controller]")]
[ApiController]
public class MoviesApi2Controller : ControllerBase
{
    private readonly IMovieService _movieService;

    public MoviesApi2Controller(IMovieService movieService)
    {
        _movieService = movieService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Movie>>> GetAll()
    {
        var movies = await _movieService.GetAllMovies();
        return Ok(movies);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Movie>> GetMovie(int id)
    {
        var movie = await _movieService.GetMovieById(id);

        if (movie is null)
        {
            return NotFound();
        }

        return movie;
    }

    [HttpPost]
    public async Task<ActionResult<Movie>> PostMovie([FromBody] MovieDTO movieDTO)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var movie = await _movieService.CreateMovie(movieDTO);
        return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovie(int id, MovieDTO movieDTO)
    {
        var movie = await _movieService.GetMovieById(id);

        if (movie is null)
        {
            return NotFound();
        }

        movie = MovieDtoToMovie(movieDTO, movie);

        await _movieService.UpdateMovie(id, movie);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteMovie(int id)
    {
        var movie = await _movieService.GetMovieById(id);

        if (movie is null)
        {
            return NotFound();
        }

        await _movieService.DeleteMovie(movie);

        return NoContent();
    }

    [HttpGet("GetCsv")]
    public async Task<IActionResult> GetCsv(DateTime? dataInicio, DateTime? dataFim)
    {
        var file = await _movieService.SendCsv(dataInicio, dataFim);
        Response.Headers.Append("Access-Control-Expose-Headers", "Content-Disposition");

        return file;
    }

    [HttpPost("GenerateDatatable")]
    public async Task<ActionResult<DtResult<Movie>>> GetMovies([FromBody] DataTableAjaxPostModel model)
    {
        return await _movieService.GetDatatable(model);
    }

    private Movie MovieDtoToMovie(MovieDTO movieDTO, Movie movie)
    {
        movie.Title = movieDTO.Title;
        movie.ReleaseDate = movieDTO.ReleaseDate;
        movie.Genre = movieDTO.Genre;
        movie.Price = movieDTO.Price;
        movie.Rating = movieDTO.Rating;

        return movie;
    }
}