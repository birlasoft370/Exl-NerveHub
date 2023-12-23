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
    [Serializable]
    [DataContract]
    public class BESession : ObjectBase 
    {
        private int _iUserID;
        private int _iSessionID;
        private string _sSystemSessionID;
        private string _sIPAddress;
        private string _sHostName;

        public BESession() { }
        public BESession(int iUserID, int iSessionID, string sSystemSessionID, string sIPAddress, string sHostName)
        {
            _iUserID = iUserID;
            _iSessionID = iSessionID;
            _sSystemSessionID = sSystemSessionID;
            _sIPAddress = sIPAddress;
            _sHostName = sHostName;
        }
        [DataMember]
        public int iUserID
        {
            get { return _iUserID; }
            set { _iUserID = value ; }
        }
        [DataMember]
        public int iSessionID
        {
            get { return _iSessionID; }
            set { _iSessionID = value; }
        }
        [DataMember]
        public string sSystemSessionID
        {
            get { return _sSystemSessionID; }
            set { _sSystemSessionID = value; }
        }
        [DataMember]
        public string sIPAddress
        {
            get { return _sIPAddress; }
            set { _sIPAddress = value; }
        }
        [DataMember]
        public string  sHostName
        {
            get { return _sHostName; }
            set { _sHostName = value; }
        }
       
    }
}
