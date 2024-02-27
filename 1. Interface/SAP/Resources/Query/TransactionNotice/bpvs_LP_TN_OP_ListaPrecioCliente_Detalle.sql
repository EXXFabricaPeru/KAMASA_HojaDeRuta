ALTER procedure "bpvs_LP_TN_OP_ListaPrecioCliente_Detalle"(
	in Id nvarchar(256),
	in Transaction_Type nchar(1), --> [A]dd, [U]pdate, [D]elete, [C]ancel, C[L]ose
	in Object_Type nvarchar(20), --> SBO Object Type
	out Error_Message nvarchar(256)
)
as
begin
	declare ITEMCODE	int;
	declare ITEMCODEDESC	nvarchar(256);
	declare CLIENTE	nvarchar(256);
	declare TERRITORIO	nvarchar(256);
	declare CANAL	nvarchar(256);
	declare LPCLIENTE	int;
	declare MYCOND 			condition;	
	declare exit handler for MYCOND begin end;
	
	if ( :Transaction_Type ='A' or  :Transaction_Type='U')
	then
	
		
		SELECT COALESCE(MAX(subquery.count), 1) into ITEMCODE
		FROM (
		    SELECT COUNT(LP1."U_CODARTICULO") AS count
		    FROM "@VS_LP_CLIENTE" LP
		    JOIN "@VS_LP_CLIENTE1" LP1 ON LP."Code" = LP1."Code"
		    WHERE LP."Code" = :Id
		    GROUP BY "U_CODARTICULO"
		) subquery;
				
		if (:ITEMCODE > 1)
		then
			select	LP1."U_CODARTICULO"	into  ITEMCODEDESC
			from 	"@VS_LP_CLIENTE" LP 
			JOIN  "@VS_LP_CLIENTE1" LP1 ON LP."Code"=LP1."Code"
			where 	LP."Code" = :Id
			GROUP BY "U_CODARTICULO"
			HAVING COUNT("U_CODARTICULO") > 1;
		
			Error_Message := 'Error al registrar el detalle, no se puede registrar el mismo artículo ' ||
			'Código :'|| :Id  ||' '|| 
			'Artículo :' || :ITEMCODEDESC;
			signal MYCOND;	
		end if;
		
		SELECT "U_CLIENTE", "U_TERRITORIO", "U_CANAL" INTO CLIENTE,TERRITORIO,CANAL
		FROM "@VS_LP_CLIENTE" LP
		WHERE LP."Code" = :Id;
		
		
		select COUNT (*) into LPCLIENTE
		FROM "@VS_LP_CLIENTE" LP
		where 
		"U_CLIENTE"=:CLIENTE AND
		"U_TERRITORIO"=:TERRITORIO AND
		"U_CANAL"=:CANAL;
		
		if(LPCLIENTE>1)THEN
			Error_Message := LPCLIENTE || ' Ya existe un cliente con el mismo canal, territorio y canal ' ||
			'Cliente :'|| :CLIENTE  ||' '|| 
			'Territorio :'|| :TERRITORIO  ||' '|| 
			'Canal :' || :CANAL;
			signal MYCOND;	
		END IF;
		
		
	end if;	
end;