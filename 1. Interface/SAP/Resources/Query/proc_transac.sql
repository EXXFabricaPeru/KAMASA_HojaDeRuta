
if (:object_type = '15'  AND (:transaction_type = 'A' OR :transaction_type = 'U') and IFNULL(:error,0)=0) 
THEN

		select "U_EXX_HOAS_STAD" INTO estadoSUNAT 	FROM ODLN T0 
										where T0."DocEntry"=:list_of_cols_val_tab_del ;
		 IF(:estadoSUNAT='A')THEN
		 	error := 1000;
			error_message := N'No puede crear la guía con el campo Estado Envío Sunat: Sin Asignar';
		END IF;							


END IF;

ALTER PROCEDURE "SP_EXXIS_FE_FOLIOS"
(
	IN DocEntry NVARCHAR(255),
	IN ObjectType NVARCHAR(20),
	IN DocSubType NVARCHAR(20),
	IN CodigoSunat NVARCHAR(20),
	IN BranchId int
)

-- =============================================
-- Autor					: RENZO PIZARRO
-- Fecha Creación			: 06/02/2019
-- Fecha de modificación 	: 30/04/2019
-- Nombre SP				: "SP_EXXIS_FE_FOLIOS"
-- Objetos involucrados		: 13,14,46,203
-- Descripción				: Asigna folios a documentos y gatilla acción
-- Modificación				: Soporte indicador Guías
-- =============================================
LANGUAGE SQLSCRIPT AS
BEGIN

DECLARE BD VARCHAR(100);
DECLARE BDFEX VARCHAR(100);
DECLARE Code NVARCHAR(50);
DECLARE SeriesName NVARCHAR(20);
DECLARE Series INTEGER;
DECLARE Folio INTEGER;
DECLARE DocNum INTEGER;
DECLARE FolioPref NVARCHAR(4);--revisar largo de campo de la versión 882
DECLARE IdFexCompany int;
DECLARE Estado NVARCHAR(3);
DECLARE Indicador NVARCHAR(10);
DECLARE RUC NVARCHAR(11);
DECLARE Contingencia NVARCHAR(1);
DECLARE FolioContingencia INTEGER;
DECLARE TipoDocContingencia NVARCHAR(3);
DECLARE EstadoEnvioSUNAT NVARCHAR(3);

--- Folio desde servicios
Declare Folia int; -- 1 (si) 0(no)

-- Asignación de Variables:
SELECT CURRENT_SCHEMA INTO BD FROM DUMMY;
SELECT TOP 1 "U_FE_dbFexName" INTO BDFEX FROM "@EXX_FE_BASE";
SELECT IFNULL(LEFT("TaxIdNum",11),'0') INTO RUC FROM OADM;
Series := 0;
Indicador := '';
DocNum := 0;
Estado := '-';
FolioPref := '';
Folio := 0;
Code :='';
SeriesName :='';
Contingencia :='N';
TipoDocContingencia := '';
EstadoEnvioSUNAT:='';

--Set Parametros para Multicompany
SELECT 
IFNULL(
(SELECT "IdFexCompany" FROM "FEX_PE".FEX_COMPANY WHERE "dbName"=:BD AND IFNULL("BranchID",-1) = IFNULL(:BranchId,-1))
,0) INTO IdFexCompany FROM DUMMY;


--- folio desde servicios
	Folia := 0;
	--Set Parametros para Multicompany
	SELECT 
	IFNULL(
	(Select "Valor" from FEX_PE.FEX_VARIABLES where "IdFexCompany" = :IdFexCompany and "Codigo" = 'ASIGNA_FOLIOS')
	,0) INTO Folia FROM DUMMY;
	
	
