using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AREA.Reaction
{
    public interface IAreaReaction
    {
        void Do(string token, string arg);
    }
}