using Autofac;
using HIS.Data;
using HIS.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIS.CommandConsole.DivisionCreator
{
    public class ScreenCreator
    {
        private IRepositoryFactory factory;
        public ScreenCreator(AutofacBootStrap a)
        {
            factory = a.Container.Resolve<IRepositoryFactory>();
            CreateData();
        }

        private void CreateData()
        {
            var repository = factory.GetRepository<Repository<ScreenResolution>>();
            var query = repository.GetQueryable();


            // var result = query.Where(x => x.Orientation.Id == new Guid("7AEB6CA3-2331-E411-A8FF-D6C8174F0792")).ToList();
            var result = query.OrderBy(x => x.Orientation).ToList();
            result.ForEach(o =>
            {

                double width = o.Width;
                double height = o.Height;


                for (int c = 1; c <= 10; c++)
                {

                    factory.OnTransaction(() =>
                                  {

                                      var screentype = new ScreenType();

                                      if (o.Orientation.Id == new Guid("7AEB6CA3-2331-E411-A8FF-D6C8174F0792"))
                                          screentype.Name = "ScreenTypeH" + (c);

                                      else if (o.Orientation.Id == new Guid("7BEB6CA3-2331-E411-A8FF-D6C8174F0792"))
                                          screentype.Name = "ScreenTypeV" + (c);

                                      o.AddScreenType(screentype);

                                  });

                }



                var repScreenType = factory.GetRepository<Repository<ScreenType>>();
                var qScreenType = repScreenType.GetQueryable();
                var rScreenType = qScreenType.Where(x => x.ScreenResolution.Id == o.Id).ToList();

                int loop = 0;


                rScreenType.ForEach(o2 =>
                {

                    string type = o2.Name.Length < 13 ? o2.Name.Substring(11, 1) : o2.Name.Substring(11, 2);
                    //string orientation = o2.Name.Substring(10, 1);

                    List<double> list = null;

                    if (type == "2" || type == "3")
                        loop = 2;

                    else if (type == "4" || type == "5" || type == "10")
                        loop = 4;

                    else if (type == "6" || type == "7" || type == "8" || type == "9")
                        loop = 3;

                    else
                        loop = 1;


                    #region HORIZONTAL
                    if (o2.Name.Contains("H"))
                    {
                        for (int i = 0; i < loop; i++)
                        {

                            factory.OnTransaction(() =>
                            {
                                ScreenDivision sd = new ScreenDivision();
                                sd.Name = "ScreenDivision" + i;
                                sd.ScreenType = o2;
                                list = ScreenDivisionH(width, height, i, o2.Name);
                                if (list != null)
                                {
                                    sd.X = Convert.ToInt32(list[0]);
                                    sd.Y = Convert.ToInt32(list[1]);
                                    sd.Width = Convert.ToInt32(list[2]);
                                    sd.Height = Convert.ToInt32(list[3]);
                                }

                                o2.ScreenDivisions.Add(sd);
                            });

                        }
                    }
                    #endregion HORIZONTAL

                    #region VERTICAL
                    if (o2.Name.Contains("V"))
                    {
                        for (int i = 0; i < loop; i++)
                        {

                            factory.OnTransaction(() =>
                            {
                                ScreenDivision sd = new ScreenDivision();
                                sd.Name = "ScreenDivision" + i;
                                sd.ScreenType = o2;
                                list = ScreenDivisionV(width, height, i, o2.Name);
                                if (list != null)
                                {
                                    sd.X = Convert.ToInt32(list[0]);
                                    sd.Y = Convert.ToInt32(list[1]);
                                    sd.Width = Convert.ToInt32(list[2]);
                                    sd.Height = Convert.ToInt32(list[3]);
                                }

                                o2.ScreenDivisions.Add(sd);
                            });

                        }
                    }
                    #endregion VERTICAL


                });

            });




        }


        private List<double> ScreenDivisionH(double width, double height, int seq, string type)
        {
            List<double> list = new List<double>();



            if (seq == 0)
            {


                switch (type)
                {
                    case "ScreenTypeH1":
                        GetList(list, 0, 0, (width / 12) * 12, (height / 12) * 12);

                        break;
                    case "ScreenTypeH2":
                        GetList(list, 0, 0, (width / 12) * 4, (height / 12) * 12);
                        break;
                    case "ScreenTypeH3":
                        GetList(list, 0, 0, (width / 12) * 8, (height / 12) * 12);
                        break;
                    case "ScreenTypeH4":
                        GetList(list, 0, 0, (width / 12) * 4, (height / 12) * 12);
                        break;
                    case "ScreenTypeH5":
                        GetList(list, 0, 0, (width / 12) * 6, (height / 12) * 4);
                        break;
                    case "ScreenTypeH6":
                        GetList(list, 0, 0, (width / 12) * 4, (height / 12) * 12);
                        break;
                    case "ScreenTypeH7":
                        GetList(list, 0, 0, (width / 12) * 8, (height / 12) * 6);
                        break;
                    case "ScreenTypeH8":
                        GetList(list, 0, 0, (width / 12) * 6, (height / 12) * 6);
                        break;
                    case "ScreenTypeH9":
                        GetList(list, 0, 0, (width / 12) * 12, (height / 12) * 6);
                        break;
                    case "ScreenTypeH10":
                        GetList(list, 0, 0, (width / 12) * 6, (height / 12) * 6);
                        break;

                }



            }

            if (seq == 1)
            {
                switch (type)
                {
                    case "ScreenTypeH2":
                        GetList(list, (width / 12) * 4, 0, (width / 12) * 8, (height / 12) * 12);
                        break;
                    case "ScreenTypeH3":
                        GetList(list, (width / 12) * 8, 0, (width / 12) * 4, (height / 12) * 12);

                        break;
                    case "ScreenTypeH4":
                        GetList(list, (width / 12) * 4, 0, (width / 12) * 4, (height / 12) * 6);
                        break;
                    case "ScreenTypeH5":
                        GetList(list, (width / 12) * 6, 0, (width / 12) * 6, (height / 12) * 8);
                        break;

                    case "ScreenTypeH6":
                        GetList(list, (width / 12) * 4, 0, (width / 12) * 8, (height / 12) * 6);
                        break;

                    case "ScreenTypeH7":
                        GetList(list, (width / 12) * 8, 0, (width / 12) * 4, (height / 12) * 12);
                        break;

                    case "ScreenTypeH8":
                        GetList(list, (width / 12) * 6, 0, (width / 12) * 6, (height / 12) * 6);
                        break;

                    case "ScreenTypeH9":
                        GetList(list, 0, (height / 12) * 6, (width / 12) * 6, (height / 12) * 6);

                        break;
                    case "ScreenTypeH10":
                        GetList(list, (width / 12) * 6, 0, (width / 12) * 6, (height / 12) * 6);
                        break;
                }

            }


            if (seq == 2)
            {
                switch (type)
                {
                    case "ScreenTypeH4":
                        GetList(list, (width / 12) * 8, 0, (width / 12) * 4, (height / 12) * 6);

                        break;
                    case "ScreenTypeH5":
                        GetList(list, 0, (height / 12) * 4, (width / 12) * 6, (height / 12) * 4);
                        break;

                    case "ScreenTypeH6":
                        GetList(list, (width / 12) * 4, (height / 12) * 6, (width / 12) * 8, (height / 12) * 6);
                        break;

                    case "ScreenTypeH7":
                        GetList(list, 0, (height / 12) * 6, (width / 12) * 8, (height / 12) * 6);
                        break;

                    case "ScreenTypeH8":
                        GetList(list, 0, (height / 12) * 6, (width / 12) * 12, (height / 12) * 6);
                        break;

                    case "ScreenTypeH9":
                        GetList(list, (width / 12) * 6, (height / 12) * 6, (width / 12) * 6, (height / 12) * 6);
                        break;

                    case "ScreenTypeH10":
                        GetList(list, 0, (height / 12) * 6, (width / 12) * 6, (height / 12) * 6);
                        break;
                }

            }
            if (seq == 3)
            {
                switch (type)
                {
                    case "ScreenTypeH4":
                        GetList(list, (width / 12) * 4, (height / 12) * 6, (width / 12) * 8, (height / 12) * 6);

                        break;
                    case "ScreenTypeH5":
                        GetList(list, 0, (height / 12) * 8, (width / 12) * 12, (height / 12) * 4);

                        break;

                    case "ScreenTypeH10":
                        GetList(list, (width / 12) * 6, (height / 12) * 6, (width / 12) * 6, (height / 12) * 6);
                        break;
                }
            }

            return list;

        }

        private List<double> ScreenDivisionV(double width, double height, int seq, string type)
        {
            List<double> list = new List<double>();



            if (seq == 0)
            {


                switch (type)
                {
                    case "ScreenTypeV1":
                        GetList(list, 0, 0, (width / 12) * 12, (height / 12) * 12);

                        break;
                    case "ScreenTypeV2":
                        GetList(list, 0, 0, (width / 12) * 12, (height / 12) * 8);
                        break;
                    case "ScreenTypeV3":
                        GetList(list, 0, 0, (width / 12) * 12, (height / 12) * 4);
                        break;
                    case "ScreenTypeV4":
                        GetList(list, 0, 0, (width / 12) * 6, (height / 12) * 4);
                        break;
                    case "ScreenTypeV5":
                        GetList(list, 0, 0, (width / 12) * 12, (height / 12) * 4);
                        break;
                    case "ScreenTypeV6":
                        GetList(list, 0, 0, (width / 12) * 6, (height / 12) * 8);
                        break;
                    case "ScreenTypeV7":
                        GetList(list, 0, 0, (width / 12) * 12, (height / 12) * 4);
                        break;
                    case "ScreenTypeV8":
                        GetList(list, 0, 0, (width / 12) * 6, (height / 12) * 6);
                        break;
                    case "ScreenTypeV9":
                        GetList(list, 0, 0, (width / 12) * 12, (height / 12) * 6);
                        break;
                    case "ScreenTypeV10":
                        GetList(list, 0, 0, (width / 12) * 6, (height / 12) * 6);
                        break;

                }



            }

            if (seq == 1)
            {
                switch (type)
                {
                    case "ScreenTypeV2":
                        GetList(list, 0, (height / 12) * 8, (width / 12) * 12, (height / 12) * 4);
                        break;
                    case "ScreenTypeV3":
                        GetList(list, 0, (height / 12) * 4, (width / 12) * 12, (height / 12) * 8);

                        break;
                    case "ScreenTypeV4":
                        //GetList(list, (width / 12) * 6, (height / 12) * 8, (width / 12) * 6, (height / 12) * 8);
                        GetList(list, (width / 12) * 6, 0, (width / 12) * 6, (height / 12) * 8);
                        break;
                    case "ScreenTypeV5":
                        GetList(list, 0, (height / 12) * 4, (width / 12) * 12, (height / 12) * 4);
                        break;

                    case "ScreenTypeV6":
                        // GetList(list, (width / 12) * 6, (height / 12) * 8, (width / 12) * 6, (height / 12) * 8);
                        GetList(list, (width / 12) * 6, 0, (width / 12) * 6, (height / 12) * 8);
                        break;

                    case "ScreenTypeV7":
                        GetList(list, 0, (height / 12) * 4, (width / 12) * 6, (height / 12) * 8);
                        break;

                    case "ScreenTypeV8":
                        GetList(list, (width / 12) * 6, 0, (width / 12) * 6, (height / 12) * 6);
                        break;

                    case "ScreenTypeV9":
                        GetList(list, 0, (height / 12) * 6, (width / 12) * 6, (height / 12) * 6);

                        break;
                    case "ScreenTypeV10":
                        GetList(list, (width / 12) * 6, 0, (width / 12) * 6, (height / 12) * 6);
                        break;
                }

            }


            if (seq == 2)
            {
                switch (type)
                {
                    case "ScreenTypeV4":
                        GetList(list, 0, (height / 12) * 4, (width / 12) * 6, (height / 12) * 4);

                        break;
                    case "ScreenTypeV5":
                        GetList(list, (width / 12) * 6, (height / 12) * 4, (width / 12) * 6, (height / 12) * 8);
                        break;

                    case "ScreenTypeV6":
                        GetList(list, 0, (height / 12) * 8, (width / 12) * 12, (height / 12) * 4);
                        break;

                    case "ScreenTypeV7":
                        GetList(list, (width / 12) * 6, (height / 12) * 4, (width / 12) * 6, (height / 12) * 8);
                        break;

                    case "ScreenTypeV8":
                        GetList(list, 0, (height / 12) * 6, (width / 12) * 12, (height / 12) * 6);
                        break;

                    case "ScreenTypeV9":
                        GetList(list, (width / 12) * 6, (height / 12) * 6, (width / 12) * 6, (height / 12) * 6);
                        break;

                    case "ScreenTypeV10":
                        GetList(list, 0, (height / 12) * 6, (width / 12) * 6, (height / 12) * 6);
                        break;
                }

            }
            if (seq == 3)
            {
                switch (type)
                {
                    case "ScreenTypeV4":
                        GetList(list, 0, (height / 12) * 8, (width / 12) * 12, (height / 12) * 4);

                        break;
                    case "ScreenTypeV5":
                        GetList(list, 0, (height / 12) * 8, (width / 12) * 6, (height / 12) * 4);

                        break;

                    case "ScreenTypeV10":
                        GetList(list, (width / 12) * 6, (height / 12) * 6, (width / 12) * 6, (height / 12) * 6);
                        break;
                }
            }

            return list;

        }

        private List<double> GetList(List<double> list, double x, double y, double wid, double hei)
        {
            list.Add(x);
            list.Add(y);
            list.Add(wid);
            list.Add(hei);

            return list;
        }




        #region FARHAN
        private void Test2()
        {

            var repository = factory.GetRepository<Repository<ScreenResolution>>();

            var screen = repository.GetById(new Guid("63263AD5-2331-E411-A8FF-D6C8174F0792"), false);

            factory.OnTransaction(() =>
            {
                for (int c = 0; c < 3; c++)
                {
                    var screentype = new ScreenType();
                    screentype.Name = "ScreenDivision" + c;
                    screen.AddScreenType(screentype);
                }

            });
        }
        #endregion
        #region SQL


        // --*********************************
        // select * from [hisdb].[dbo].[screendivision] 
        // select * from [hisdb].[dbo].[screentype]   
        // ________________________________________________________
        //       select * from [hisdb].[dbo].[screenresolution]


        //        select a.* ,b.name ,b.id,c.width,c.height
        //        from [hisdb].[dbo].[screendivision] a join  [hisdb].[dbo].[screentype] b
        //        on a.screentypeid = b.id
        //        join [hisdb].[dbo].[screenresolution] c
        //        on c.id = b.screenresolutionid
        //        --where b.screenresolutionid = '63263ad5-2331-e411-a8ff-d6c8174f0792'
        //        order by c.id,a.screentypeid,b.name,a.name



        //        delete from [screendivision] where screentypeid not in 
        //        (
        //        select id  from [screentype]
        //        where screenresolutionid 
        //         in ('f0d5b3bb-2331-e411-a8ff-d6c8174f0792')
        //        )
        //        delete from [screentype] where screenresolutionid not in ('f0d5b3bb-2331-e411-a8ff-d6c8174f0792');

        //--**********************************************************************
        // select a.id , c.name from [channels] a join [screendivision] b
        // on a.screendivisionid = b.id
        // join [screentype] c 
        //  on b.screentypeid = c.id
        //delete block
        //  delete from channels
        //  where id in
        //  (
        //    'bbac59fe-c5f9-4f86-8b46-a3e000a5f4a3',
        //    '009a3870-de49-4bb7-8cd8-a3e000a5f4a3',
        //    '60cdef75-c487-4a2a-a462-a3e000a5f4a3',
        //    'ccd3b95e-1734-454b-b036-a3e000a5f4a3'
        //  )

        //  select a.* , b.name from timelines a join   [screentype] b
        //  on b.id = a.screentypeid

        //  delete from timelines
        //  where id in
        //  (
        //    '4636cd63-c3c1-435f-831d-a3e000a5f4ac',
        //    '9604f00f-2ff7-4ef3-8f86-a3e000a5f4ac',
        //    'd9f10897-2d46-4af6-a6f8-a3e000a5f4ac',
        //    '16b8fb57-3f1d-4945-b35c-a3e000a5f4ac'
        //  )

        #endregion
    }


}
