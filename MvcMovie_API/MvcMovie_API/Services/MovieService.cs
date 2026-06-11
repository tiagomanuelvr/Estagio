using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;
using MvcMovie_API.Models;
using MvcMovie_API.Models.DTO;
using MvcMovie_API.Repositories;
using System.Text;

namespace MvcMovie_API.Services;

public class MovieService : IMovieService
{
    private readonly IRepository<Movie> _repository;

    public MovieService(IRepository<Movie> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Movie>> GetAllMovies()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Movie?> GetMovieById(int id)
    {
        var movie = await _repository.GetByIdAsync(id);
        return movie;
    }

    public async Task<Movie> CreateMovie(MovieDTO movieDTO)
    {
        var movie = MovieDtoToMovie(0, movieDTO);
        await _repository.AddAsync(movie);
        var result = await _repository.SaveChangesAsync();
        return movie;
    }

    public async Task UpdateMovie(int id,  Movie movie)
    {

        _repository.Update(movie);
        await _repository.SaveChangesAsync();
    }

    public async Task DeleteMovie(Movie movie)
    {
        _repository.Delete(movie);
        var result = await _repository.SaveChangesAsync();
    }

    public async Task<FileContentResult> SendCsv(DateTime? dataInicio, DateTime? dataFim)
    {
        IQueryable<Movie> query = _repository.DoQueries();
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
        result.FileDownloadName = "movies-csv-file.csv";

        return result;
    }

    public async Task<DtResult<Movie>> GetDatatable(DataTableAjaxPostModel model)
    {
        var movies = _repository.DoQueries();
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
                query = sortDir == "asc" ? query.OrderBy(c => EF.Property<string>(c, orderColumnName)) : query.OrderByDescending(c => EF.Property<string>(c, orderColumnName));
            }
        }
        var sendData = query.Skip(model.start).Take(model.length);

        var obj = CreateDtResult(sendData, model.draw, movies.Count(), query.Count());
        return obj;
    }

    private DtResult<Movie> CreateDtResult(IEnumerable<Movie> sendData, int draw, int countMovies, int countQueries)
    {
        var obj = new DtResult<Movie>
        {
            Data = sendData,
            Draw = draw,
            RecordsTotal = countMovies,
            RecordsFiltered = countQueries,
        };
        return obj;
    }
    private Movie MovieDtoToMovie(int id, MovieDTO movieDTO)
    {
         Movie movie = new Movie
        {
            Id = id,
            Title = movieDTO.Title,
            ReleaseDate = movieDTO.ReleaseDate,
            Genre = movieDTO.Genre,
            Price = movieDTO.Price,
            Rating = movieDTO.Rating
        };

        return movie;
    }
}