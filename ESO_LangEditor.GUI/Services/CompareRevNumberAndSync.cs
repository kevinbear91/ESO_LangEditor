using AutoMapper;
using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.EnumTypes;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore;
using ESO_LangEditor.GUI.NetClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ESO_LangEditor.GUI.Services
{
    public class CompareRevNumberAndSync
    {

        private IMapper _mapper;
        private Dictionary<Guid, ReviewReason> langtextUpdateTypeDict;
        private int RevCompareNum = 0;
        private int TaskCount = 0;
        private LangtextNetService langtextNetService = new LangtextNetService(App.ServerPath);
        private LangTextRepoClientService _langTextRepoClient = new LangTextRepoClientService();

        private List<LangTextDto> langTextDtos = new List<LangTextDto>();
        private List<LangTextDto> added;
        private List<LangTextDto> changed;
        private List<Guid> deleted;


        public CompareRevNumberAndSync()
        {
            _mapper = App.Mapper;
        }



        public async Task CompareRevNumber()
        {
            int revNumberClinet = await GetLangtextRevNumberInClient();
            int revNumberServer = await GetLangtextRevNumberInServer();

            RevCompareNum = revNumberServer - revNumberClinet;
            int count = 0;

            for (int i = 1; i <= RevCompareNum; i++)
            {
                int id = revNumberClinet + i;
                var langtextRevisedListDto = await GetLangTextRevisedDtos(id);
                langtextUpdateTypeDict = new Dictionary<Guid, ReviewReason>();

                added = null;
                changed = null;
                deleted = null;

                foreach (var langtextRev in langtextRevisedListDto)
                {
                    langtextUpdateTypeDict.Add(langtextRev.LangtextID, langtextRev.ReasonFor);
                }

                try
                {
                    langTextDtos = await langtextNetService.GetLangtextByGuidListAsync(langtextUpdateTypeDict.Keys.ToList(),
                    App.LangConfig.UserAuthToken);
                }
                catch (HttpRequestException ex)
                {
                    MessageBox.Show(ex.Message);
                    break;
                }

                added = new List<LangTextDto>();
                changed = new List<LangTextDto>();
                deleted = new List<Guid>();

                foreach (var lang in langTextDtos)
                {
                    if (langtextUpdateTypeDict.TryGetValue(lang.Id, out ReviewReason reviewReason))
                    {
                        switch (reviewReason)
                        {
                            case ReviewReason.NewAdded:
                                added.Add(lang);
                                langtextUpdateTypeDict.Remove(lang.Id);
                                break;
                            case ReviewReason.ZhChanged:
                                changed.Add(lang);
                                langtextUpdateTypeDict.Remove(lang.Id);
                                break;
                            case ReviewReason.EnChanged:
                                changed.Add(lang);
                                langtextUpdateTypeDict.Remove(lang.Id);
                                break;
                            case ReviewReason.Deleted:
                                deleted.Add(lang.Id);
                                Debug.WriteLine("Deleted ID: {0} ", lang.Id.ToString());
                                break;
                        }
                    }
                }
                deleted.AddRange(langtextUpdateTypeDict.Keys.ToList());

                //SortListFromServer();
            }

            await ApplyToDatabase();


            //if (revNumberClinet < revNumberServer)
            //{
            //    var langtextRevisedListDto = await GetLangTextRevisedDtos();

            //    langtextUpdateTypeDict = new Dictionary<Guid, ReviewReason>();

            //    foreach(var langtextRev in langtextRevisedListDto)
            //    {
            //        langtextUpdateTypeDict.Add(langtextRev.LangtextID, langtextRev.ReasonFor);
            //    }

            //}


        }

        private void SortListFromServer()
        {
            added = new List<LangTextDto>();
            changed = new List<LangTextDto>();
            deleted = new List<Guid>();

            foreach (var lang in langTextDtos)
            {
                if (langtextUpdateTypeDict.TryGetValue(lang.Id, out ReviewReason reviewReason))
                {
                    switch (reviewReason)
                    {
                        case ReviewReason.NewAdded:
                            added.Add(lang);
                            langtextUpdateTypeDict.Remove(lang.Id);
                            break;
                        case ReviewReason.ZhChanged:
                            changed.Add(lang);
                            langtextUpdateTypeDict.Remove(lang.Id);
                            break;
                        case ReviewReason.EnChanged:
                            changed.Add(lang);
                            langtextUpdateTypeDict.Remove(lang.Id);
                            break;
                        case ReviewReason.Deleted:
                            deleted.Add(lang.Id);
                            Debug.WriteLine("Deleted ID: {0} ", lang.Id.ToString());
                            break;
                    }
                }
            }
            deleted.AddRange(langtextUpdateTypeDict.Keys.ToList());
        }

        private async Task ApplyToDatabase()
        {
            if (changed.Count >= 1)
            {
                var updatedlang = _mapper.Map<List<LangTextClient>>(changed);
                await _langTextRepoClient.UpdateLangtexts(updatedlang);
            }

            if (added.Count >= 1)
            {
                var langtexts = _mapper.Map<List<LangTextClient>>(added);
                await _langTextRepoClient.AddLangtexts(langtexts);
            }

            if (deleted.Count >= 1)
            {
                List<LangTextClient> langtextForDelete = new List<LangTextClient>();

                foreach (var id in deleted)
                {
                    var langtextDto = await _langTextRepoClient.GetLangTextByGuidAsync(id);
                    //var langtext = _mapper.Map<LangTextClient>(langtextDto);
                    langtextForDelete.Add(langtextDto);
                }
                //await _langTextRepoClient.DeleteLangtexts(langtextForDelete);
            }
        }

        private async Task<List<LangTextRevisedDto>> GetLangTextRevisedDtos(int id)
        {
            try
            {
                return await langtextNetService.GetLangTextRevisedDtosAsync(id, App.LangConfig.UserAuthToken);
            }
            catch (HttpRequestException ex)
            {
                return null;
            }
        }

        private async Task<int> GetLangtextRevNumberInServer()
        {
            try
            {
                return await langtextNetService.GetLangTextRevisedNumberAsync(App.LangConfig.UserAuthToken);
            }
            catch(HttpRequestException ex)
            {
                return 0;
            }


            
        }

        private async Task<int> GetLangtextRevNumberInClient()
        {
            int langtextRev = 0;

            using (var db = new LangtextClientDbContext(App.DbOptionsBuilder))
            {
                var langtextRevNumber = await db.LangtextRevNumber.FindAsync(1);
                langtextRev = langtextRevNumber.LangTextRev;
                db.Dispose();
            }

            return langtextRev;
        }

    }
}
