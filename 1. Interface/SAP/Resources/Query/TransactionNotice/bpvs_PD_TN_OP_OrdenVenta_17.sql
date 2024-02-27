CREATE PROCEDURE "bpvs_PD_TN_OP_OrdenVenta_17"(
	IN Id int,
	IN Transaction_Type nchar(1),
	IN Object_Type nvarchar(20),
	OUT Mensaje_Error nvarchar(256)
)
AS
BEGIN

	--> Variable de retorno de mensaje de error
	--DECLARE Mensaje_Error	nvarchar(256);


	DECLARE ValDistribution	NCHAR(1);
	DECLARE SerieGuia	nvarchar(100);
	DECLARE SerieVenta	nvarchar(100);
	
	DECLARE cont			int;
	DECLARE linea			int;
	DECLARE NumAtCard		nvarchar(100);
	DECLARE TipPersona		nvarchar(3);
	DECLARE NroDocId		nvarchar(32);
	DECLARE TipDocId		nvarchar(1);
	DECLARE Serie			nvarchar(20);
	DECLARE Correlativo		nvarchar(20);
	DECLARE CardCode		nvarchar(20);
	DECLARE TipoDocumento	nvarchar(2);
	DECLARE Anulada			NVARCHAR(1);
	DECLARE U_BPV_SERI		nVarchar(10);
	DECLARE U_BPP_MDTD		nVarchar(20);
	DECLARE U_BPP_MDSD		nVarChar(5);
	DECLARE U_BPP_MDCD		nVarchar(20);
	Declare U_BPP_MDTO		nVarChar(2);
	Declare U_BPP_MDSO		nVarChar(5);
	Declare U_BPP_MDCO		nVarChar(20);
	DECLARE U_BPP_SDocDate 	DateTime;
	DECLARE U_VS_SDocTotal 	numeric(19,6);
	DECLARE U_VS_TDOCORG	INT;
	DECLARE U_VS_DOCORG		INT;
	DECLARE NombreSN		nvarchar(100);
	DECLARE ComentariosAsiento nvarchar(50);
	DECLARE Tipomoneda		nvarchar(1);
	DECLARE UsarReservado	nVarchar(1);
	DECLARE Monedacabecera 	nvarchar(3);
	DECLARE Monedalineas	nvarchar(3);
	DECLARE DocType			nvarchar(1);
	DECLARE DocSubType		nvarchar(3);
	DECLARE E_Serie			char(1);
	DECLARE FE_Estado		char(1);
	DECLARE Cancelado		NCHAR(1);
	DECLARE Motivo			NVARCHAR(30);
	DECLARE Sustento		VARCHAR(100);
	DECLARE E_Mail			nvarchar(100);
	DECLARE U_VS_NOMCOM		NVARCHAR(50);
	DECLARE U_VS_GRATEN		nVarChar(1);
	DECLARE Ubigeo			INT;
	DECLARE Descuento		INT;
	--> Variables de validación
	DECLARE Canal			NVARCHAR(3);
	DECLARE Activado		CHAR(1);
	DECLARE Mensaje			NVARCHAR(250);
	DECLARE DataSource		char(1);
	DECLARE V1				NVARCHAR(15);
	-->Detracciones
	DECLARE AfectDetracc	NCHAR(1);
	DECLARE PorcenDetracc	DECIMAL(19,6);
	DECLARE PorcMaxDetracc	DECIMAL(19,6);
	DECLARE CantCuotas		INT;
	DECLARE Mpago			NVARCHAR(3);
	DECLARE TipoFactura		NVARCHAR(5);
	DECLARE U_VS_MTRCL	 	varchar(15);
	DECLARE U_VS_NMEMB		varchar(50);
	DECLARE U_VS_DESESPC	varchar(100);	
	DECLARE U_VS_LGDESC	 	varchar(200);
	DECLARE U_VS_FECDESC	date;
	DECLARE U_VS_CNESPC	 	INT;
	
	DECLARE U_CL_CANAL			NVARCHAR(3);
	
	DECLARE GroupNumPago	NVARCHAR(10);
	DECLARE FormaPago	varchar(200);
	DECLARE PagoContado	varchar(2);
	
	DECLARE BASETYPE		NVARCHAR(3);
	
	-->Anticipo
	DECLARE AplicaAnticipo	NCHAR(1);
	-->Aux
	DECLARE var1			INT;
	DECLARE var2			INT;
	DECLARE filasAnticipos	INT;
	-->Portal
	DECLARE U_VS_SERSUNAT	NCHAR(1);
	DECLARE U_VS_PORTSUNAT	NCHAR(1);
	DECLARE anexos			INT;
	DECLARE MYCOND CONDITION;	
	DECLARE EXIT HANDLER FOR MYCOND BEGIN END;
	
	DECLARE EXIT HANDLER FOR SQL_ERROR_CODE 1299
	SELECT ::SQL_ERROR_CODE , '' into Cont, Mensaje_Error FROM DUMMY;
	
	--> Asignación de valor  		
	IF LOCATE('|A|U|','|' || :Transaction_Type || '|')>0
	THEN	
		SELECT
				'Y',
				0,
				T0."CardCode",
				IFNULL(T0."NumAtCard",''),
				RIGHT(IFNULL(T0."U_BPV_SERI",''),2),
				LEFT(IFNULL(T0."U_BPV_SERI",''),
				CASE WHEN LOCATE (IFNULL(T0."U_BPV_SERI",''),'-') = 0 THEN 0 ELSE LOCATE (IFNULL(T0."U_BPV_SERI",''),'-')-1 END),
				IFNULL(T0."U_BPV_NCON2",''),
				REPLACE(LEFT(IFNULL(T0."CardName",''),50),'&',''),
				REPLACE(IFNULL(T0."JrnlMemo",''),'&',''),
				IFNULL(T0."CurSource",''),
				IFNULL(T0."U_OK1_Anulada",'N'),
				IFNULL(T0."LicTradNum",''),
				IFNULL(T0."U_BPP_MDCD",''),
				IFNULL(T0."U_BPP_MDTD",''),
				IFNULL(T0."U_BPP_MDSD",''),
				IFNULL(T0."U_BPV_SERI",''),
				IFNULL(T0."U_BPP_MDTO",''),
				IFNULL(T0."U_BPP_MDCO",''),
				IFNULL(T0."U_BPP_MDSO",''),
				IFNULL(T0."U_BPP_SDOCDATE",'1'),
				IFNULL(T0."U_VS_SDOCTOTAL",0),
				IFNULL(T0."U_VS_TDOCORG",'13'),
				IFNULL(T0."U_VS_DOCORG",0),
				IFNULL(T0."DocCur",''),
				T0."DocType",
				IFNULL(T0."U_VS_USRSV",''),
				T0."DocSubType",
				T0."DataSource",
				IFNULL(T0."U_VS_AFEDET",'N'),
				IFNULL(T0."U_VS_PORDET",0),
				T0."Installmnt",
				IFNULL(T0."U_VS_APLANT",'N'),
				'',
				IFNULL(T0."U_VS_FESTAT",''),
				IFNULL(T1."U_BPP_BPTD",''),
				IFNULL(T1."U_BPP_BPTP",''),
				T0."CANCELED",
				IFNULL(T0."U_VS_MOTEMI",''),
				IFNULL(T0."U_VS_SUSTNT",''),
				IFNULL(T1."E_Mail",''),
				IFNULL(T1."U_VS_NOMCOM",''),
				IFNULL(T0."U_VS_GRATEN",''),
				'',--IFNULL(T2."U_VS_SERSUNAT",''),
				IFNULL(T0."U_VS_PORTSUNAT",''),
				IFNULL(T0."U_VS_MPAGO",''),
				IFNULL(T0."U_VS_TIPO_FACT",''),
				IFNULL(T0."U_VS_MTRCL",''),	
				IFNULL(T0."U_VS_NMEMB",''),	
				IFNULL(T0."U_VS_DESESPC",''),
				IFNULL(T0."U_VS_LGDESC",''),
				IFNULL(T0."U_VS_FECDESC",''),
				IFNULL(T0."U_VS_CNESPC",0),
				IFNULL(T0."U_EXK_VALD",''),
				IFNULL(T0."U_EXK_SRGR",''),
				IFNULL(T0."U_EXK_SRVT",''),
				IFNULL(T0."U_CL_CANAL",''),
				IFNULL(T0."GroupNum",-1),
				IFNULL(T0."U_EXK_PGCT",'')
				
			INTO
				Activado			
				,linea 				
				,CardCode  			
				,NumAtCard 			
				,TipoDocumento 		
				,Serie 				
				,Correlativo 		
				,NombreSN 			
				,ComentariosAsiento 	
				,Tipomoneda 			
				,Anulada 			
				,NroDocId 			
				,U_BPP_MDCD 			
				,U_BPP_MDTD 			
				,U_BPP_MDSD 			
				,U_BPV_SERI 			
				,U_BPP_MDTO 			
				,U_BPP_MDCO 			
				,U_BPP_MDSO 			
				,U_BPP_SDocDate 		
				,U_VS_SDocTotal 		
				,U_VS_TDOCORG		
				,U_VS_DOCORG			
				,Monedacabecera 		
				,DocType 			
				,UsarReservado 		
				,DocSubType 			
				,DataSource			
				,AfectDetracc		
				,PorcenDetracc		
				,CantCuotas			
				,AplicaAnticipo		
				,E_Serie				
				,FE_Estado			
				,TipDocId			
				,TipPersona			
				,Cancelado			
				,Motivo				
				,Sustento			
				,E_Mail				
				,U_VS_NOMCOM			
				,U_VS_GRATEN
				,U_VS_SERSUNAT
				,U_VS_PORTSUNAT
				,Mpago
				,TipoFactura
				,U_VS_MTRCL	 
				,U_VS_NMEMB	
				,U_VS_DESESPC
				,U_VS_LGDESC	 
				,U_VS_FECDESC
				,U_VS_CNESPC
				,ValDistribution
				,SerieGuia
				,SerieVenta
				,U_CL_CANAL
				,GroupNumPago
				,PagoContado

			
		FROM "ORDR" T0 INNER JOIN "OCRD" T1 ON T0."CardCode" = T1."CardCode"
		WHERE T0."DocEntry" = :Id;
					
		--> Canal
		SELECT CASE :DataSource
				WHEN 'I' THEN 'SB1' 
				WHEN 'N' THEN 'SB1' 
				WHEN 'O' THEN 'SDK' 
			   ELSE 'NNN' END
		INTO Canal
		FROM DUMMY;
		
		 select UPPER("PymntGroup") into FormaPago from "OCTG"  where "GroupNum"=GroupNumPago;
	
