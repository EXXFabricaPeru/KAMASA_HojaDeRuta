using System;

namespace GeolocationAPI.Code
{
    public static class Error
    {
        public static Exception IndicatorNotDefined(string code)
            => new Exception(@$"[ERROR] El indicador de prioridad '{code}' no esta definido. Tabla: U_VS_PD_OINP");

        public static Exception CertificatesNotAssigned(string code)
            => new Exception(@$"[ERROR] El cliente '{code}' no tiene certificados asignados. Tabla: U_VS_PD_OACD");

        public static Exception SaleChannelNotAssigned(int number)
            => new Exception(@$"[ERROR] La orden de traslado '{number}' no tiene un canal de venta asignado. Tabla: @VS_PD_ORTR");

        public static Exception DispatchTimeNotAssigned(int number)
            => new Exception(@$"[ERROR] La orden de traslado '{number}' no tiene una ventana horaria de despacho asignada. Tabla: @VS_PD_ORTR");

        public static Exception DeliveryControlNotAssigned(int number)
            => new Exception(@$"[ERROR] La orden de traslado '{number}' no tiene una restricción de entrega asignada. Tabla: @VS_PD_ORTR");

        public static Exception SaleChannelNotReferencedDispatchTime(string code)
            => new Exception(@$"[ERROR] El canal de venta '{code}' no tiene tiempo de despacho referenciado. Tabla: U_VS_PD_OTPD");

        public static Exception BusinessPartnerNotReferencedShipAddress(string code)
            => new Exception(@$"[ERROR] El socio de negocio '{code}' no tiene dirección de entrega referenciada.");

        public static Exception VelocitiesNotDefined(string range)
            => new Exception(@$"[ERROR] No existe una velocidad definida en este rango de horas: '{range}'.");

        public static Exception ServiceLayerException(string url, string message)
            => new Exception(@$"[SERVICE LAYER] Error:'{message}'. URL: {url}");

        public static Exception ServiceLayerNotFoundException(string url)
            => new Exception(@$"[SERVICE LAYER] No existen registros coincidentes. URL: {url}");

        public static Exception ServiceLayerNotInsertRecordException(string url)
            => new Exception(@$"[SERVICE LAYER] No se ha podido realizar la inserción. URL: {url}");
    }
}