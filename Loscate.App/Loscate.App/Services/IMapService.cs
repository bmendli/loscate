using Loscate.DTO.Map;
using System;
using System.Collections.Generic;
using System.Text;
using Loscate.App.Map;

namespace Loscate.App.Services
{
    public interface IMapService
    {
        void OnPinClickSubscribe(Action<CustomPin> action);
        void OnPinClickUnSubscribe(Action<CustomPin> action);
    }
}
