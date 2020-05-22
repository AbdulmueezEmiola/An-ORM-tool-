using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace SQLGenerator
{
    interface IDatabaseWrapper
    {
        void ExecuteCommand(string query);
        void ExecuteCommand(string query, Dictionary<string, string> values);
        DbDataReader ReadData(string query);
    }
}
