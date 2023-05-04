using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace Ray.Infrastructure.AutoTask
{
    public class TargetAccountManager<TTargetAccountInfo> where TTargetAccountInfo : TargetAccountInfo
    {
        public TargetAccountManager(IOptions<List<TTargetAccountInfo>> accountsOptions):this(accountsOptions.Value)
        {
        }

        public TargetAccountManager(List<TTargetAccountInfo> accounts)
        {
            for (int i = 0; i < accounts.Count; i++)
            {
                TargetAccountDic[i] = accounts[i];
            }
        }

        public int Index { get; set; }

        public int Count => TargetAccountDic.Count;

        private Dictionary<int, TTargetAccountInfo> TargetAccountDic { get; } = new();

        public TTargetAccountInfo CurrentTargetAccount => TargetAccountDic[Index];

        public void MoveToNext()
        {
            Index++;
        }

        public bool HasNext()
        {
            return Index + 1 < Count;
        }

        public void ReplaceCookieContainerWithCurrentAccount(CookieContainer source)
        {
            //clear existed by Expiring it
            source.GetAllCookies().ToList().ForEach(x => x.Expired = true);
            if (source.Count > 0)
            {
                var m = source.GetType().GetMethod("AgeCookies", BindingFlags.NonPublic | BindingFlags.Instance);
                m.Invoke(source, new object[] { null });
            }

            //add new
            source.Add(this.CurrentTargetAccount.MyCookieContainer.GetAllCookies());
        }

        public void UpdateCurrentCookieContainer(CookieContainer update)
        {
            var newCookieContainer = new CookieContainer();
            newCookieContainer.Add(update.GetAllCookies());
            this.CurrentTargetAccount.MyCookieContainer = newCookieContainer;
        }
    }

    public class TargetAccountInfo
    {
        public TargetAccountInfo()
        {
            
        }

        public TargetAccountInfo(string userName, string pwd, CookieContainer cookieContainer = null)
        {
            UserName = userName;
            Pwd = pwd;
            MyCookieContainer = cookieContainer ?? new CookieContainer();
        }

        public CookieContainer MyCookieContainer { get; set; }

        public string UserName { get; set; }

        public string Pwd { get; set; }
    }
}
