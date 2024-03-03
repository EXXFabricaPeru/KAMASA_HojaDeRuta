create PROCEdure "SP_EXX_HOAS_Reporte"(
in Code nvarchar(50)
)
AS

BEGIN
SELECT * from "@EX_HR_OHGR" OH
JOIN "@EX_HR_HGR1" H1 ON H1."Code"=OH."Code"
where OH."Code"='2024-004';

END		