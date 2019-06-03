using IRunes.App.Extensions;
using IRunes.Data;
using IRunes.Models;
using SIS.HTTP.Requests;
using SIS.HTTP.Responses;
using SIS.WebServer;
using SIS.WebServer.Atributes.Http;
using System.Collections.Generic;
using System.Linq;

namespace IRunes.App.Controllers
{
    public class TracksController : Controller
    {
        public IHttpResponse Create(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("/Users/Login");
            }
            string trackId = httpRequest.QueryData["albumId"].ToString();

            this.ViewData["AlbumId"] = trackId;
            return this.View();
        }

        [HttpPost(ActionName = "Create")]
        public IHttpResponse CreateConfirm(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("/Users/Login");
            }
            string trackId = httpRequest.QueryData["albumId"].ToString();

            using (var context = new RunesDbContext())
            {
                Album albumFromDb = context.Albums.FirstOrDefault(a => a.Id == trackId);

                if (albumFromDb == null)
                {
                    return this.Redirect("/Albums/All");
                }

                string name = ((ISet<string>)httpRequest.FormData["name"]).FirstOrDefault();
                string link = ((ISet<string>)httpRequest.FormData["link"]).FirstOrDefault();
                string price = ((ISet<string>)httpRequest.FormData["price"]).FirstOrDefault();

                Track track = new Track
                {
                    Name = name,
                    Link = link,
                    Price = decimal.Parse(price)
                };

                albumFromDb.Tracks.Add(track);
                albumFromDb.Price = (albumFromDb.Tracks
                                        .Select(trackA => track.Price)
                                        .Sum() * 87) / 100;
                context.Update(albumFromDb);
                context.SaveChanges();

            }

            return this.Redirect($"/Albums/Details?id={trackId}");
        }

        public IHttpResponse Details(IHttpRequest httpRequest)
        {
            if (!this.IsLoggedIn(httpRequest))
            {
                return this.Redirect("/Users/Login");
            }

            string albumId = httpRequest.QueryData["albumId"].ToString();
            string trackId = httpRequest.QueryData["trackId"].ToString();

            using (var context = new RunesDbContext())
            {
                Track track = context.Tracks.FirstOrDefault(a => a.Id == trackId);

                if (track == null)
                {
                    return this.Redirect($"/Albums/Details?id={albumId}");
                }

                this.ViewData["AlbumId"] = albumId;
                this.ViewData["Track"] = track.ToHtmlDetails(albumId);
                return this.View();
            }
        }
    }
}