--FACTURAS, BOLETAS, FACTURAS DE RESERVA y NOTAS DE DEBITO        
IF (:ObjectType = '13') THEN
	SELECT "Series", "Indicator", "DocNum",IFNULL("U_EXX_FE_Estado",'-'), "U_EXX_FE_FOLIOCONT"
	INTO Series,Indicador,DocNum,Estado, FolioContingencia
	FROM OINV WHERE "DocEntry"= :DocEntry;

	SELECT "FolioPref",LEFT("SeriesName",4), IFNULL("U_FE_Contingencia", 'N'), "U_FE_TipoDocumento"
	INTO FolioPref,SeriesName, Contingencia, TipoDocContingencia
	FROM NNM1 WHERE "Series"=:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';
	
	SELECT IFNULL((SELECT TOP 1("Code") FROM "@EXX_FE_FOLIO_LIB" WHERE "U_FE_Series"=:Series),0)
	 INTO Code FROM DUMMY;

	IF (:Code=0) THEN
		IF (:Contingencia = 'Y') THEN
			Folio := :FolioContingencia;
		ELSE
			SELECT IFNULL("NextFolio", 1) INTO Folio
			FROM NNM1 WHERE "Series"=:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';
		END IF;	
	ELSE 
		SELECT "U_FE_FolioNum" INTO Folio FROM "@EXX_FE_FOLIO_LIB" WHERE "Code"=:Code;
		DELETE FROM "@EXX_FE_FOLIO_LIB" WHERE "Code"=:Code;
	END IF;	

	IF (:Estado) !='-' THEN
		UPDATE OINV
		SET "U_EXX_FE_Estado" = 'NEN'
		WHERE "DocEntry"=:DocEntry;
	END IF;
	
	UPDATE OINV
	SET
		"FolioNum"=:Folio,
		"FolioPref"=:FolioPref,
		"Printed"='Y',
		"U_EXX_FE_TIPCOM"=:CodigoSunat,
		"U_EXX_FE_ClaAcc"= :RUC||'-'||:Indicador||'-'||:SeriesName||'-'||CAST(:Folio AS NVARCHAR)
	WHERE
		"DocEntry"= :DocEntry;

	--FACTURA Y BOLETA DE ANTICIPO
ELSEIF (:ObjectType = '203') THEN

	SELECT "Series", "Indicator", "DocNum",IFNULL("U_EXX_FE_Estado",'-'), "U_EXX_FE_FOLIOCONT"
	INTO Series,Indicador,DocNum,Estado, FolioContingencia
	FROM ODPI WHERE "DocEntry"= :DocEntry;

	SELECT "FolioPref",LEFT("SeriesName",4), IFNULL("U_FE_Contingencia", 'N'), "U_FE_TipoDocumento"
	INTO FolioPref, SeriesName, Contingencia, TipoDocContingencia
	FROM NNM1 WHERE "Series"=:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';
	
	SELECT IFNULL((SELECT TOP 1("Code") FROM "@EXX_FE_FOLIO_LIB" WHERE "U_FE_Series"=:Series),0) INTO Code FROM DUMMY;

	IF (:Code=0) THEN
		IF (:Contingencia = 'Y') THEN
			Folio := :FolioContingencia;
		ELSE
			SELECT IFNULL("NextFolio", 1) INTO Folio
			FROM NNM1 WHERE "Series"=:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';
		END IF;	
	ELSE 
		SELECT "U_FE_FolioNum" INTO Folio FROM "@EXX_FE_FOLIO_LIB" WHERE "Code"=:Code;
		DELETE FROM "@EXX_FE_FOLIO_LIB" WHERE "Code"=:Code;
	END IF;	
	
	IF (:Estado) !='-' THEN
		UPDATE ODPI
		SET "U_EXX_FE_Estado" = 'NEN'
		WHERE "DocEntry"=:DocEntry ;
	END IF;

	UPDATE ODPI
	SET
		"FolioNum"=:Folio,
		"FolioPref"=:FolioPref,
		"Printed"='Y',
		"U_EXX_FE_TIPCOM"=:CodigoSunat,
		"U_EXX_FE_ClaAcc"=:RUC||'-'||:Indicador||'-'||:SeriesName||'-'||CAST(:Folio AS NVARCHAR)
	WHERE
		"DocEntry"= :DocEntry;
	
