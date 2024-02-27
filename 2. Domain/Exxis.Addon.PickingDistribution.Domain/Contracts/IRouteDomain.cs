using System;
using System.Collections.Generic;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.ApiResponse;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.Domain.Code;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;

namespace Exxis.Addon.HojadeRutaAGuia.Domain.Contracts
{
    public interface IRouteDomain : IBaseDomain
    {
        Tuple<bool, string> Register(OARD route);

        OARD Retrieve(int documentEntry);

        IEnumerable<OARD> RetrieveInProgressRoutes();
        
        IEnumerable<OARD> RetrieveOpenedRoutesByDate(DateTime date);

        IEnumerable<OARD> SyncRoutesWithThirdPartySoftware(IEnumerable<OARD> routes);

        //Tuple<bool, string> SaveRoute(RouteBuildResponse route);
        Tuple<bool, string> SaveRoute(RouteBuildResponse route,DateTime distribuitionDate,ref List<string> msg );
        

        Tuple<bool, string> UpdateRoute(OARD route);

        Tuple<bool, string> RemoveDetail<T>(int routEntry) where T : BaseUDO;

        Tuple<bool, string> CloseRoute(int entry);

        Tuple<bool, string> LiquidateRoute(OARD route);

        Tuple<bool, string> TryRevertClosingRoute(OARD route);
        
        int RetrieveLastDailyCounter(DateTime date);

        Tuple<bool, string> AppendDetails(int entry, IEnumerable<ARD1> transferOrdersRelated, IEnumerable<ARD2> documentsRelated);

        Tuple<bool, string> ChangeToTransshipping(int entry, int transferOrdersRelatedIndex, int originTransshipping);

        IEnumerable<OARD> RetrieveRoutesbyDate(DateTime _startDate, DateTime _endDate);
    }
}
