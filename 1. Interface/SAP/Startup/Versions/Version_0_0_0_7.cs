﻿using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Code.Models;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.System.Header.Document;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Detail;
using Exxis.Addon.HojadeRutaAGuia.CrossCutting.Model.UDO.Header;
using Exxis.Addon.HojadeRutaAGuia.Interface.Resources.Query;

namespace Exxis.Addon.HojadeRutaAGuia.Interface.Startup.Versions
{
    // ReSharper disable once InconsistentNaming
    public class Version_0_0_0_7 : Versioner
    {
        public static Versioner Make => new Version_0_0_0_7();

        public Version_0_0_0_7() : base("0.0.0.7")
        {
        }

        protected override void InitializeTables()
        {
            CreateObject(typeof(OHRG));

            //var root = "Resources/Data/";

            //CurrentFlag.UDOs.Data.PutCabData<OPDS>(root + "Configuracion.csv");

        }
        protected override void InitializeFormattedSearch()
        {     
            
        }
    }
}