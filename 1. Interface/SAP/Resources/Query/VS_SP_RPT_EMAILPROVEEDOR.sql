CREATE PROCEDURE VS_SP_RPT_EMAILPROVEEDOR
(
	docEntry int
)
as
begin
	select 
		T4."CardCode",
		T4."CardName",
		T0."DocDate",
		T6."PrintHeadr",
		T4."LicTradNum" "TaxIdNum",
		case T0."DocCurr" when 'SOL' then  T0."DocTotal" else T0."DocTotalFC" end "DocTotal",
		T0."DocCurr",		
		case when  T0."CashSum" > 0 Then'Efectivo' When T0."CreditSum" > 0 Then'Tarjeta de credito'
		When T0."TrsfrSum" > 0 Then 'Transferencia' Else 'Cheque' End AS "FormaPago",
		T3."U_NMBANCO" "BankName",
		T3."U_CCBANCO" "DflAccount",
		T5."DocDate" "FechaDoc",
		T5."NroSUNAT",
		case left("NroSUNAT",2) 
			when '01' then 'Factura' 
			when '03' then 'Boleta' 
			when '07' then 'Nota de crédito' 
			when '08' then 'Nota de débito' 
			else 'Otros' 
		end "TipoDocumento",
		case T0."DocCurr" when 'SOL' then T1."SumApplied" else T1."AppliedFC" end "SumApplied" 
	from OVPM T0 
	inner join VPM2 T1 			on T0."DocEntry" = T1."DocNum"
	inner join "@VS_PMP1" T2	on T0."DocEntry" = T2."U_IDPAGO" and T1."DocEntry" = T2."U_IDDOC" and T2."U_LINDOC" = T1."DocLine"
	inner join "@VS_OPMP" T3 	on T3."DocEntry" = T2."DocEntry"  
	inner join OCRD T4 			on T0."CardCode" = T4."CardCode"
	--inner join ODSC T5 			on T2."BankCode" = T3."BankCode"
	inner join 
		(
		select
		 	"ObjType",
		 	"DocEntry",
		 	"DocDate",
		 	"DocNum",
		 	"U_BPP_MDTD"||'-'||"U_BPP_MDSD"||'-'||"U_BPP_MDCD" "NroSUNAT" 
		 from OPCH
		 union all 
		 select
		 	"ObjType",
		 	"DocEntry",
		 	"DocDate",
		 	"DocNum",
		 	"U_BPP_MDTD"||'-'||"U_BPP_MDSD"||'-'||"U_BPP_MDCD" "NroSUNAT" 
		 from ORPC
		 union all 
		 select
		 	"ObjType",
		 	"TransId",
		 	"RefDate",
		 	"TransId",
		 	'30'||'-'||"TransId" "NroSUNAT" 
		 from OJDT
		 ) T5 on T1."InvType" = T5."ObjType" and T1."DocEntry" = T5."DocEntry"
	,OADM T6
	where T0."DocEntry" = :docEntry;
end;/*se reduce las lineas ya que daba error por tantas*/