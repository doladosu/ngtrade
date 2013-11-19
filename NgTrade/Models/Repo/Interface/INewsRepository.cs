using System.Collections.Generic;

namespace NgTrade.Models.Repo.Interface
{
    public interface INewsRepository
    {
        List<string> NewsList();
        string NewsDetail(string sUrl);
    }
}
