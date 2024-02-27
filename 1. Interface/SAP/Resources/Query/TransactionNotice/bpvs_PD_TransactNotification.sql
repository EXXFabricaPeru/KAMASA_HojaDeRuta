CREATE PROCEDURE "bpvs_PD_TransactNotification"( 
	IN object_type nvarchar(30), --> SBO Object Type
	IN transaction_type nchar(1), --> [A]dd, [U]pdate, [D]elete, [C]ancel, C[L]ose
	IN num_of_cols_in_key int,
	IN list_of_key_cols_tab_del nvarchar(255),
	IN list_of_cols_val_tab_del nvarchar(255),
	OUT Resultado "tt_LC_TransactNotification"
	)
AS
BEGIN

	--> Valores de retorno
	DECLARE Activar char(1); --> Activa o desactiva las validaciones. Valores: 1-Activa, 0-Desactiva	
	DECLARE MYCOND CONDITION;	
	DECLARE Error INT;
	DECLARE Error_Message NVARCHAR(256);
	DECLARE EXIT HANDLER FOR MYCOND BEGIN END;
	
	Error := 0;
	Error_Message := N'';
	Activar := 1;

	--> Factura de Venta
	IF :Object_Type = '17' AND :Activar = 1
	THEN
		CALL "bpvs_PD_TN_OP_OrdenVenta_17" (:list_of_cols_val_tab_del, :transaction_type,:object_type,:Error_Message);
		IF IFNULL(:Error_Message,'') <> '' 
		THEN
			Resultado = SELECT  17 AS "Error" ,:Error_Message  AS "Error_Message"  FROM DUMMY;
			SIGNAL MYCOND; 
		END IF;
	END	IF;	

	--> Orden de Traslado - Detalle de Lote
	IF :Object_Type = 'VS_PD_RTR3' AND :Activar = 1
	THEN
		CALL "bpvs_PD_TN_OP_OrdenTraslado_RTR3" (:list_of_cols_val_tab_del, :transaction_type, :object_type, :Error_Message);
		IF IFNULL(:Error_Message,'') <> '' 
		THEN
			Resultado = SELECT  17 AS "Error" , :Error_Message  AS "Error_Message"  FROM DUMMY;
			SIGNAL MYCOND; 
		END IF;
	END	IF;	
	
		--> LISTA DE PRECIO X ESCALA
	IF :Object_Type = 'VS_LP_CLIENTE' AND :Activar = 1
	THEN
		CALL "bpvs_LP_TN_OP_ListaPrecioCliente_Detalle" (:list_of_cols_val_tab_del, :transaction_type, :object_type, :Error_Message);
		IF IFNULL(:Error_Message,'') <> '' 
		THEN
			Resultado = SELECT  17 AS "Error" , :Error_Message  AS "Error_Message"  FROM DUMMY;
			SIGNAL MYCOND; 
		END IF;
	END	IF;	

	Resultado = select 0 AS "Error" ,''  AS "Error_Message"  from dummy;

END;