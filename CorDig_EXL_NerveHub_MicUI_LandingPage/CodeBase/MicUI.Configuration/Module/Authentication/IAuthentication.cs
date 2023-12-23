using MicUI.Configuration.Helper;
using MicUI.Configuration.Models;
using MicUI.Configuration.Models.Security;
using MicUI.Configuration.Services.Authentication;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;

namespace MicUI.Configuration.Module.Authentication
{
    public interface IAuthenticationTokenService
    {
        public BETenantInfo GetTanentInfo(string token);
        IList<BELdapUserInfo> GetUserList(string LoginName, string Domain);
    }

    public class AuthenticationTokenService : IAuthenticationTokenService
    {
        private readonly IAuthenticationApiService _authenticationApiService;

        public AuthenticationTokenService(IAuthenticationApiService authenticationApiService)
        {
            _authenticationApiService = authenticationApiService;
        }

        public BETenantInfo GetTanentInfo(string token)
        {
            var tenant = _authenticationApiService.SSOValidationAsync(token).GetAwaiter().GetResult();
            return tenant;
        }
        public IList<BELdapUserInfo> GetUserList(string LoginName, string Domain)
        {
            Errlog.ErrorLogFile("AuthenticationTokenService", "GetUserList", $"input value  LoginName: {LoginName} & Domain: {Domain}");

            IList<BELdapUserInfo> lstUserInfo = new List<BELdapUserInfo>();

            try
            {
                Errlog.ErrorLogFile("AuthenticationTokenService", "GetUserList Start", "try Block");

                lstUserInfo = GetUserList(LoginName, SearchByEnum.UserID);
            }
            catch (Exception ex)
            {
                Errlog.ErrorLogFile("AuthenticationTokenService", "GetUserList", $"Exception : {ex.Message}");
                throw;
            }

            Errlog.ErrorLogFile("AuthenticationTokenService", "GetUserList End", $"{lstUserInfo.Count}");

            return lstUserInfo;
        }

