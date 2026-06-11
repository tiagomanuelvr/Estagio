using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Web;

namespace SegurosDirecto.ContentFinders;

public class PageNotFoundContentFinder : IContentLastChanceFinder
{
    private readonly IUmbracoContextFactory _umbracoContextFactory;

    // Replace with your actual 404 page GUID
    private static readonly Guid NotFoundPageKey =
        Guid.Parse("85ae539b-2305-40e8-9fbd-a4eba72d8994");

    public PageNotFoundContentFinder(IUmbracoContextFactory umbracoContextFactory)
    {
        _umbracoContextFactory = umbracoContextFactory;
    }

    public Task<bool> TryFindContent(IPublishedRequestBuilder request)
    {
        using UmbracoContextReference contextRef = _umbracoContextFactory.EnsureUmbracoContext();

        IPublishedContent? notFoundPage = contextRef
            .UmbracoContext
            .Content?
            .GetById(NotFoundPageKey);

        if (notFoundPage == null)
        {
            return Task.FromResult(false);
        }

        request.SetPublishedContent(notFoundPage);
        request.SetResponseStatus(404);

        return Task.FromResult(true);
    }
}

// Register the content finder
public class PageNotFoundContentFinderComposer : IComposer
{
    public void Compose(IUmbracoBuilder builder)
    {
        builder.SetContentLastChanceFinder<PageNotFoundContentFinder>();
    }
}