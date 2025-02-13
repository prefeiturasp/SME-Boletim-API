using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SERAp.Boletim.Infra.Dtos
{
    [MessagePackObject(keyAsPropertyName: true)]
    public abstract class DtoBase
    {
    }
}