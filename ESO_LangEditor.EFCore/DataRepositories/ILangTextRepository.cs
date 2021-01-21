using ESO_LangEditor.Core.Entities;
using ESO_LangEditor.Core.Models;
using ESO_LangEditor.EFCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ESO_LangEditor.EFCore.DataRepositories
{
    public interface ILangTextRepository : IBaseRepository<LangText>, IBaseRepository2<LangText, Guid>
    {



        /// <summary>
        /// 通过Guid获取单条文本
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        //Task<LangText> GetLangText(Guid guid);

        /// <summary>
        /// 获取所有文本（几十万条当场炸服务器）
        /// </summary>
        /// <returns></returns>
        //Task<IEnumerable<LangText>> GetAllLangTexts();

        /// <summary>
        /// 按条件获取文本
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        //IEnumerable<LangText> FindByCondition(Expression<Func<LangText, bool>> expression);

        /// <summary>
        /// 添加文本
        /// </summary>
        /// <param name="langText"></param>
        /// <returns></returns>
        //void Insert(LangTextDto langText);

        /// <summary>
        /// 更新文本
        /// </summary>
        /// <param name="updateLangText"></param>
        /// <returns></returns>
        //LangText Update(LangText updateLangText);

        /// <summary>
        /// 删除文本
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        //void Delete(Guid guid);
    }
}
