using Chaithit_Market.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chaithit_Market.Core
{
    public class LogManager
    {
        private static LogModel _log = null;

        public static LogModel ServiceLog
        {
            get
            {
                if (_log == null)
                {
                    _log = new LogModel();
                }

                return _log;
            }
        }
    }
}