--NOTA DE CRÉDITO
ELSEIF (:ObjectType = '14') THEN

	SELECT "Series", "Indicator", "DocNum",IFNULL("U_EXX_FE_Estado",'-'), "U_EXX_FE_FOLIOCONT"
	INTO Series,Indicador, DocNum, Estado, FolioContingencia
	FROM ORIN WHERE "DocEntry"= :DocEntry;

	SELECT "FolioPref",LEFT("SeriesName",4), IFNULL("U_FE_Contingencia", 'N'), "U_FE_TipoDocumento"
	INTO FolioPref,SeriesName, Contingencia, TipoDocContingencia
	FROM NNM1 WHERE "Series"=:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';
	
	SELECT IFNULL((SELECT TOP 1("Code") FROM "@EXX_FE_FOLIO_LIB" WHERE "U_FE_Series"=:Series),0) INTO Code FROM DUMMY;

	IF (:Code=0) THEN
		IF (:Contingencia = 'Y') THEN
			Folio := :FolioContingencia;
		ELSE
			SELECT IFNULL("NextFolio", 1) INTO Folio
			FROM NNM1 WHERE "Series"=:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';
		END IF;	
	ELSE 
		SELECT "U_FE_FolioNum" INTO Folio FROM "@EXX_FE_FOLIO_LIB" WHERE "Code"=:Code;
		DELETE FROM "@EXX_FE_FOLIO_LIB" WHERE "Code"=:Code;
	END IF;	
	
	IF (:Estado) !='-' THEN
		UPDATE ORIN
		SET "U_EXX_FE_Estado" = 'NEN'
		WHERE "DocEntry"=:DocEntry;
	END IF;

	UPDATE ORIN
	SET
		"FolioNum"=:Folio,
		"FolioPref"=:FolioPref,
		"Printed"='Y',
		"U_EXX_FE_TIPCOM"=:CodigoSunat,
		"U_EXX_FE_ClaAcc"=:RUC||'-'||:Indicador||'-'||:SeriesName||'-'||CAST(:Folio AS NVARCHAR)
	WHERE
		"DocEntry"= :DocEntry;

-- CRE
ELSEIF (:ObjectType = '46') THEN

	SELECT "Series", "DocNum", IFNULL("U_EXX_FE_Estado",'-'),IFNULL("U_EXX_SERIECER",''),TO_INT(IFNULL("U_EXX_CORRECER",0)), "U_EXX_FE_FOLIOCONT"
	INTO Series,DocNum, Estado,FolioPref,Folio,FolioContingencia
	FROM OVPM WHERE "DocEntry"= :DocEntry;
	
	SELECT IFNULL("NextFolio", 1), IFNULL("U_FE_Contingencia", 'N') INTO Folio, Contingencia
	FROM NNM1 WHERE "Series" =:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';         
											
	IF (:FolioPref='') THEN
		SELECT LEFT("SeriesName",4) INTO FolioPref FROM OVPM INNER JOIN NNM1 ON OVPM."Series"=NNM1."Series"
		WHERE OVPM."DocEntry"= :DocEntry AND NNM1."U_FE_TipoEmision" ='FE';   
		
		Code  = 0;	
	    	
	END IF;
    
    IF (:Contingencia = 'Y') THEN
    	Folio := :FolioContingencia;
    ELSE
		SELECT IFNULL("NextFolio", 1) INTO Folio
		FROM NNM1 WHERE "Series"=:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';
	END IF;
		
    IF (:Estado) !='-' THEN
			UPDATE OVPM
			SET
				"U_EXX_FE_Estado" = 'NEN',
				"U_EXX_CORRECER"=TO_VARCHAR(:Folio),
				"U_EXX_SERIECER"=:FolioPref,
				"U_EXX_FE_TIPCOM" = :CodigoSunat

			WHERE
				"DocEntry"= :DocEntry;
	END IF;
	
    
 -- CPE