        private IList<BELdapUserInfo> GetUserList(string LoginName, SearchByEnum searchby)
        {
            Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", $"LoginName {LoginName}");

            IList<BELdapUserInfo> IuserList = new List<BELdapUserInfo>();

            Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", "foreach GetDomains");

            foreach (string d in GetDomains())
            {
                Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", $"Inside  foreach {d}");

                using (DirectoryEntry deDirEntry = new("LDAP://" + GetDomainFullName(d)))
                {
                    Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", "deDirEntry");

                    using (DirectorySearcher ds = new DirectorySearcher(deDirEntry))
                    {
                        Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", "DirectorySearcher");
                        Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", $"switch {searchby}");
                        switch (searchby)
                        {
                            case SearchByEnum.UserID:
                                ds.Filter = "(SAMAccountName=" + ValidationRegex.whitelist(LoginName) + "*)";
                                break;
                            case SearchByEnum.EmailID:
                                ds.Filter = "(mail=" + ValidationRegex.whitelist(LoginName) + "*)";
                                break;
                            case SearchByEnum.UserName:
                                ds.Filter = "(displayname=*" + ValidationRegex.whitelist(LoginName) + "*)";
                                break;
                        }

                        ds.PropertyNamesOnly = true;		// this will get names of only those properties to which a value is set

                        ds.PropertiesToLoad.Add("SAMAccountName");

                        // (PageSize) Maximum number of objects the server will return per page
                        // in a paged search. Default is 0, i.e. no paged search
                        ds.PageSize = 20;

                        // (ServerPageTimeLimit) the amount of time the server should observe to search a page of results
                        // default is -1, i.e. search indefinitely
                        ds.ServerPageTimeLimit = new TimeSpan((long)(60 * TimeSpan.TicksPerSecond));

                        // (SizeLimit) maximum number of objects the server returns in a search
                        // default is 0 - interpreted as server set default limit of 1000 entries
                        ds.SizeLimit = 20;

                        // (CacheResults) property by default is true
                        ds.CacheResults = true;

                        ds.ReferralChasing = ReferralChasingOption.None;

                        ds.Sort = new SortOption("name", System.DirectoryServices.SortDirection.Ascending);

                        Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", "before searching");

                        // start searching
                        using (SearchResultCollection src = ds.FindAll())
                        {
                            try
                            {
                                Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", "SearchResultCollection");

                                foreach (SearchResult sr in src)
                                {
                                    Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", "SearchResult sr");

                                    BELdapUserInfo oUser = new BELdapUserInfo();

                                    if (String.Compare(sr.GetDirectoryEntry().SchemaClassName, "user", true) == 0)
                                    {
                                        Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", "sr.GetDirectoryEntry().SchemaClassName");

                                        if (sr.GetDirectoryEntry().Properties["SAMAccountName"] != null)
                                        {
                                            Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", "sr.GetDirectoryEntry().Properties[\"SAMAccountName\"] != null");

                                            if (sr.GetDirectoryEntry().Properties["SAMAccountName"].Count > 0)
                                            {
                                                Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", "sr.GetDirectoryEntry().Properties[\"SAMAccountName\"].Count > 0");

                                                oUser.UserName = sr.GetDirectoryEntry().Properties["SAMAccountName"].Value.ToString() + " - " + sr.GetDirectoryEntry().Name.Substring(3);
                                            }
                                            else
                                            {
                                                oUser.UserName = string.Empty;
                                            }
                                        }
                                        if (sr.GetDirectoryEntry().Properties["mail"] != null)
                                        {
                                            Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", "sr.GetDirectoryEntry().Properties[\"mail\"] != null");

                                            if (sr.GetDirectoryEntry().Properties["mail"].Count > 0)
                                            {
                                                Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", "sr.GetDirectoryEntry().Properties[\"mail\"].Count > 0");

                                                oUser.EmailID = sr.GetDirectoryEntry().Properties["mail"].Value == null ? "" : sr.GetDirectoryEntry().Properties["mail"].Value.ToString();
                                            }
                                            else
                                            {
                                                oUser.EmailID = string.Empty;
                                            }
                                        }
                                        if (sr.GetDirectoryEntry().Properties["displayname"] != null)
                                        {
                                            if (sr.GetDirectoryEntry().Properties["displayname"].Count > 0)
                                            {
                                                oUser.DisplayName = sr.GetDirectoryEntry().Properties["displayname"].Value == null ? "" : sr.GetDirectoryEntry().Properties["displayname"].Value.ToString();
                                            }
                                            else
                                            {
                                                oUser.DisplayName = string.Empty;
                                            }
                                        }

                                        if (sr.GetDirectoryEntry().Properties["department"] != null)
                                        {
                                            if (sr.GetDirectoryEntry().Properties["department"].Count > 0)
                                            {
                                                oUser.Department = sr.GetDirectoryEntry().Properties["department"].Value == null ? "" : sr.GetDirectoryEntry().Properties["department"].Value.ToString();
                                            }
                                            else
                                            {
                                                oUser.Department = string.Empty;
                                            }
                                        }
                                        if (sr.GetDirectoryEntry().Properties["company"] != null)
                                        {
                                            if (sr.GetDirectoryEntry().Properties["company"].Count > 0)
                                            {
                                                oUser.Company = sr.GetDirectoryEntry().Properties["company"].Value == null ? "" : sr.GetDirectoryEntry().Properties["company"].Value.ToString();
                                            }
                                            else
                                            {
                                                oUser.Company = string.Empty;
                                            }
                                        }

                                        if (sr.GetDirectoryEntry().Properties["sAMAccountName"] != null)
                                        {
                                            if (sr.GetDirectoryEntry().Properties["sAMAccountName"].Count > 0)
                                            {
                                                oUser.UserID = sr.GetDirectoryEntry().Properties["sAMAccountName"].Value == null ? "" : sr.GetDirectoryEntry().Properties["sAMAccountName"].Value.ToString();
                                            }
                                            else
                                            {
                                                oUser.UserID = string.Empty;
                                            }
                                        }
                                        if (sr.GetDirectoryEntry().Properties["mobile"] != null)
                                        {
                                            if (sr.GetDirectoryEntry().Properties["mobile"].Count > 0)
                                            {
                                                oUser.Mobile = sr.GetDirectoryEntry().Properties["mobile"].Value == null ? "" : sr.GetDirectoryEntry().Properties["mobile"].Value.ToString();
                                            }
                                            else
                                            {
                                                oUser.Mobile = string.Empty;
                                            }
                                        }

                                        if (sr.GetDirectoryEntry().Properties["telephoneNumber"] != null)
                                        {
                                            if (sr.GetDirectoryEntry().Properties["telephoneNumber"].Count > 0)
                                            {
                                                oUser.TelephoneNumber = sr.GetDirectoryEntry().Properties["telephoneNumber"].Value == null ? "" : sr.GetDirectoryEntry().Properties["telephoneNumber"].Value.ToString();
                                            }
                                            else
                                            {
                                                oUser.TelephoneNumber = string.Empty;
                                            }
                                        }
                                        if (sr.GetDirectoryEntry().Properties["title"] != null)
                                        {
                                            if (sr.GetDirectoryEntry().Properties["title"].Count > 0)
                                            {
                                                oUser.Title = sr.GetDirectoryEntry().Properties["title"].Value == null ? "" : sr.GetDirectoryEntry().Properties["title"].Value.ToString();
                                            }
                                            else
                                            {
                                                oUser.Title = string.Empty;
                                            }
                                        }
                                        if (sr.GetDirectoryEntry().Properties["employeeID"] != null)
                                        {
                                            if (sr.GetDirectoryEntry().Properties["employeeID"].Count > 0)
                                            {
                                                oUser.EmployeeID = sr.GetDirectoryEntry().Properties["employeeID"].Value == null ? "" : sr.GetDirectoryEntry().Properties["employeeID"].Value.ToString();
                                            }
                                            else
                                            {
                                                oUser.EmployeeID = string.Empty;
                                            }
                                        }
                                        if (sr.GetDirectoryEntry().Properties["sn"] != null)
                                        {
                                            if (sr.GetDirectoryEntry().Properties["sn"].Count > 0)
                                            {
                                                oUser.LastName = sr.GetDirectoryEntry().Properties["sn"].Value == null ? "" : sr.GetDirectoryEntry().Properties["sn"].Value.ToString();
                                            }
                                            else
                                            {
                                                oUser.LastName = string.Empty;
                                            }
                                        }

                                        if (sr.GetDirectoryEntry().Properties["MiddleName"] != null)
                                        {
                                            if (sr.GetDirectoryEntry().Properties["MiddleName"].Count > 0)
                                            {
                                                oUser.MiddleName = sr.GetDirectoryEntry().Properties["MiddleName"].Value == null ? "" : sr.GetDirectoryEntry().Properties["MiddleName"].Value.ToString();
                                            }
                                            else
                                            {
                                                oUser.MiddleName = string.Empty;
                                            }
                                        }
                                        if (sr.GetDirectoryEntry().Properties["GivenName"] != null)
                                        {
                                            if (sr.GetDirectoryEntry().Properties["GivenName"].Count > 0)
                                            {
                                                oUser.FirstName = sr.GetDirectoryEntry().Properties["GivenName"].Value == null ? "" : sr.GetDirectoryEntry().Properties["GivenName"].Value.ToString();
                                            }
                                            else
                                            {
                                                oUser.FirstName = string.Empty;
                                            }
                                        }

                                        Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", "End if of String.Compare(sr.GetDirectoryEntry().SchemaClassName, \"user\", true) == 0");
                                    }
                                    IuserList.Add(oUser);
                                }
                                Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", $"Child  foreach {d} src completed");
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        }
                    }
                }

                Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", $"Parent foreach {d} completed");
            }

            Errlog.ErrorLogFile("AuthenticationTokenService", "private GetUserList", $"{IuserList.Count}");

            return IuserList;
        }

        private IEnumerable<string> GetDomains()
        {
            ICollection<string> ldomains = new List<string>();
            // Querying the current Forest for the domains within.
            foreach (Domain d in Forest.GetCurrentForest().Domains)
                ldomains.Add(d.Name);
            return ldomains;
        }
        private string GetDomainFullName(string friendlyName)
        {
            DirectoryContext context = new DirectoryContext(DirectoryContextType.Domain, friendlyName);
            Domain domain = Domain.GetDomain(context);
            return domain.Name;
        }
    }
}
