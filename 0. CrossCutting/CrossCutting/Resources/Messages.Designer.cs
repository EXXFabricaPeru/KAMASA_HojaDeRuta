﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Exxis.Addon.HojadeRutaAGuia.CrossCutting.Resources {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Exxis.Addon.HojadeRutaAGuia.CrossCutting.Resources.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [Error validación] La orden de traslado no supero el monto minimo para ser distribuida..
        /// </summary>
        public static string NotPassAmountValidation {
            get {
                return ResourceManager.GetString("NotPassAmountValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [Error validación] El socio de negocio de la orden de traslado no posee certificados de distribución..
        /// </summary>
        public static string NotPassCertificateValidation {
            get {
                return ResourceManager.GetString("NotPassCertificateValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [Error validación] La orden de traslado no posee la cantidad minima de entrega para ser distribuida.
        /// </summary>
        public static string NotPassQuantityValidation {
            get {
                return ResourceManager.GetString("NotPassQuantityValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [Error validación] La dirección no tiene asignada una restricción de entrega..
        /// </summary>
        public static string NotPassRestrictionValidation {
            get {
                return ResourceManager.GetString("NotPassRestrictionValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [Error validación] La orden de traslado no supero el peso minimo para ser distribuida..
        /// </summary>
        public static string NotPassWeightValidation {
            get {
                return ResourceManager.GetString("NotPassWeightValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [Error validación] La dirección no tiene asignada una ventana horaria.
        /// </summary>
        public static string NotPassWindowValidation {
            get {
                return ResourceManager.GetString("NotPassWindowValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a El cierre manual de recepción de documentos SAP se realizó de forma exitosa..
        /// </summary>
        public static string SucessfullClosingDocumentReception {
            get {
                return ResourceManager.GetString("SucessfullClosingDocumentReception", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [Error validación] El almacén de entrega no tiene una altitud asignada..
        /// </summary>
        public static string WarehouseNotPassLatitudeValidation {
            get {
                return ResourceManager.GetString("WarehouseNotPassLatitudeValidation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a [Error validación] El almacén de entrega no tiene una longitud asignada..
        /// </summary>
        public static string WarehouseNotPassLongitudeValidation {
            get {
                return ResourceManager.GetString("WarehouseNotPassLongitudeValidation", resourceCulture);
            }
        }
    }
}
