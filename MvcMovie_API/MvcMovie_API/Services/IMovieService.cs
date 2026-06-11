using Microsoft.AspNetCore.Mvc;
using MvcMovie.Models;
using MvcMovie_API.Models;
using MvcMovie_API.Models.DTO;

namespace MvcMovie_API.Services;

public interface IMovieService
{
    Task<IEnumerable<Movie>> GetAllMovies();
    Task<Movie?> GetMovieById(int id);
    Task<Movie> CreateMovie(MovieDTO movieDTO);
    Task UpdateMovie(int id, Movie movie);
    Task DeleteMovie(Movie movie);
    Task<FileContentResult> SendCsv(DateTime? dataInicio, DateTime? dataFim);
    Task<DtResult<Movie>> GetDatatable(DataTableAjaxPostModel model);
}