ELSEIF (:ObjectType = '24') THEN

	SELECT "Series", "DocNum", IFNULL("U_EXX_FE_Estado",'-'),IFNULL("U_EXX_SERIEPER",''),TO_INT(IFNULL("U_EXX_CORREPER",0)), "U_EXX_FE_FOLIOCONT"
	INTO Series,DocNum, Estado,FolioPref,Folio,FolioContingencia
	FROM ORCT WHERE "DocEntry"= :DocEntry;
	
	SELECT IFNULL("NextFolio", 1), IFNULL("U_FE_Contingencia", 'N') INTO Folio, Contingencia
	FROM NNM1 WHERE "Series" =:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';         
											
	IF (:FolioPref='') THEN
		SELECT LEFT("SeriesName",4) INTO FolioPref FROM ORCT INNER JOIN NNM1 ON ORCT."Series"=NNM1."Series"
		WHERE ORCT."DocEntry"= :DocEntry AND NNM1."U_FE_TipoEmision" ='FE';   
		
		Code  = 0;	
	    	
	END IF;
    
    IF (:Contingencia = 'Y') THEN
    	Folio := :FolioContingencia;
    ELSE
		SELECT IFNULL("NextFolio", 1) INTO Folio
		FROM NNM1 WHERE "Series"=:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';
	END IF;
		
    IF (:Estado) !='-' THEN
			UPDATE ORCT
			SET
				"U_EXX_FE_Estado" = 'NEN',
				"U_EXX_CORREPER"=TO_VARCHAR(:Folio),
				"U_EXX_SERIEPER"=:FolioPref,
				"U_EXX_FE_TIPCOM"= :CodigoSunat
			WHERE
				"DocEntry"= :DocEntry;
	END IF;
	
    
    
--GUIA POR VENTA
ELSEIF (:ObjectType = '15') THEN

	SELECT "Series", "Indicator", "DocNum",IFNULL("U_EXX_FE_Estado",'-'),"U_EXX_HOAS_STAD"
	INTO Series,Indicador,DocNum,Estado,EstadoEnvioSUNAT
	FROM ODLN WHERE "DocEntry"= :DocEntry;

	SELECT "FolioPref",LEFT("SeriesName",4)
	INTO FolioPref,SeriesName
	FROM NNM1 WHERE "Series"=:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';
	
	SELECT IFNULL((SELECT TOP 1("Code") FROM "@EXX_FE_FOLIO_LIB" WHERE "U_FE_Series"=:Series),0) INTO Code FROM DUMMY;

	IF (:Code=0) THEN
		SELECT IFNULL("NextFolio", 1) INTO Folio
		FROM NNM1 WHERE "Series"=:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';
	
	ELSE 
		SELECT "U_FE_FolioNum" INTO Folio FROM "@EXX_FE_FOLIO_LIB" WHERE "Code"=:Code;
		DELETE FROM "@EXX_FE_FOLIO_LIB" WHERE "Code"=:Code;
	END IF;
	
	IF (:Estado) !='-' THEN
		UPDATE ODLN
		SET "U_EXX_FE_Estado" = 'NEN'
		WHERE "DocEntry"=:DocEntry;
	END IF;

	UPDATE ODLN
	SET
		"FolioNum"=:Folio,
		"FolioPref"=:FolioPref,
		"Printed"='Y',
		"U_EXX_FE_TIPCOM"=:CodigoSunat,
		"U_EXX_FE_ClaAcc"=:RUC||'-'||:Indicador||'-'||:SeriesName||'-'||CAST(:Folio AS NVARCHAR)
	WHERE
		"DocEntry"= :DocEntry;
		
