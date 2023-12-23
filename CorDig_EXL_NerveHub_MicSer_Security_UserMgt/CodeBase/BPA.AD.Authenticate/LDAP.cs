using BPA.Security.BusinessEntity.ExtrernalRefre.Uitility;
using BPA.Security.BusinessEntity.Security;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BPA.AD.Authenticate
{
    public enum SearchByEnum
    {
        EmailID,
        UserName,
        UserID
    }
    /// <summary>
    /// This class performs user authentication against Active Directory and
    /// Novell Edirectory.
    /// </summary>
    public class LDAPUser : IAuthenticate, IDisposable
    {

        /// <summary>
        /// string specifying user name
        /// </summary>
        private string _strUser;

        /// <summary>
        /// string specifying user password
        /// </summary>
        private string _strPasPhrase;

        /// <summary>
        /// string specifying user domain
        /// </summary>
        private string _strDomain;




        /// <summary>
        /// default constructor
        /// </summary>
        public LDAPUser(string Domain)
        {
            _strDomain = Domain;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LDAPUser"/> class.
        /// </summary>
        /// <param name="UserName">Name of the user.</param>
        /// <param name="Password">The password.</param>
        /// <param name="Domain">The domain.</param>
        public LDAPUser(string UserName, string Password, string Domain)
        {
            _strUser = UserName;
            _strPasPhrase = Password;
            _strDomain = Domain;
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        public void Dispose()
        {
            _strUser = null;
            _strPasPhrase = null;
            _strDomain = null;
        }

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        public string domain
        {
            get { return _strDomain; }
            set { _strDomain = "LDAP://" + value; }
        }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName
        {
            get { return _strUser; }
            set { _strUser = value; }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string PasPhrase
        {
            get { return _strPasPhrase; }
            set { _strPasPhrase = value; }
        }



        #region Authenticate
        /// <summary>
        /// function that performs login task
        /// and welcomes user if they are verified
        /// </summary>
        /// <returns></returns>
        public bool Authenticate()
        {
            bool isAuthenticate = false;
            foreach (string d in GetDomains())
            {
                using (System.DirectoryServices.DirectoryEntry deDirEntry = new System.DirectoryServices.DirectoryEntry("LDAP://" + GetDomainFullName(d), this._strUser, this._strPasPhrase, AuthenticationTypes.Secure))
                {
                    //using (DirectoryEntry deDirEntry = new DirectoryEntry(_strDomain,
                    //                                                     this._strUser,
                    //                                                     this._strPass,
                    //                                                     AuthenticationTypes.Secure))
                    //{
                    try
                    {
                        using (DirectorySearcher search = new DirectorySearcher(deDirEntry))
                        {
                            search.SizeLimit = 10;

                            SearchResult result;

                            result = search.FindOne();

                            search.Filter = "(SAMAccountName=" + ValidationRegex.whitelist(this._strUser) + ")";

                            //search.PropertiesToLoad.Add("mail");  
                            result = search.FindOne();
                            isAuthenticate = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;

                    }

                }
            }
            return isAuthenticate;
        }

        #region GetUserList
        /// <summary>
        /// Gets the user list.
        /// </summary>
        /// <param name="LoginName">Name of the login.</param>
        /// <param name="Domain"></param>
        /// <returns>List of All the Active User</returns>
        public IList<BELdapUserInfo> GetUserList(string LoginName, SearchByEnum searchby)
        {
            IList<BELdapUserInfo> IuserList = new List<BELdapUserInfo>();
            foreach (string d in GetDomains())
            {

                using (System.DirectoryServices.DirectoryEntry deDirEntry = new System.DirectoryServices.DirectoryEntry("LDAP://" + GetDomainFullName(d)))
                {
                    using (DirectorySearcher ds = new DirectorySearcher(deDirEntry))
                    {

                        //ds.Filter = FormFilter("name", LoginName + "*");		// get the LDAP filter string based on selections on the form

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

                        // start searching
                        using (SearchResultCollection src = ds.FindAll())
                        {
                            try
                            {
                                foreach (SearchResult sr in src)
                                {
                                    BELdapUserInfo oUser = new BELdapUserInfo();
                                    if (String.Compare(sr.GetDirectoryEntry().SchemaClassName, "user", true) == 0)
                                    {
                                        if (sr.GetDirectoryEntry().Properties["SAMAccountName"] != null)
                                        {
                                            if (sr.GetDirectoryEntry().Properties["SAMAccountName"].Count > 0)
                                            {
                                                oUser.UserName = sr.GetDirectoryEntry().Properties["SAMAccountName"].Value.ToString() + " - " + sr.GetDirectoryEntry().Name.Substring(3);
                                            }
                                            else
                                            {
                                                oUser.UserName = string.Empty;
                                            }
                                        }
                                        if (sr.GetDirectoryEntry().Properties["mail"] != null)
                                        {
                                            if (sr.GetDirectoryEntry().Properties["mail"].Count > 0)
                                            {
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


                                    }
                                    IuserList.Add(oUser);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
            return IuserList;
        }
        #endregion

        // filter string for the search in LDAP format
        //private string FormFilter(string objectCategory, string filter)
        //{
        //    String result;
        //    result = String.Format("(&(objectCategory={0})(name={1}))", objectCategory, filter);
        //    return result;
        //}
        #endregion

        #region TimeLeft for Password Expire

        /// <summary>
        /// Checks the password expire.
        /// </summary>
        /// <param name="strLoginName">Name of the STR login.</param>
        /// <param name="output">The output.</param>
        /// <returns></returns>
        public string CheckPasswordExpire(string strLoginName, out string output, out int DaysLeft)
        {
            string returnValue = "";
            output = "";

            System.DirectoryServices.DirectoryEntry entry = new System.DirectoryServices.DirectoryEntry(_strDomain);
            try
            {
                entry.AuthenticationType = AuthenticationTypes.Secure;
                using (DirectorySearcher search = new DirectorySearcher(entry))
                {
                    search.Filter = "(SAMAccountName=" + ValidationRegex.whitelist(strLoginName) + ")";
                    search.SearchScope = SearchScope.Subtree;
                    search.CacheResults = false;

                    using (SearchResultCollection results = search.FindAll())
                    {
                        if (results.Count > 0)
                        {
                            foreach (SearchResult result in results)
                            {
                                try
                                {
                                    entry = result.GetDirectoryEntry();
                                }
                                catch
                                {
                                    output += "<BR><FONT COLOR='RED'><b>Login ID not found !</b></FONT>";
                                }
                            }
                        }
                        TimeSpan tSpan = GetTimeLeft(entry);
                        DaysLeft = tSpan.Days;
                        if (tSpan == TimeSpan.MaxValue)
                            returnValue += "Your password will never expire !";
                        else if (tSpan.Days == 1)
                            returnValue += "Your password will expire after " + tSpan.Days.ToString() + " day ! ";
                        else
                            returnValue += "Your password will expire after " + tSpan.Days.ToString() + " days ! ";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                entry = null;
            }
            return returnValue;
        }
        /// <summary>
        /// Gets the int64.
        /// </summary>
        /// <param name="entry">The entry.</param>
        /// <param name="attr">The attr.</param>
        /// <returns></returns>
        private Int64 GetInt64(System.DirectoryServices.DirectoryEntry entry, string attr)
        {
            //we will use the marshaling behavior of
            //the searcher
            using (DirectorySearcher ds = new DirectorySearcher(
              entry,
              String.Format("({0}=*)", attr),
              new string[] { attr },
              SearchScope.Base
              ))
            {
                SearchResult sr = ds.FindOne();
                if (sr != null)
                {
                    if (sr.Properties.Contains(attr))
                    {
                        return (Int64)sr.Properties[attr][0];
                    }
                }

            }
            return -1;
        }
        /// <summary>
        /// Gets the time left.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public TimeSpan GetTimeLeft(System.DirectoryServices.DirectoryEntry user)
        {
            DateTime willExpire = GetExpiration(user);
            if (willExpire == DateTime.MaxValue)
                return TimeSpan.MaxValue;

            if (willExpire == DateTime.MinValue || willExpire.AddDays(30) < DateTime.Now)
                return TimeSpan.MinValue;

            return willExpire.AddDays(30).Subtract(DateTime.Now);

        }

        /// <summary>
        /// Gets the expiration.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public DateTime GetExpiration(System.DirectoryServices.DirectoryEntry user)
        {
            const int UF_DONT_EXPIRE_P = 0x10000;

            int flags =
              (int)user.Properties["userAccountControl"][0];

            //check to see if password is set to expire
            if (Convert.ToBoolean(flags & UF_DONT_EXPIRE_P))
            {
                //the user’s password will never expire
                return DateTime.MaxValue;
            }

            long ticks = GetInt64(user, "pwdLastSet");

            //user must change password at next login
            if (ticks == 0)
                return DateTime.MinValue;

            //password has never been set
            if (ticks == -1)
            {
                throw new InvalidOperationException(
                  "User does not have a password"
                  );
            }

            //get when the user last set their password;
            DateTime pwdLastSet = DateTime.FromFileTime(
              ticks
              );

            //use our policy class to determine when
            //it will expire
            return pwdLastSet;
        }
        #endregion

        #region Change Password
        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="sLoginName">Name of the s login.</param>
        /// <param name="sCurrentPassword">The s current password.</param>
        /// <param name="sNewPass">The s new pass.</param>
        /// <returns></returns>
        public bool ChangePd(string sLoginName, string sCurrentPd, string sNewPass)
        {
            bool returnvalue = false;
            System.DirectoryServices.DirectoryEntry entry = new System.DirectoryServices.DirectoryEntry(_strDomain);
            try
            {
                entry.AuthenticationType = AuthenticationTypes.Secure;
                using (DirectorySearcher search = new DirectorySearcher(entry))
                {
                    search.Filter = "(SAMAccountName=" + ValidationRegex.whitelist(sLoginName) + ")";
                    search.SearchScope = SearchScope.Subtree;
                    search.CacheResults = false;

                    using (SearchResultCollection results = search.FindAll())
                    {
                        if (results.Count > 0)
                        {
                            foreach (SearchResult result in results)
                            {
                                try
                                {
                                    entry = result.GetDirectoryEntry();
                                }
                                catch
                                {
                                    throw new Exception(@"Login ID not found");
                                }
                            }

                            try
                            {
                                entry.Invoke("ChangePassword", new object[] { Security.BusinessEntity.ExtrernalRefre.Utility.EncryptDecrypt.Decrypt(sCurrentPd), Security.BusinessEntity.ExtrernalRefre.Utility.EncryptDecrypt.Decrypt(sNewPass) });
                                entry.CommitChanges();
                                returnvalue = true;
                            }
                            catch (Exception error)
                            {
                                throw new Exception("Invalid current password or check Password Policy");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (entry != null)
                {
                    entry = null;
                }
            }

            return returnvalue;
        }
        #endregion

        #region GetUsersInGroup
        /// <summary>
        /// Gets the users in group.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="group">The group.</param>
        /// <returns></returns>
        public IList<BELdapUserInfo> GetUsersInGroup(string group)
        {
            IList<BELdapUserInfo> groupMemebers = new List<BELdapUserInfo>();
            string sam = "";
            string fname = "";
            string lname = "";
            string active = "";
            string email = "";
            foreach (string d in GetDomains())
            {
                using (System.DirectoryServices.DirectoryEntry domain = new System.DirectoryServices.DirectoryEntry(GetDomainFullName(d)))
                {
                    using (DirectorySearcher ds = new DirectorySearcher(domain, "(objectClass=person)"))
                    {

                        ds.Filter = "(&(memberOf=CN=" + ValidationRegex.whitelist(group) + ",OU=Groups&DL,OU=India,DC=corp,DC=exlservice,DC=com))";

                        ds.PropertiesToLoad.Add("givenname");
                        ds.PropertiesToLoad.Add("samaccountname");
                        ds.PropertiesToLoad.Add("sn");

                        ds.PropertiesToLoad.Add("useraccountcontrol");
                        int counter = 1;
                        foreach (SearchResult sr in ds.FindAll())
                        {
                            BELdapUserInfo oUser = new BELdapUserInfo();
                            try
                            {
                                sam = sr.Properties["samaccountname"][0].ToString();
                                fname = sr.Properties["givenname"][0].ToString();
                                lname = sr.Properties["sn"][0].ToString();
                                active = sr.Properties["useraccountcontrol"][0].ToString();
                                email = sr.Properties["CN"][0].ToString();
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            if (active.ToString() != "514")
                            {
                                oUser.iCount = counter;
                                oUser.LoginName = sam.ToString();
                                oUser.UserName = fname.ToString() + " " + lname.ToString();
                                oUser.EmailID = email;
                                groupMemebers.Add(oUser);
                                oUser = null;
                                counter += 1;
                            }
                        }
                    }
                }
            }
            return groupMemebers;
        }
        #endregion

        #region GetUsersGroup
        /// <summary>
        /// Gets the users group.
        /// </summary>
        /// <param name="domain">The domain.</param>
        /// <param name="UserName">Name of the user.</param>
        /// <returns></returns>
        public IList<BELdapUserInfo> GetUsersGroup(string UserName)
        {
            IList<BELdapUserInfo> groupMemebers = new List<BELdapUserInfo>();


            string sam = "";
            string fname = "";
            string lname = "";
            string active = "";
            string email = "";
            using (System.DirectoryServices.DirectoryEntry de = new System.DirectoryServices.DirectoryEntry(_strDomain))
            {
                using (DirectorySearcher ds = new DirectorySearcher(de))
                {
                    ds.Filter = "(&(objectClass=user)(anr=" + ValidationRegex.whitelist(UserName) + "))";

                    //ds.PropertiesToLoad.Add("givenname");
                    //ds.PropertiesToLoad.Add("samaccountname");
                    //ds.PropertiesToLoad.Add("sn");

                    //ds.PropertiesToLoad.Add("useraccountcontrol");
                    int counter = 1;
                    int counterGroup = 1;

                    foreach (SearchResult sr in ds.FindAll())
                    {
                        BELdapUserInfo oUser = new BELdapUserInfo();
                        try
                        {
                            sam = sr.Properties["samaccountname"][0].ToString();
                            fname = sr.Properties["givenname"][0].ToString();
                            lname = sr.Properties["sn"][0].ToString();
                            active = sr.Properties["useraccountcontrol"][0].ToString();
                            email = sr.Properties["CN"][0].ToString();
                            if (active.ToString() != "514")
                            {
                                oUser.iCount = counter;
                                oUser.LoginName = sam.ToString();
                                oUser.UserName = fname.ToString() + " " + lname.ToString();
                                if (oUser != null)
                                {
                                    groupMemebers.Add(oUser);
                                }
                                oUser = null;
                                counter += 1;
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        counterGroup = 1;
                        foreach (string propertyKey in sr.Properties.PropertyNames)
                        {
                            // Retrieve the value assigned to that property name 
                            // in the ResultPropertyValueCollection.
                            ResultPropertyValueCollection valueCollection = sr.Properties[propertyKey];

                            // Iterate through values for each property name in each 
                            // SearchResult.
                            foreach (Object propertyValue in valueCollection)
                            {
                                if (propertyKey.ToString() == "memberof")
                                {
                                    oUser = new BELdapUserInfo();
                                    oUser.iCount = counterGroup;
                                    oUser.UserName = propertyValue.ToString().Substring(3, propertyValue.ToString().IndexOf(",", 0) - 3);
                                    groupMemebers.Add(oUser);
                                    counterGroup += 1;
                                }
                            }
                        }
                    }
                }
            }
            return groupMemebers;
        }
        #endregion

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
        #region Change Password
        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="sLoginName">Name of the s login.</param>
        /// <param name="sCurrentPassword">The s current password.</param>
        /// <param name="sNewPass">The s new pass.</param>
        /// <returns></returns>
        public bool ChangePassword(string sLoginName, string sCurrentPassword, string sNewPass, out string ErrorMsg)
        {
            ErrorMsg = string.Empty;
            bool returnvalue = false;
            System.DirectoryServices.DirectoryEntry entry = new System.DirectoryServices.DirectoryEntry(_strDomain);
            try
            {
                entry.AuthenticationType = AuthenticationTypes.Secure;
                using (DirectorySearcher search = new DirectorySearcher(entry))
                {
                    search.Filter = "(SAMAccountName=" + ValidationRegex.whitelist(sLoginName) + ")";
                    search.SearchScope = SearchScope.Subtree;
                    search.CacheResults = false;

                    using (SearchResultCollection results = search.FindAll())
                    {
                        if (results.Count > 0)
                        {
                            foreach (SearchResult result in results)
                            {
                                try
                                {
                                    entry = result.GetDirectoryEntry();
                                }
                                catch
                                {
                                    throw new Exception(@"Login ID not found");
                                }
                            }

                            try
                            {
                                entry.Invoke("ChangePassword", new object[] { Security.BusinessEntity.ExtrernalRefre.Utility.EncryptDecrypt.Decrypt(sCurrentPassword), Security.BusinessEntity.ExtrernalRefre.Utility.EncryptDecrypt.Decrypt(sNewPass) });
                                entry.CommitChanges();
                                returnvalue = true;
                            }
                            catch (Exception error)
                            {
                                if (error.InnerException != null)
                                {
                                    ErrorMsg = "Invalid password, the password does not meet the EXL password policy. Please try with the new password !";

                                }
                                else
                                {
                                    ErrorMsg = "Invalid password, the password does not meet the EXL password policy. Please try with the new password !";

                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //throw new Exception("Invalid current password or check Password Policy");
                ErrorMsg = "Invalid password, the password does not meet the EXL password policy. Please try with the new password !";
            }
            finally
            {
                if (entry != null)
                {
                    entry = null;
                }
            }

            return returnvalue;
        }
        #endregion
    }



}
