create procedure "bpvs_PD_TN_OP_OrdenTraslado_RTR3"
(
	in Id int,
	in Transaction_Type nchar(1), --> [A]dd, [U]pdate, [D]elete, [C]ancel, C[L]ose
	in Object_Type nvarchar(20), --> SBO Object Type
	out Error_Message nvarchar(256)
)
as
begin
	declare U_EXK_DCNM	int;
	declare MYCOND 			condition;	
	declare exit handler for MYCOND begin end;
	
	if locate('|A|','|' || :Transaction_Type || '|') > 0
	then
		select	ifnull("U_EXK_DCNM", 0)
		into	U_EXK_DCNM			
		from 	"@VS_PD_RTR3"
		where 	"DocEntry" = :Id;
					
		if (:U_EXK_DCNM = 0)
		then
			Error_Message := 'Error al registrar el detalle de lote, el numero del documento referenciado no puede ser ' || :U_EXK_DCNM;
			signal MYCOND;	
		end if;
	end if;	
end;