--GUIA POR DEVOLUCION DE COMPRA
ELSEIF (:ObjectType = '21') THEN

	SELECT "Series", "Indicator", "DocNum",IFNULL("U_EXX_FE_Estado",'-')
	INTO Series,Indicador,DocNum,Estado
	FROM ORPD WHERE "DocEntry"= :DocEntry;

	SELECT "FolioPref",LEFT("SeriesName",4)
	INTO FolioPref,SeriesName
	FROM NNM1 WHERE "Series"=:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';
	
	SELECT IFNULL((SELECT TOP 1("Code") FROM "@EXX_FE_FOLIO_LIB" WHERE "U_FE_Series"=:Series),0) INTO Code FROM DUMMY;

	IF (:Code=0) THEN
		SELECT IFNULL("NextFolio", 1) INTO Folio
		FROM NNM1 WHERE "Series"=:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';
	
	ELSE 
		SELECT "U_FE_FolioNum" INTO Folio FROM "@EXX_FE_FOLIO_LIB" WHERE "Code"=:Code;
		DELETE FROM "@EXX_FE_FOLIO_LIB" WHERE "Code"=:Code;
	END IF;
	
	IF (:Estado) !='-' THEN
		UPDATE ORPD
		SET "U_EXX_FE_Estado" = 'NEN'
		WHERE "DocEntry"=:DocEntry;
	END IF;

	UPDATE ORPD
	SET
		"FolioNum"=:Folio,
		"FolioPref"=:FolioPref,
		"Printed"='Y',
		"U_EXX_FE_TIPCOM"=:CodigoSunat,
		"U_EXX_FE_ClaAcc"=:RUC||'-'||:Indicador||'-'||:SeriesName||'-'||CAST(:Folio AS NVARCHAR)
	WHERE
		"DocEntry"= :DocEntry;
		
--GUIA POR SALIDA DE INVENTARIO
ELSEIF (:ObjectType = '60') THEN

	SELECT "Series", '09', "DocNum",IFNULL("U_EXX_FE_Estado",'-')
	INTO Series,Indicador,DocNum,Estado
	FROM OIGE WHERE "DocEntry"= :DocEntry;

	SELECT "FolioPref",LEFT("SeriesName",4)
	INTO FolioPref,SeriesName
	FROM NNM1 WHERE "Series"=:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';
	
	SELECT IFNULL((SELECT TOP 1("Code") FROM "@EXX_FE_FOLIO_LIB" WHERE "U_FE_Series"=:Series),0) INTO Code FROM DUMMY;

	IF (:Code=0) THEN
		SELECT IFNULL("NextFolio", 1) INTO Folio
		FROM NNM1 WHERE "Series"=:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';
	
	ELSE 
		SELECT "U_FE_FolioNum" INTO Folio FROM "@EXX_FE_FOLIO_LIB" WHERE "Code"=:Code;
		DELETE FROM "@EXX_FE_FOLIO_LIB" WHERE "Code"=:Code;
	END IF;
	
	IF (:Estado) !='-' THEN
		UPDATE OIGE
		SET "U_EXX_FE_Estado" = 'NEN',
		"Indicator"= :Indicador
		WHERE "DocEntry"=:DocEntry;
	END IF;

	UPDATE OIGE
	SET
		"FolioNum"=:Folio,
		"FolioPref"=:FolioPref,
		"Printed"='Y',
		"U_EXX_FE_TIPCOM"=:CodigoSunat,
		"U_EXX_FE_ClaAcc"=:RUC||'-'||:Indicador||'-'||:SeriesName||'-'||CAST(:Folio AS NVARCHAR)
	WHERE
		"DocEntry"= :DocEntry;
		
