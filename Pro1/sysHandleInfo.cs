using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pro1
{
    public class SysHandleInfo
    {
        private string _HandlePerson;
        private string _HandleContent;
        private string _HandleRemark;
        public string HandlePerson
        {
            get { return _HandlePerson; }
            set { _HandlePerson = value; }
        }

        public string HandleContent
        {
            get { return _HandleContent; }
            set { _HandleContent = value; }
        }

        public string HandleRemark
        {
            get { return _HandleRemark; }
            set { _HandleRemark = value; }
        }




    }
}
