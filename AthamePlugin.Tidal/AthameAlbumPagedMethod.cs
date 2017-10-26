using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Athame.PluginAPI.Service;
using AthamePlugin.Tidal.InternalApi.Models;

namespace AthamePlugin.Tidal
{
    public class AthameAlbumPagedMethod : PagedMethod<Album>
    {
        private PagedMethod<TidalAlbum> albumsPagedMethod;
        private TidalServiceSettings settings;

        public AthameAlbumPagedMethod(TidalServiceSettings settings, PagedMethod<TidalAlbum> albumsPagedMethod) : base(albumsPagedMethod.ItemsPerPage)
        {
            this.albumsPagedMethod = albumsPagedMethod;
            this.settings = settings;
        }

        public override async Task<IList<Album>> GetNextPageAsync()
        {
            var nextItems = await albumsPagedMethod.GetNextPageAsync();
            return nextItems.Select(tidalAlbum => tidalAlbum.CreateAthameAlbum(settings)).ToList();
        }

        public override int ItemsPerPage => albumsPagedMethod.ItemsPerPage;

        public override IList<Album> AllItems => albumsPagedMethod.AllItems.Select(tidalAlbum => tidalAlbum.CreateAthameAlbum(settings)).ToList();

        public override bool HasMoreItems => albumsPagedMethod.HasMoreItems;

        public override int TotalNumberOfItems => albumsPagedMethod.TotalNumberOfItems;

        public override async Task LoadAllPagesAsync()
        {
            while (HasMoreItems)
            {
                await GetNextPageAsync();
            }
        }
    }
}