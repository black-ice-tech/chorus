using System;
using System.Collections.Generic;
using System.Text;

namespace Chorus.CQRS
{
    public interface ICommand : ICorrelatable, IVersionable
    {
    }
}