--GUIA POR TRANSFERENCIA ENTRE ALMACENES
ELSEIF (:ObjectType = '67') THEN

	SELECT "Series", '09', "DocNum",IFNULL("U_EXX_FE_Estado",'-')
	INTO Series,Indicador,DocNum,Estado
	FROM OWTR WHERE "DocEntry"= :DocEntry;

	SELECT "FolioPref",LEFT("SeriesName",4)
	INTO FolioPref,SeriesName
	FROM NNM1 WHERE "Series"=:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';
	
	SELECT IFNULL((SELECT TOP 1("Code") FROM "@EXX_FE_FOLIO_LIB" WHERE "U_FE_Series"=:Series),0) INTO Code FROM DUMMY;

	IF (:Code=0) THEN
		SELECT IFNULL("NextFolio", 1) INTO Folio
		FROM NNM1 WHERE "Series"=:Series AND "U_FE_TipoEmision"='FE' AND "Locked" = 'N';
	
	ELSE 
		SELECT "U_FE_FolioNum" INTO Folio FROM "@EXX_FE_FOLIO_LIB" WHERE "Code"=:Code;
		DELETE FROM "@EXX_FE_FOLIO_LIB" WHERE "Code"=:Code;
	END IF;
	
	IF (:Estado) !='-' THEN
		UPDATE OWTR
		SET "U_EXX_FE_Estado" = 'NEN',
		"Indicator"= :Indicador
		WHERE "DocEntry"=:DocEntry;
	END IF;

	UPDATE OWTR
	SET
		"FolioNum"=:Folio,
		"FolioPref"=:FolioPref,
		"Printed"='Y',
		"U_EXX_FE_TIPCOM"=:CodigoSunat,
		"U_EXX_FE_ClaAcc"=:RUC||'-'||:Indicador||'-'||:SeriesName||'-'||CAST(:Folio AS NVARCHAR)
	WHERE
		"DocEntry"= :DocEntry;
END IF;

UPDATE OJDT
	SET
		"FolioNum"=:Folio,
		"FolioPref"=:FolioPref
	WHERE
		"CreatedBy"= :DocEntry AND "TransType"=:ObjectType AND "BaseRef"=:DocNum;

IF (:Code=0) THEN
	UPDATE NNM1
	SET
		"NextFolio"=:Folio + 1
	WHERE
		"Series" = :Series;
END IF;

	IF IFNULL(:Folia,0) = 0 THEN

		IF (:Contingencia = 'N') THEN
			 IF(:ObjectType = '15') THEN
				IF(:EstadoEnvioSUNAT='Y') THEN
					Call "FEX_PE"."FEX_ACCION_Guardar" (
					  0,     --IN IdAccion bigint.  0 Para Insert.  Valor de IdAccion para actualizar registro existente
					  :IdFexCompany,     --IN IdFexCompany int. Valor de Company para documento actual
					 :CodigoSunat, --IN CodigoEntidad varchar(50).
					 :DocEntry,    --IN DocEntry int.
					 :ObjectType,    --IN ObjectType varchar(50).
					 :DocSubType,    --IN DocSubType varchar(50).
					 '100',    --IN IdTipoAccion int.
					 'VIG',    --IN Estado varchar(50).
					 CURRENT_TIMESTAMP --IN FechaCreacion varchar(70)
					);
				END IF;
			ELSE 	
				Call "FEX_PE"."FEX_ACCION_Guardar" (
				  0,     --IN IdAccion bigint.  0 Para Insert.  Valor de IdAccion para actualizar registro existente
				  :IdFexCompany,     --IN IdFexCompany int. Valor de Company para documento actual
				 :CodigoSunat, --IN CodigoEntidad varchar(50).
				 :DocEntry,    --IN DocEntry int.
				 :ObjectType,    --IN ObjectType varchar(50).
				 :DocSubType,    --IN DocSubType varchar(50).
				 '100',    --IN IdTipoAccion int.
				 'VIG',    --IN Estado varchar(50).
				 CURRENT_TIMESTAMP --IN FechaCreacion varchar(70)
				);
			END IF;

		ELSE
			IF (:TipoDocContingencia <> '03') THEN
								
				Call "FEX_PE"."FEX_ACCION_Guardar" (
				  0,     --IN IdAccion bigint.  0 Para Insert.  Valor de IdAccion para actualizar registro existente
				  :IdFexCompany,     --IN IdFexCompany int. Valor de Company para documento actual
				 :CodigoSunat, --IN CodigoEntidad varchar(50).
				 :DocEntry,    --IN DocEntry int.
				 :ObjectType,    --IN ObjectType varchar(50).
				 :DocSubType,    --IN DocSubType varchar(50).
				 '122',    --IN IdTipoAccion int.
				 'VIG',    --IN Estado varchar(50).
				 CURRENT_TIMESTAMP --IN FechaCreacion varchar(70)
				);
				
				
				
				
			END IF;
		
		END IF;
	END IF; -- FOLIA
END;
