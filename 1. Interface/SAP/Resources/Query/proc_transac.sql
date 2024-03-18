

if (:object_type = '15'  AND (:transaction_type = 'A' OR :transaction_type = 'U') and IFNULL(:error,0)=0) 
THEN

		select "U_EXX_HOAS_STAD" INTO estadoSUNAT 	FROM ODLN T0 
										where COALESCE(T0."U_EXX_ADDON",'') NOT IN ('FMS','ECM','WTP')
										AND  T0."DocEntry"=:list_of_cols_val_tab_del ;
		 IF(:estadoSUNAT='A')THEN
		 	error := 1000;
			error_message := N'No puede crear la guía con el campo Estado Envío Sunat: Sin Asignar';
		END IF;							


END IF;

