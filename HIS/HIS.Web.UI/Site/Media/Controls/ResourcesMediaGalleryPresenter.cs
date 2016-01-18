using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HIS.Web.UI
{
    /// <summary>
    /// Input : { Context , Guid : MediaCategoryID }
    /// </summary>
    public class ResourcesMediaGalleryPresenter : GenericPresenter<IResourcesMediaGalleryView>
    {

        private IRepositoryFactory factory;

        public ResourcesMediaGalleryPresenter(IRepositoryFactory factory)
        {

            this.factory = factory;
        }

        public override void View_Load(object sender, EventArgs e)
        {
            DataQuery();
        }

        private void DataQuery()
        {

            var e = View.PostParameter;

            var repository = factory.GetRepository<Repository<MediaCategory>>();
            var query = repository.GetQueryable();

            Guid mediaCategoryId = Guid.Empty;
            string guid = View.PostParameter.Data.UID;
            string context = View.PostParameter.Data.Context;

            if (Guid.TryParse(guid, out mediaCategoryId))
            {
                var mediaCategory = query.Where(o => o.Id == mediaCategoryId)
                                         .FirstOrDefault();


                switch (context)
                {
                    case "Image":
                        var resources = mediaCategory.Resources
                                                    .Cast<ResourceImage>()
                                                    .Select(o => new
                                                    {
                                                        Id = o.Id,
                                                        Name = o.Name,
                                                        Description = o.Description,
                                                        Thumbnail = o.Thumbnail,
                                                        Size = o.Size,
                                                        Dimension = o.Dimension,

                                                    });


                        View.JsonData = Json(new
                        {
                            Data = new
                            {
                                Context = context,
                                Resources = resources,
                            }
                        });

                        break;
                    case "Video":
                        var resourcesVideo = mediaCategory.Resources
                                                  .Cast<ResourceVideo>()
                                                  .Select(o => new
                                                  {
                                                      Id = o.Id,
                                                      Name = o.Name,
                                                      Description = o.Description,
                                                      Size = o.Size,
                                                      Dimension = o.Dimension,
                                                      FileName = o.Filename,//.Split('.')[0].ToString().Trim(),
                                                      //FilePath = ConfigurationManager.AppSettings["DOWNLOAD_PATH"] ,
                                                      //FilePath2 = AppDomain.CurrentDomain.BaseDirectory,
                                                  });


                        View.JsonData = Json(new
                        {
                            Data = new
                            {
                                Context = context,
                                Resources = resourcesVideo,

                            }
                        });

                        break;
                    case "Music":
                        var resourcesMusic = mediaCategory.Resources
                                                  .Cast<ResourceMusic>()
                                                  .Select(o => new
                                                  {
                                                      Id = o.Id,
                                                      Name = o.Name,
                                                      Description = o.Description,
                                                      Size = o.Size,
                                                      FileName = o.Filename,//.Split('.')[0].ToString().Trim(),
                                                      Artist = o.Artist,
                                                      Album = o.Album,
                                                      Genre = o.Genre,
                                                      Length = o.Length,
                                                  });


                        View.JsonData = Json(new
                        {
                            Data = new
                            {
                                Context = context,
                                Resources = resourcesMusic,

                            }
                        });
                        break;
                }



            }

        }
    }
}