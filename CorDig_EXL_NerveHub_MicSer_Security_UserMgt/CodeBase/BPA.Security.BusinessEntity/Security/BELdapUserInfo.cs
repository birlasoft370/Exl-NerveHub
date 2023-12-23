/* Copyright © 2012 ExlService (I) Pvt. Ltd.
 * project Name                 :   
 * Class Name                   :   
 * Namespace                    :   
 * Purpose                      :
 * Description                  :
 * Dependency                   :   
 * Related Table                :
 * Related Class                :
 * Related StoreProcdure        :
 * Author                       :   
 * Created on                   :   
 * Reviewed on                  :          
 * Reviewed by                  :
 * Tested on                    :
 * Tested by                    :
 * Modification history         :
 * modify1 on                   :
 * modify1 By                   :
 * Overall effect               :
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace BPA.Security.BusinessEntity.Security
{
    /// <summary>
    /// Ldap User info 
    /// </summary>
    [Serializable]
    [DataContract]

    public class BELdapUserInfo : ObjectBase
    {
        private int _iCount;
        private string _UserName;
        private string _LoginName;
        private string _EmailID;
        private string _WorkPhone;
        private string _Mobile;
        private string _Department;
        private string _Company;
        private string _title;
        private string _UserID;
        private string _DisplayName;
        private string _telephoneNumber;
        private string _employeeID;
        private string _middleName;
        private string _lastName;
        private string _firstName;

        public BELdapUserInfo() { }
        public BELdapUserInfo(int iCount, string UserName, string LoginName, string EmailID)
        {
            _iCount = iCount;
            _UserName = UserName;
            _LoginName = LoginName;
            _EmailID = EmailID;
        }
        [DataMember]
        public int iCount
        {
            get { return _iCount; }
            set { _iCount = value; }
        }
        [DataMember]
        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value; }
        }
        [DataMember]
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        [DataMember]
        public string LoginName
        {
            get { return _LoginName; }
            set { _LoginName = value; }
        }
        [DataMember]
        public string EmailID
        {
            get { return _EmailID; }
            set { _EmailID = value; }
        }

        [DataMember]
        public string WorkPhone
        {
            get { return _WorkPhone; }
            set { _WorkPhone = value; }
        }
        public string Mobile
        {
            get { return _Mobile; }
            set { _Mobile = value; }
        }
        [DataMember]
        public string Department
        {
            get { return _Department; }
            set { _Department = value; }
        }
        [DataMember]
        public string Company
        {
            get { return _Company; }
            set { _Company = value; }
        }
        [DataMember]
        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }
        [DataMember]
        public string UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        [DataMember]
        public string TelephoneNumber
        {
            get { return _telephoneNumber; }
            set { _telephoneNumber = value; }
        }
        [DataMember]
        public string EmployeeID
        {
            get { return _employeeID; }
            set { _employeeID = value; }
        }
        [DataMember]
        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        [DataMember]
        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }
       

        [DataMember]
        public string MiddleName { get { return _middleName; } set { _middleName = value; } }

        [DataMember]
        public bool isSelected
        {
            get;
            set;
        }
    }
}