--> VALIDACIONES		
----------------------------------------------------------------------------------------------------------------------------------------------------------		
			/*Mensaje_Error := :Canal;
							SIGNAL MYCOND;	*/
			if (:PagoContado<>'A' and :PagoContado<>'D' )
			THEN
			if(:FormaPago like '%CONTADO%')
					THEN
					update "ORDR"
								set "U_EXK_PGCT"='Y'
							where "DocEntry"=:Id;				
					else
						update "ORDR"
							set "U_EXK_PGCT"='N'
							where "DocEntry"=:Id;
					
					END IF;
			
			END IF;
			
					
					
					IF (:ValDistribution='Y')
					THEN
						IF (:Transaction_Type='A')
						THEN
							update "ORDR"
							set "U_EXK_OTRF"=null,"U_EXK_HDST"=null,"U_EXK_VRDS"=null
							,"U_EXK_VMDS"=null,"U_EXK_VSDS"='NV',"U_EXK_VPTO"='NV'
							,"U_EXK_VPTR"=null,"U_EXK_VPTC"=null,"U_EXK_STDS"=null
							where "DocEntry"=:Id;
						END IF;
					
					
						IF (:SerieGuia='')
						THEN
							Mensaje_Error := 'Debe colocar la serie de guía para el flujo de distribución';
							SIGNAL MYCOND;	
						END IF;	
						
						IF (:U_CL_CANAL='' or  :U_CL_CANAL='0')
						THEN
							Mensaje_Error := 'Debe colocar el canal de venta para el flujo de distribución';
							SIGNAL MYCOND;	
						END IF;
						/*IF (:SerieVenta='')
						THEN
							Mensaje_Error := :Mensaje;
							SIGNAL MYCOND;	
						END IF;					
				*/
					END IF;
			
				
----------------------------------------------------------------------------------------------------------------------------------------------------------




					
			
	END IF;

END;