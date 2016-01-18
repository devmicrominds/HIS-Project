using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.UI;
using HIS.Web.UI.Utilities;
using Newtonsoft.Json;
using HIS.Data;
using Shell32;
using System.IO;
using Newtonsoft.Json.Linq;
using HIS.Shared;

namespace HIS.Web.UI
{
    public class MediaController : ApiController
    {
        const string RootUpload = "ServerRootUploadPath";

        const string Image = "Image";
        const string Video = "Video";
        const string Music = "Music";
        const string HTML5 = "HTML5";
        const string RSS = "RSS";


        private string ServerUploadFolder;
        private string ServerTempFolder;

        private Icons icons;

        private IRepositoryFactory factory;

        public MediaController(IRepositoryFactory factory, ApplicationPath appPath, Icons icons)
        {
            this.factory = factory;
            this.icons = icons;
            ServerUploadFolder = appPath.Resources;
            ServerTempFolder = appPath.Temp;

        }

        #region Upload Files
        [HttpPost]
        [ValidateMimeMultipartContentFilter]
        public async Task<HttpResponseMessage> UploadFile()
        {
            var streamProvider = new CustomMultipartFormDataStreamProvider(ServerTempFolder);

            try
            {
                // Read the form data.
                await Request.Content.ReadAsMultipartAsync(streamProvider);

                var formData = NVCHelper.ToDictionary(streamProvider.FormData);

                dynamic uploadModel = JsonConvert.DeserializeObject<dynamic>(formData["UploadModel"]);

                string context = uploadModel.Data.Context;

                string mediaCategoryUID = uploadModel.Data.UID;


                Guid guid = Guid.Empty;

                if (Guid.TryParse(mediaCategoryUID, out guid))
                {
                    // switch context  
                    switch (context)
                    {
                        case Image:
                            {
                                SaveImages(guid, streamProvider.FileData);
                            }
                            break;
                        case Video:
                            {
                                SaveVideo(guid, streamProvider.FileData);
                            }
                            break;
                        case Music:
                            {
                                SaveMusic(guid, streamProvider.FileData);
                            }
                            break;
                    }


                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (System.Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }


        }
        /// <summary>
        /// Get Media category
        /// Do in transaction        
        /// Move file from temp to imagefolder
        /// Read moved file properties
        /// Create ResourceImage objects
        /// SaveOrUpdate in db
        /// Commit
        /// 
        /// </summary>
        /// <param name="mediaCategoryId"></param>
        /// <param name="fileData"></param>
        /// <returns></returns>
        private bool SaveImages(Guid mediaCategoryId, IEnumerable<MultipartFileData> fileData)
        {

            foreach (MultipartFileData file in fileData)
            {
                var repository = factory.GetRepository<Repository<MediaCategory>>();
                var query = repository.GetQueryable();
                var mediaCategory = query.Where(o => o.Id == mediaCategoryId).FirstOrDefault();

                factory.OnTransaction(() =>
                {
                    var actualFileName = file.Headers.ContentDisposition.FileName;
                    var serverFileName = Path.GetFileName(file.LocalFileName);

                    Shell shell = new Shell();
                    Folder objFolder = shell.NameSpace(ServerTempFolder);
                    FolderItem folderItem = objFolder.ParseName(serverFileName);

                    var resource = new ResourceImage()
                    {

                        Name = Path.GetFileNameWithoutExtension(Strings.UnquoteToken(actualFileName)),
                        Filename = serverFileName,
                        Location = Path.Combine(ServerUploadFolder, Constants.Image),
                        Height = objFolder.GetDetailsOf(folderItem, (int)ImageProperties.Height),
                        Width = objFolder.GetDetailsOf(folderItem, (int)ImageProperties.Width),
                        Description = String.Empty,
                        ResourceType = ResourceType.Image,
                        Extension = Path.GetExtension(serverFileName),
                        Size = objFolder.GetDetailsOf(folderItem, (int)ImageProperties.Size),
                        Dimension = objFolder.GetDetailsOf(folderItem, (int)ImageProperties.Dimensions),
                        Thumbnail = GetThumbnail(file.LocalFileName)
                    };

                    var destinationPath = Path.Combine(resource.Location, resource.Filename);

                    File.Move(file.LocalFileName, destinationPath);

                    mediaCategory.AddResources(resource);

                });


            }


            return true;
        }

        private bool SaveVideo(Guid mediaCategoryId, IEnumerable<MultipartFileData> fileData)
        {
            foreach (MultipartFileData file in fileData)
            {
                var repository = factory.GetRepository<Repository<MediaCategory>>();
                var query = repository.GetQueryable();
                var mediaCategory = query.Where(o => o.Id == mediaCategoryId).FirstOrDefault();

                factory.OnTransaction(() =>
                {
                    var actualFileName = file.Headers.ContentDisposition.FileName;
                    var serverFileName = Path.GetFileName(file.LocalFileName);

                    Shell shell = new Shell();
                    Folder objFolder = shell.NameSpace(ServerTempFolder);
                    FolderItem folderItem = objFolder.ParseName(serverFileName);

                    var resource = new ResourceVideo()
                    {

                        Name = Path.GetFileNameWithoutExtension(Strings.UnquoteToken(actualFileName)),
                        Filename = serverFileName,
                        Location = Path.Combine(ServerUploadFolder, Constants.Video),
                        Height = objFolder.GetDetailsOf(folderItem, (int)VideoProperties.Height),
                        Width = objFolder.GetDetailsOf(folderItem, (int)VideoProperties.Width),
                        Description = String.Empty,
                        ResourceType = ResourceType.Video,
                        Extension = Path.GetExtension(serverFileName),
                        Size = objFolder.GetDetailsOf(folderItem, (int)VideoProperties.Size),
                        Dimension = objFolder.GetDetailsOf(folderItem, (int)VideoProperties.Dimensions),

                        Title = objFolder.GetDetailsOf(folderItem, (int)VideoProperties.Title),
                        Length = objFolder.GetDetailsOf(folderItem, (int)VideoProperties.Length)
                    };

                    var destinationPath = Path.Combine(resource.Location, resource.Filename);

                    File.Move(file.LocalFileName, destinationPath);

                    mediaCategory.AddResources(resource);

                });


            }


            return true;
        }

        private bool SaveMusic(Guid mediaCategoryId, IEnumerable<MultipartFileData> fileData)
        {

            foreach (MultipartFileData file in fileData)
            {
                var repository = factory.GetRepository<Repository<MediaCategory>>();
                var query = repository.GetQueryable();
                var mediaCategory = query.Where(o => o.Id == mediaCategoryId).FirstOrDefault();

                factory.OnTransaction(() =>
                {
                    var actualFileName = file.Headers.ContentDisposition.FileName;
                    var serverFileName = Path.GetFileName(file.LocalFileName);

                    Shell shell = new Shell();
                    Folder objFolder = shell.NameSpace(ServerTempFolder);
                    FolderItem folderItem = objFolder.ParseName(serverFileName);

                    var resource = new ResourceMusic()
                    {

                        Name = Path.GetFileNameWithoutExtension(Strings.UnquoteToken(actualFileName)),
                        Filename = serverFileName,
                        Location = Path.Combine(ServerUploadFolder, Constants.Music),
                        Description = String.Empty,
                        ResourceType = ResourceType.Music,
                        Extension = Path.GetExtension(serverFileName),
                        Size = objFolder.GetDetailsOf(folderItem, (int)MusicProperties.Size),
                        Artist = objFolder.GetDetailsOf(folderItem, (int)MusicProperties.ContributingArtist),
                        Album = objFolder.GetDetailsOf(folderItem, (int)MusicProperties.Album),
                        Genre = objFolder.GetDetailsOf(folderItem, (int)MusicProperties.Genre),
                        Length = objFolder.GetDetailsOf(folderItem, (int)MusicProperties.Length)
                    };

                    var destinationPath = Path.Combine(resource.Location, resource.Filename);

                    File.Move(file.LocalFileName, destinationPath);

                    mediaCategory.AddResources(resource);

                });


            }


            return true;
        }

        private byte[] GetThumbnail(string fileName)
        {

            using (var fStream = new FileInfo(fileName).OpenRead())
            {

                byte[] fileBytes = ReadFully(fStream);
                return ImageHelper.CreateThumbnail(fileBytes, true, 200, 200);
            }
        }

        private static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private static Stream GetStream(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms;
            }

        }


        #endregion

        #region Get Image

        [HttpGet]
        public HttpResponseMessage GetImage([FromUri] Guid imageId)
        {

            var repository = factory.GetRepository<Repository<ResourceImage>>();

            var query = repository.GetQueryable();

            var resource = query.Where(o => o.Id == imageId).FirstOrDefault();

            var file = Path.Combine(resource.Location, resource.Filename);

            using (var stream = File.OpenRead(file))
            {

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(ReadFully(stream));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");
                return response;

            }


        }

        #endregion

        #region Get Video

        [HttpGet]
        public HttpResponseMessage GetVideo([FromUri] Guid videoId)
        {

            var repository = factory.GetRepository<Repository<ResourceVideo>>();

            var query = repository.GetQueryable();

            var resource = query.Where(o => o.Id == videoId).FirstOrDefault();

            var file = Path.Combine(resource.Location, resource.Filename);

            using (var stream = File.OpenRead(file))
            {

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(ReadFully(stream));
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("video/jpeg");
                return response;

            }


        }

        #endregion

        /// <summary>
        /// data 
        /// {
        ///     timelineID: Guid
        ///     channelID: Guid
        ///     resourceID:  Guid
        ///     blocklength: s
        /// }
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage ChannelBlocksAdd(dynamic data)
        {

            var repository = factory.GetRepository<Repository<Channel>>();
            var query = repository.GetQueryable();

            Guid id_channel = data.Data.Channel;
            string context = data.Data.Context;

            try
            {
                var channel = query.Where(o => o.Id == id_channel).FirstOrDefault();

                JObject collections = data.Data.Collections;

                if (collections.Count > 0)
                {


                    foreach (dynamic c in collections)
                    {
                        Guid resourceId = c.Value.ResourceId;
                        int resourceLength = c.Value.Length;

                        factory.OnTransaction(() =>
                        {

                            var block = new Block()
                            {
                                Length = resourceLength,
                                Resource = GetResource(resourceId),
                            };
                            var result = channel.AddBlocks(block);

                        });
                    }


                }

                var response = new HttpResponseMessage(HttpStatusCode.OK);

                response.Content = new JsonContent(new
                {
                    Campaign = channel.Timeline.Campaign.Id,
                    Timeline = channel.Timeline.Id,
                    Channel = channel.Id,
                });


                return response;

            }
            catch (Exception ex)
            {

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }



        [HttpPost]
        public HttpResponseMessage ChannelBlocksEdit(dynamic data)
        {

            Guid blockId = data.blockID;

            TimeSpan timespan = new TimeSpan((int)data.timespan.hours,
                                             (int)data.timespan.minutes,
                                             (int)data.timespan.seconds);



            try
            {
                var repository = factory.GetRepository<Repository<Block>>();

                var block = repository.GetById(blockId, false);


                var totalseconds = timespan.TotalSeconds;

                factory.OnTransaction(() =>
                {
                    block.Length = Convert.ToInt32(totalseconds);

                });

                var response = new HttpResponseMessage(HttpStatusCode.OK);

                response.Content = new JsonContent(new
                {
                    block_length = totalseconds
                });

                return response;
            }
            catch (Exception ex)
            {

                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }


        }



        #region Utility

        public HttpResponseMessage ResourcesBlockGet(dynamic data)
        {

            try
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);

                Guid mediaCategoryId = data.mediaCategoryId;
                string context = data.Context;

                var repository = factory.GetRepository<Repository<MediaCategory>>();

                var mediaCategory = repository.GetQueryable()
                                      .Where(o => o.Id == mediaCategoryId)
                                      .FirstOrDefault();

                dynamic result = null;

                switch (context)
                {
                    case "Image":
                        // out keyvalue pairs
                        // id : Thumbnail
                        result = mediaCategory.Resources
                                              .Cast<ResourceImage>()
                                              .Select(o => new
                                              {
                                                  Id = o.Id,
                                                  Resource = new
                                                  {
                                                      Name = o.Name,
                                                      Thumbnail = Convert.ToBase64String(o.Thumbnail),
                                                  }
                                              }).ToDictionary(prop => prop.Id,
                                                             prop => prop.Resource);
                        break;
                    case "Video":
                        break;
                    case "HTML5":
                        result = mediaCategory.Resources
                                              .Cast<ResourceHTML>()
                                              .Select(o => new
                                              {
                                                  Id = o.Id,
                                                  Resource = new
                                                  {
                                                      Name = o.Name,
                                                      Thumbnail = Convert.ToBase64String(icons.HTML5.Icon),
                                                  }

                                              }).ToDictionary(prop => prop.Id,
                                                              prop => prop.Resource);
                        break;

                }

                response.Content = new JsonContent(new
                {
                    resources = result,
                });



                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public HttpResponseMessage ScreenResolutionsGet(dynamic data)
        {
            try
            {
                Orientation orientation = (Orientation)data.Orientation;

                var response = new HttpResponseMessage(HttpStatusCode.OK);

                var repository = factory.GetRepository<Repository<ScreenOrientation>>();
                var query = repository.GetQueryable();
                var screenOrientation = query.Where(o => o.Orientation == orientation)
                                             .FirstOrDefault();

                var result = screenOrientation
                                .ScreenResolutions
                                .Select(o => new { o.Id, o.Resolution })
                                .ToDictionary(prop => prop.Id, prop => prop.Resolution);

                response.Content = new JsonContent(result);
                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }

        }

        [HttpPost]
        public HttpResponseMessage CampaignAdd(dynamic data)
        {

            try
            {
                string name = data.Name;
                string description = data.Description;
                Orientation orientation = (Orientation)data.Orientation;
                string resolution = data.Resolution;
                Guid screentypeid = data.ScreenTemplate.screenType;

                var template = ScreenTemplate(orientation, resolution);
                var screenType = ScreenType(screentypeid);

                var repository = factory.GetRepository<Repository<Campaigns>>();
                var repositoryChannel = factory.GetRepository<Repository<Channel>>();

                var campaign = new Campaigns()
                {
                    Name = name,
                    Description = description,
                    ScreenTemplate = template,
                };

                var timeline = new Timeline();

                factory.OnTransaction(() =>
                {

                    var channels = screenType.CreateChannels();

                    timeline.AddScreenType(screenType);

                    foreach (var c in channels)
                    {
                        var channel = repositoryChannel.SaveOrUpdate(c);
                        timeline.AddChannels(channel);
                    }

                    // must get channel id:Z                
                    campaign.AddTimeline(timeline);

                    repository.SaveOrUpdate(campaign);

                });

                var response = new HttpResponseMessage(HttpStatusCode.OK);

                response.Content = new JsonContent(new
                {
                    Campaign = campaign.Id,
                });

                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }


        }

        private ScreenTemplate ScreenTemplate(Orientation orientation, string resolution)
        {

            var repository = factory.GetRepository<Repository<ScreenResolution>>();

            var resx = resolution.Split('x');
            var width = Convert.ToInt32(resx.First());
            var height = Convert.ToInt32(resx.Last());

            var query = repository.GetQueryable();

            var screenresolution = query.Where(o => o.Orientation.Orientation == orientation)
                                         .Where(o => o.Width == width)
                                         .Where(o => o.Height == height)
                                         .FirstOrDefault();



            return new ScreenTemplate() { Resolution = screenresolution };
        }

        private ScreenType ScreenType(Guid id)
        {

            var repository = factory.GetRepository<Repository<ScreenType>>();

            return repository.GetById(id, false);

        }

        [HttpPost]
        public HttpResponseMessage CampaignsTimelineAdd(dynamic data)
        {

            Guid campaignId = data.Campaign;
            Guid screenTypeId = data.ScreenType;

            var screenType = GetScreenType(screenTypeId);

            var repository = factory.GetRepository<Repository<Campaigns>>();
            var repositoryChannel = factory.GetRepository<Repository<Channel>>();

            var query = repository.GetQueryable();
            var campaign = query.Where(o => o.Id == campaignId)
                                .FirstOrDefault();


            var timeline = new Timeline();

            factory.OnTransaction(() =>
            {

                var channels = screenType.CreateChannels();


                timeline.AddScreenType(screenType);

                foreach (var c in channels)
                {
                    var channel = repositoryChannel.SaveOrUpdate(c);
                    timeline.AddChannels(channel);
                }

                // must get channel id:Z                
                campaign.AddTimeline(timeline);


            });

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            var result = new
            {
                Timeline = timeline.Id
            };

            response.Content = new JsonContent(result);

            return response;
        }

        [HttpPost]
        public HttpResponseMessage CampaignsTimelineRemove(dynamic data)
        {

            try
            {
                Guid campaignId = data.Campaign;
                Guid timelineId = data.Timeline;

                var response = new HttpResponseMessage(HttpStatusCode.OK);

                var repository = factory.GetRepository<Repository<Campaigns>>();

                factory.OnTransaction(() =>
                {

                    var campaign = repository.GetById(campaignId, false);

                    var timeline = campaign.Timelines.Where(o => o.Id == timelineId).FirstOrDefault();

                    campaign.Timelines.Remove(timeline);

                });

                response.Content = new JsonContent(new
                {
                    Campaign = campaignId,
                });

                return response;
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }

        #endregion



        private Resource GetResource(Guid resourceID)
        {

            var repository = factory.GetRepository<Repository<Resource>>();

            var query = repository.GetQueryable();

            var resource = query.Where(o => o.Id == resourceID)
                                .FirstOrDefault();

            return resource;
        }

        private ScreenType GetScreenType(Guid screenType)
        {

            var repository = factory.GetRepository<Repository<ScreenType>>();
            var query = repository.GetQueryable();

            return query.Where(o => o.Id == screenType).FirstOrDefault();


        }

        #region Get SearchMedia


        [HttpGet]
        public dynamic GetSearchMedia(string key)
        {
            var repository = factory.GetRepository<Repository<Resource>>();
            var query = repository.GetQueryable();

            var list = query.Where(o => o.Name.Contains(key)).ToList();

            var result = list.Select((o, i) =>
                  new
                  {
                      Id = o.Id,
                      SeqNo = i.ToString(),
                      MediaType = o.ResourceType == ResourceType.Video ? "Video" : o.ResourceType == ResourceType.Image ? "Image" : "Music",
                      Name = o.Name,
                      KeyInput = ""
                  }
            );

            var result2 = Json(result.ToArray());

            return result2;
        }

        #endregion


        [HttpGet]
        public dynamic GetTickers()
        {


            Dictionary<int, string> data = new Dictionary<int, string>();
            data.Add(1, "MAKLUMAN 01");
            data.Add(2, "MAKLUMAN 02");
            data.Add(3, "MAKLUMAN 03");
            data.Add(4, "MAKLUMAN 04");
            data.Add(5, "MAKLUMAN 05");

            var result = data.Select((o, i) =>
                   new
                   {
                       Id = o.Key,
                       Name = o.Value

                   }
             );

            var result2 = Json(result.ToArray());

            return result2;

        }

        [HttpPost]
        public void SaveTicker(dynamic data)
        {
            try
            {
                factory.OnTransaction(() =>
                {
                    Guid masterGuid = SaveResourceTitleandFontProperties(data);

                    if (masterGuid != new Guid())
                    {
                        foreach (var ticker in data.Tickers)
                        {
                            //dynamic tickerObject = JsonConvert.DeserializeObject<dynamic>(ticker.Value.ToString());
                            var tickerData = new Ticker()
                            {
                                MId = masterGuid,
                                Title = ticker.Title,
                                Display = true,
                                OrderBy = ticker.OrderBy,
                            };

                            var repositoryTicker = factory.GetRepository<Repository<Ticker>>();
                            repositoryTicker.SaveOrUpdate(tickerData);
                        }
                    }
                });

            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        public void HtmlAdd(dynamic data)
        {
            try
            {
                factory.OnTransaction(() =>
                {
                    Guid masterGuid = SaveResourceTitleandFontProperties(data);
                });
            }
            catch (Exception ex)
            {

            }
        }        

        private Guid SaveResourceTitleandFontProperties(dynamic data)
        {
            Guid masterGuid = new Guid();
            try
            {
                var resourceTitle = new ResourceTitle()
                {
                    Name = data.Name,
                    Display = true,
                    Title = data.MainTitle,
                    ResourceType = data.ResourceType,
                    CreateDate = System.DateTime.Now,//To do assign createdby using logged in user
                    //UpdateDate = System.DateTime.Now,
                };

                var resourceFontProperties = new ResourceFontProperties()
                {
                    FontSize = data.FontSize,
                    ForeColor = data.ForeColor.ToString(),
                    BackgroundColor = data.Backcolor.ToString(),
                    Font = data.Font,
                };

                var repositoryResourceTitle = factory.GetRepository<Repository<ResourceTitle>>();
                repositoryResourceTitle.SaveOrUpdate(resourceTitle);
                masterGuid = resourceTitle.Id;

                resourceFontProperties.MId = masterGuid;
                var repositoryResourceFont = factory.GetRepository<Repository<ResourceFontProperties>>();
                repositoryResourceFont.SaveOrUpdate(resourceFontProperties);
            }
            catch (Exception ex)
            {

            }
            return masterGuid;
        }

        [HttpGet]
        public dynamic GetHTMLData(Guid id)
        {
            var _resource = GetSingleResource(id);
            var _fontProperties = GetResourceFontProperties(id);
            return Json(new
            {
                resource = _resource,
                fontProperties = _fontProperties
            });
        }

        [HttpGet]
        public dynamic GetCCTVData(Guid id)
        {
            var _resource = GetSingleResource(id);
            return Json(new
            {
                resource = _resource
            });
        }

        public dynamic GetSingleResource(Guid id)
        {
            var repository = factory.GetRepository<Repository<ResourceTitle>>();
            var query = repository.GetQueryable();
            return query.Where(x => x.Id == id).FirstOrDefault();
        }

        public dynamic GetResourceFontProperties(Guid id)
        {
            var repository = factory.GetRepository<Repository<ResourceFontProperties>>();
            var query = repository.GetQueryable();
            return query.Where(x => x.MId == id).FirstOrDefault();
        }
    }


}