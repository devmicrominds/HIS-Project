using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Web.UI
{
    public class ResourcesMenuPresenter : GenericPresenter<IResourcesMenuView>
    {
        private IRepositoryFactory factory;

        public ResourcesMenuPresenter(IRepositoryFactory factory)
        {
            this.factory = factory;
        }
        public override void View_Load(object sender, EventArgs e)
        {
            if (View.IsInitialRun)
                View.Parent.AddControl(ControlPath.IResourcesMenuSearchView, View.MediaPlaceHolder);

            if (View.Parent.IsPartialRendering)
            {

                string action = View.PostParameter.Action;

                switch (action)
                {

                    case "NavigateTo": NavigateTo();
                        break;
                    case "FileUpload": FileUpload();
                        break;
                    case "ShowGallery": ShowGallery();
                        break;
                    case "SaveMediaCategory": SaveMediaCategory();
                        break;
                    case "SaveCCTVData": SaveCCTVData();
                        break;
                    case "SaveHTMLData": SaveHTMLData();
                        break;

                }
            }
        }

        private void SaveMediaCategory()
        {

            /* go to parent to handle all format
             * View.Parent.AddControl(ControlPath.IResourcesImageView, View.MediaPlaceHolder);*/
            var p = View.PostParameter;
            var data = p.Data;


            var repository = factory.GetRepository<Repository<MediaCategory>>();
            string groupid = data.GroupID;

            var oMedCat = new MediaCategory()
            {
                Name = View.PostParameter.Data.Name,
                Description = View.PostParameter.Data.Description,
                MediaResourceType = View.PostParameter.Data.Resource,
                ColorCode = View.PostParameter.Data.Color

            };


            factory.OnTransaction(() =>
            {
                oMedCat = repository.SaveOrUpdate(oMedCat);
            });

            NavigateTo();

        }

        public void SaveCCTVData()
        {
            try
            {
                var p = View.PostParameter;
                var data = p.Data;

                string id = data.ID;
                bool delete = data.Operation;
                Guid recordId = id == null ? Guid.Parse("00000000-0000-0000-0000-000000000000") : Guid.Parse(id);

                var repositoryResourceTitle = factory.GetRepository<Repository<ResourceTitle>>();

                var query = repositoryResourceTitle.GetQueryable();
                var resourceTitle = query.Where(o => o.Id == recordId).FirstOrDefault();

                if (!delete)
                {
                    if (resourceTitle == null)
                    {
                        resourceTitle = new ResourceTitle()
                        {
                            Name = data.Name,
                            Display = true,
                            Title = data.MainTitle,
                            ResourceType = data.ResourceType,
                            CreateDate = System.DateTime.Now,
                            //CreatedBy = System.DateTime.Now,//To do assign createdby using logged in user
                        };
                    }
                    else
                    {
                        resourceTitle.Name = data.Name;
                        resourceTitle.Title = data.MainTitle;
                        resourceTitle.UpdateDate = DateTime.Now;
                        //resourceTitle.UpdatedBy =  TO Do assign logged in user
                    }

                    factory.OnTransaction(() =>
                    {
                        resourceTitle = repositoryResourceTitle.SaveOrUpdate(resourceTitle);
                    });
                }
                else
                {
                    factory.OnTransaction(() =>
                    {
                        repositoryResourceTitle.Delete(resourceTitle);
                    });
                }

                NavigateTo();
            }
            catch (Exception ex)
            {

            }
        }

        public void SaveHTMLData()
        {
            try
            {
                var p = View.PostParameter;
                var data = p.Data;

                string id = data.ID;
                bool delete = data.Operation;
                Guid recordId = id == null ? Guid.Parse("00000000-0000-0000-0000-000000000000") : Guid.Parse(id);

                var repositoryResourceTitle = factory.GetRepository<Repository<ResourceTitle>>();

                var query = repositoryResourceTitle.GetQueryable();
                var resourceTitle = query.Where(o => o.Id == recordId).FirstOrDefault();

                var repositoryResourceFont = factory.GetRepository<Repository<ResourceFontProperties>>();
                var queryFont = repositoryResourceFont.GetQueryable();
                var resourceFont = queryFont.Where(o => o.MId == recordId).FirstOrDefault();

                if (!delete)
                {
                    if (resourceTitle == null)
                    {
                        resourceTitle = new ResourceTitle()
                        {
                            Name = data.Name,
                            Display = true,
                            Title = data.MainTitle,
                            ResourceType = data.ResourceType,
                            CreateDate = System.DateTime.Now,
                            //CreatedBy = System.DateTime.Now,//To do assign createdby using logged in user
                        };

                        resourceFont = new ResourceFontProperties()
                        {
                            FontSize = data.FontSize,
                            ForeColor = data.ForeColor.ToString(),
                            BackgroundColor = data.Backcolor.ToString(),
                            Font = data.Font,
                        };
                    }
                    else
                    {
                        resourceTitle.Name = data.Name;
                        resourceTitle.Title = data.MainTitle;
                        resourceTitle.UpdateDate = DateTime.Now;
                        //resourceTitle.UpdatedBy =  TO Do assign logged in user

                        resourceFont.FontSize = data.FontSize;
                        resourceFont.ForeColor = data.ForeColor.ToString();
                        resourceFont.BackgroundColor = data.Backcolor.ToString();
                        resourceFont.Font = data.Font;
                    }

                    factory.OnTransaction(() =>
                    {
                        resourceTitle = repositoryResourceTitle.SaveOrUpdate(resourceTitle);
                        resourceFont.MId = resourceTitle.Id;
                        resourceFont = repositoryResourceFont.SaveOrUpdate(resourceFont);
                    });
                }
                else
                {
                    factory.OnTransaction(() =>
                    {                        
                        repositoryResourceFont.Delete(resourceFont);
                        repositoryResourceTitle.Delete(resourceTitle);
                    });
                }

                NavigateTo();
            }
            catch (Exception ex)
            {

            }
        }

        private void FileUpload()
        {
            View.Parent.AddControl(ControlPath.IResourcesFileUploadView, View.MediaPlaceHolder);

            var Data = View.PostParameter.Data;
            string context = Data.Context;
            switch (context)
            {
                case "Image":
                    View.PostParameter.Data.AcceptedFiles = ".png,.jpeg,.jpg";
                    break;
                case "Video":
                    View.PostParameter.Data.AcceptedFiles = ".mp4";
                    break;
                case "Music":
                    View.PostParameter.Data.AcceptedFiles = ".mp3";
                    break;
            }

            View.JsonData = Json(View.PostParameter);

        }

        private void NavigateTo()
        {
            string target = View.PostParameter.Data.Context;
            switch (target)
            {
                case "Image":
                    View.Parent.AddControl(ControlPath.IResourcesImageView, View.MediaPlaceHolder);

                    break;
                case "Video":
                    View.Parent.AddControl(ControlPath.IResourcesVideoView, View.MediaPlaceHolder);
                    break;
                case "HTML5":
                    View.Parent.AddControl(ControlPath.IResourcesHTML5View, View.MediaPlaceHolder);
                    break;
                case "Stream":
                    View.Parent.AddControl(ControlPath.IResourceCCTVView, View.MediaPlaceHolder);
                    break;
                case "Music":
                    View.Parent.AddControl(ControlPath.IResourcesMusicView, View.MediaPlaceHolder);
                    break;
                case "ClickSearch":
                    View.Parent.AddControl(ControlPath.IResourcesMenuSearchView, View.MediaPlaceHolder);
                    break;
                case "SelectedItemSearch":
                    View.Parent.AddControl(ControlPath.IResourcesSearchGalleryView, View.MediaPlaceHolder);
                    break;
                case "Ticker":
                    View.Parent.AddControl(ControlPath.IResource_TickerView, View.MediaPlaceHolder);
                    break;



            }

            View.JsonData = Json(View.PostParameter);
        }

        private void ShowGallery()
        {

            View.Parent.AddControl(ControlPath.IResourcesMediaGalleryView, View.MediaPlaceHolder);


        }
    }
}